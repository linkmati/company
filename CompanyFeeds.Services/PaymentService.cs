using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using CompanyFeeds.Configuration;
using System.Globalization;

namespace CompanyFeeds.Services
{
	public class PaymentService
	{
		private const string TransactionTypePayment = "subscr_payment";
		private const string TransactionTypeSignup = "subscr_signup";

		/// <summary>
		/// Processes the payment from Paypal IPN
		/// </summary>
		/// <returns>true if the Payment was processed successfully</returns>
		public static bool ProcessPaymentNotification(NameValueCollection form, SiteConfiguration config)
		{
			//Tasks:
			//Get the values from the form variable.
			//Make a request to paypal to check if the payment is ok.
			//mark it in the database.
			LogPayment(null, "PRE PROCESS LOG: " + form.ToString(), config);

			try
			{
				if (!IsValidForm(form, config))
				{
					return false;
				}
				var identifier = form["custom"];

				if (IsTransactionSignup(form))
				{
					return ProcessSignup(identifier, form, config.Payment);
				}

				if (IsPricePaidValid(form, config.Payment))
				{
					return ProcessPayment(identifier, form, config.Payment);
				}
				else
				{
					LogPayment(identifier, "Price paid error: " + form.ToString(), config);
				}
			}
			catch (Exception ex)
			{
				LogPaymentException(ex, form.ToString(), config);
			}
			return false;
		}

