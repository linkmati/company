using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using CompanyFeeds.Web.State;
using CompanyFeeds.DataAccess.Queries;
using System.Web;
using System.Web.Script.Serialization;
using CompanyFeeds.Services;
using CompanyFeeds.Validation;

namespace CompanyFeeds.Web.Helpers
{
	public static class SecurityHelper
	{
        private const string keyMemberCookie = "member";
        private const string Secret = "whatILIKEaboutyou";

		public static UserState Login(SessionWrapper session, HttpCookieCollection cookies, string email, string password)
		{
			//Retrieve from dataaccess
			UsersQueries.UsersLoginRow userLogin = UsersService.GetLogin(email, password);
			if (userLogin == null)
			{
				return null;
			}

			//SetState
			SetState(session, userLogin);

			//WriteCookie
			WriteMemberCookie(cookies, session.User);

			return session.User;
		}

		public static UserState Login(SessionWrapper session, HttpCookieCollection responseCookies, int userId, Guid guid)
		{
			//Retrieve from service
			UsersQueries.UsersLoginRow userLogin = UsersService.GetLogin(userId, guid);
			if (userLogin == null)
			{
				return null;
			}

			SetState(session, userLogin);

			WriteMemberCookie(responseCookies, session.User);

			return session.User;
		}

		public static string ReadMemberCookie(HttpCookieCollection requestCookies, HttpCookieCollection responseCookies, SessionWrapper session)
		{
			//Get the cookie
            HttpCookie cookie = requestCookies[keyMemberCookie];
            session.CookieRetrieved = true;
			if (cookie == null)
            {
                return "failed cookie";
			}
		    var id = Convert.ToInt32(cookie.Values["id"]);
		    var d = Convert.ToInt64(cookie.Values["d"]);
            if (DateTimeOffset.UtcNow.Ticks - d * TimeSpan.TicksPerMinute > 120 * TimeSpan.TicksPerMinute)
            {
                return "failed time";
		    }
		    var user = UsersService.Get(id);
		    if (user == null)
            {
                return "failed id";
		    }
		    if (!CompareHash(cookie.Values["h"], new[] { id.ToString(), user.Guid.ToString(), d.ToString()}))
		    {
		        return "failed hash";
		    }
            Login(session, responseCookies, id, user.Guid);
		    return "OK";
		}

		/// <summary>
		/// Set the user to the session
		/// </summary>
		private static void SetState(SessionWrapper session, UsersQueries.UsersLoginRow user)
		{
			#region Get the agencyId
			string agencyTag = null;
			if ((!user.IsAgencyIdNull()) && (!user.IsAgencyValidatedNull()) && user.AgencyValidated)
			{
				agencyTag = user.AgencyTag;
			} 
			#endregion
			session.User = new UserState(user.UserId, user.UserGuid, user.UserName, user.UserEmail, agencyTag, user.UserEmailActive, user.UserUpdates, user.IsPremium);
		}

		/// <summary>
		/// Writes a cookie to the client by adding a cookie to the response
		/// </summary>
		/// <param name="cookies">Response cookies</param>
		/// <param name="user"></param>
		public static void WriteMemberCookie(HttpCookieCollection cookies, UserState user)
		{
		    try
		    {
		        HttpCookie cookie = new HttpCookie(keyMemberCookie);
		        var d = (DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMinute).ToString();
		        cookie.Values.Add("id", user.Id.ToString());
		        cookie.Values.Add("d", d);
		        cookie.Values.Add("h", ComputeHash(new[] { user.Id.ToString(), user.Guid.ToString(), d}));
		        cookie.HttpOnly = true;
		        cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 0, 30, 0));
		        cookies.Add(cookie);
		    }
		    catch (Exception)
		    {
		        //Do nothing!
		    }
		}

	    private static string ComputeHash(string[] values)
	    {
	        var buffer = Encoding.UTF8.GetBytes(String.Join("", new [] { Secret }.Concat(values).ToArray()));
	        byte[] hash;
            using (SHA512 sha = new SHA512Managed())
            {
                hash = sha.ComputeHash(buffer);
            }
	        return Convert.ToBase64String(hash);
	    }

	    private static bool CompareHash(string base64Hash, string[] values)
	    {
	        return base64Hash == ComputeHash(values);
	    }

		public static void Logout(SessionWrapper session)
		{
			session.User = null;
		}

        public static void ValidateCaptcha(NameValueCollection form, String ipAddress)
	    {
	        var value = form["g-recaptcha-response"];

	        using (var client = new WebClient())
	        {
	            var values = new NameValueCollection();
                values["secret"] = "6Lc7F5gUAAAAADrvRb4euejl0BYeVdwdu_cvbEIF";
                values["response"] = value;
	            values["remoteip"] = ipAddress;

                var response = client.UploadValues("https://www.google.com/recaptcha/api/siteverify", values);

	            var responseString = Encoding.UTF8.GetString(response);

	            var serializer = new JavaScriptSerializer();
	            var result = (Dictionary<String, Object>) serializer.DeserializeObject(responseString);

	            //{ "success": true, "challenge_ts": "2019-03-16T19:18:02Z", "hostname": "localhost" }
	            if (!result["success"].Equals(true))
	            {
                    var ex = new ValidationException(new List<ValidationError>());
	                ex.ValidationErrors.Add(new ValidationError("captcha", ValidationErrorType.CompareNotMatch));
                    throw ex;
	            }
	        }
	    }
	}
}
