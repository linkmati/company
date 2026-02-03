using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;

namespace CompanyFeeds.Tests.Fakes
{


    public class FakeHttpContext : HttpContextBase
    {
        private readonly FakePrincipal _principal;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly SessionStateItemCollection _sessionItems;
		private readonly string _url;
		private FakeHttpRequest _request;

        public FakeHttpContext(string url, FakePrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems )
        {
			_url = url;
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;
			_request = new FakeHttpRequest(_url, _formParams, _queryStringParams, _cookies);
        }

        public override HttpRequestBase Request
        {
            get
            {
				return _request;
            }
        }

        public override IPrincipal User
        {
            get
            {
                return _principal;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(_sessionItems);
            }
        }

		private FakeHttpResponse _response = new FakeHttpResponse();
		public override HttpResponseBase Response
		{
			get
			{
				return _response;
			}
		}

		public override System.Web.Caching.Cache Cache
		{
			get
			{
				return HttpRuntime.Cache;
			}
		}

    }


}