		/// <summary>
		/// Determines is the parameters of the form are valid to be processed
		/// </summary>
		/// <param name="form"></param>
		/// <param name="config"></param>
		/// <returns></returns>
		private static bool IsValidForm(NameValueCollection form, SiteConfiguration config)
		{
			if ((!IsTransactionSupported(form)))
			{
				//if it isn't a payment notification or signup (eot,cancel,...) do not process.
				return false;
			}
			if (!IsReceiverValid(form, config.Payment.Account))
			{
				return false;
			}
			if (config.Payment.ValidatePosting && !IsValidNotification(form, config))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Determines if the receiver of the payment is the same as the Paypal account.
		/// </summary>
		/// <returns></returns>
		private static bool IsReceiverValid(NameValueCollection form, string paymentAccount)
		{
			var receiver = form["receiver_email"];
			if (receiver != null && receiver.ToUpper() == paymentAccount.ToUpper())
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the price paid for a product is correct
		/// </summary>
		private static bool IsPricePaidValid(NameValueCollection form, PaymentConfiguration config)
		{
			var priceAccounted = Convert.ToDecimal(form["mc_gross"], CultureInfo.InvariantCulture);
			var currencyAccounted = (form["mc_currency"] ?? "").ToUpper();
			var productCode = (form["item_number"] ?? "").ToUpper();
			var price = config.PriceYearly;
			if (currencyAccounted != config.Currency.ToUpper())
			{
				return false;
			}
			if (productCode != config.NameYearly.ToUpper())
			{
				price = config.PriceMonthly;
			}
			if (priceAccounted != price)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Determines if the notification is a payment notification
		/// </summary>
		/// <returns></returns>
		private static bool IsTransactionPayment(NameValueCollection form)
		{
			if (form["txn_type"] == TransactionTypePayment)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the notification is a Signup notification
		/// </summary>
		/// <returns></returns>
		private static bool IsTransactionSignup(NameValueCollection form)
		{
			if (form["txn_type"] == TransactionTypeSignup)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the transaction is supported by the payment ipn handler
		/// </summary>
		/// <returns></returns>
		private static bool IsTransactionSupported(NameValueCollection form)
		{
			return IsTransactionPayment(form) || IsTransactionSignup(form);
		}

		/// <summary>
		/// Parses the payment information from the form and sets the payment.
		/// </summary>
		/// <returns></returns>
		public static bool ProcessPayment(string identifier, NameValueCollection form, PaymentConfiguration config)
		{
			//Si status is failed: 
				//rollback
			//Si el status is completed/pending
				//pagado en el sistema (repetidos)
			var userId = Convert.ToInt32(identifier);
			var paymentStatus = GetStatus(form);
			var payerEmail = form["payer_email"];
			var subscriptionId = form["subscr_id"];
			var isYearly = (form["item_number"] ?? "").ToUpper() == config.NameYearly.ToUpper();

			if (paymentStatus == PaymentStatus.Failed)
			{
				UsersService.RollbackPayment(userId);
			}
			else
			{
				//the payment could be pending or completed
				UsersService.SetPayment(userId, paymentStatus, payerEmail, subscriptionId, isYearly);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Parses the signup information
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="form"></param>
		/// <param name="paymentConfiguration"></param>
		/// <returns></returns>
		private static bool ProcessSignup(string identifier, NameValueCollection form, PaymentConfiguration paymentConfiguration)
		{
			var userId = Convert.ToInt32(identifier);
			var payerEmail = form["payer_email"];
			var subscriptionId = form["subscr_id"];
			var user = UsersService.Get(userId);
			if (user.AgencyPremiumDate != null)
			{
				//Payment has already been processed
				return false;
			}
			UsersService.SetPayment(userId, PaymentStatus.Pending, payerEmail, subscriptionId, false);
			return true;
		}

		/// <summary>
		/// POST the data back to PayPal.
		/// </summary>
		/// <param name="httpRequest"></param>
		public static bool IsValidNotification(NameValueCollection form, SiteConfiguration config)
		{
			var result = false;
			var paypalUrl = config.Payment.ServiceUrl;
			var formPostData = "cmd = _notify-validate";
			var charset = Encoding.GetEncoding(form["charset"]);
			
			foreach (String postKey in form)
			{
				string postValue = Encode(form[postKey], charset);
				formPostData += string.Format("&{0}={1}", postKey, postValue);
			}

            //TLS 1.2 on .NET 3.5 
            // https://support.microsoft.com/en-us/help/3154518/support-for-tls-system-default-versions-included-in-the-net-framework
		    ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
			
            var client = new WebClient();
			client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			byte[] postByteArray = Encoding.ASCII.GetBytes(formPostData);
			byte[] responseArray = client.UploadData(paypalUrl, "POST", postByteArray);
			string response = Encoding.ASCII.GetString(responseArray);
			switch (response)
			{
				case "VERIFIED":
					result = true;
					break;
				default:
					// Possible fraud. Log for investigation.
					LogPayment(null, "UNVERIFIED -- Response: " + response + ";Form: " + form.ToString(), config);
					break;
			}

			return result;
		}

		public static PaymentStatus GetStatus(NameValueCollection form)
		{
			string postedPaymentStatus = form["payment_status"].ToUpper();
			PaymentStatus result = PaymentStatus.Failed;

			switch (postedPaymentStatus)
			{
				case "COMPLETED":
					result = PaymentStatus.Completed;
					break;
				case "DENIED":
				case "FAILED":
				case "EXPIRED":
					result = PaymentStatus.Failed;
					break;
				case "PENDING":
					result = PaymentStatus.Pending;
					break;
			}

			return result;
		}

		private static string Encode(string oldValue, Encoding charset)
		{
			string newValue = oldValue.Replace("\"", "'");
			newValue = UrlEncoder(newValue, charset);
			newValue = newValue.Replace("%2f", "/");
			return newValue;
		}

		private static Func<string, Encoding, string> _urlEncoder;
		public static Func<string, Encoding, string> UrlEncoder
		{
			get
			{
				if (_urlEncoder == null)
				{
					_urlEncoder = (value, encoding) => System.Uri.EscapeDataString(value);
				}
				return _urlEncoder;
			}
			set
			{
				_urlEncoder = value;
			}
		}

		/// <summary>
		/// Prepares the message to log
		/// </summary>
		private static void LogPayment(string identifier, string message, SiteConfiguration config)
		{
			if (identifier != null)
			{
				message = "identifier: " + identifier + ";\r\n" + message;
			}
			LogService.LogStatus("Payment log", message);
		}

		private static void LogPaymentException(Exception ex, string extraInfo, SiteConfiguration config)
		{
			LogService.LogException("Payment exception", ex, extraInfo, config.Mail);
		}
	}
}
