using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Services;
using CompanyFeeds.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Globalization;
using System.Net;
using System.IO;

namespace CompanyFeeds.Tests.Services
{
	[TestClass]
	public class PaymentServiceTests
	{
		[TestMethod]
		public void PaymentService_ProcessPayment_Test1()
		{
			bool result = false;
			var user = TestHelper.GetAnyUser();
			var form = HttpUtility.ParseQueryString("transaction_subject=Premium+subscription+prsync.com&payment_date=05%3a58%3a11+Apr+19%2c+2012+PDT&txn_type=subscr_payment&subscr_id=I-JJKEBY7BT7S0&last_name=User&residence_country=US&item_name=Premium+subscription+prsync.com&payment_gross=550.00&mc_currency=USD&business=info_1220350160_biz%40tagpoint.es&payment_type=instant&protection_eligibility=Ineligible&verify_sign=AxLUkHRFlqD6SOedCTlJHNPfKWVQALHlz8rTcBCRrqMQanM1Eq624o7k&payer_status=verified&test_ipn=1&payer_email=soport_1220350310_per%40tagpoint.es&txn_id=4U962042A1358823E&receiver_email=info_1220350160_biz%40tagpoint.es&first_name=Test&payer_id=9CNSH8BSPE2FC&receiver_id=AW3QSMCN4SKJY&item_number=prsync_com-premium-yearly&payment_status=Completed&payment_fee=16.25&mc_fee=16.25&mc_gross=550.00&custom=a123456&charset=windows-1252&notify_version=3.4&ipn_track_id=63546888326df");

			var config = new PaymentConfiguration()
			{
				Currency = form["mc_currency"],
				NameYearly = form["item_number"],
				PriceYearly = Convert.ToDecimal(form["mc_gross"], CultureInfo.InvariantCulture)
			};

			result = PaymentService.ProcessPayment(user.Id.ToString(), form, config);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void PaymentService_ProcessPaymentNotification_Receiver_Test()
		{
			bool result = false;
			var user = TestHelper.GetAnyUser();
			var form = HttpUtility.ParseQueryString("transaction_subject=Premium+subscription+prsync.com&payment_date=05%3a58%3a11+Apr+19%2c+2012+PDT&txn_type=subscr_payment&subscr_id=I-JJKEBY7BT7S0&last_name=User&residence_country=US&item_name=Premium+subscription+prsync.com&payment_gross=550.00&mc_currency=USD&business=info_1220350160_biz%40tagpoint.es&payment_type=instant&protection_eligibility=Ineligible&verify_sign=AxLUkHRFlqD6SOedCTlJHNPfKWVQALHlz8rTcBCRrqMQanM1Eq624o7k&payer_status=verified&test_ipn=1&payer_email=soport_1220350310_per%40tagpoint.es&txn_id=4U962042A1358823E&receiver_email=info_1220350160_biz%40tagpoint.es&first_name=Test&payer_id=9CNSH8BSPE2FC&receiver_id=AW3QSMCN4SKJY&item_number=prsync_com-premium-yearly&payment_status=Completed&payment_fee=16.25&mc_fee=16.25&mc_gross=550.00&custom=a123456&charset=windows-1252&notify_version=3.4&ipn_track_id=63546888326df");

			var config = SiteConfiguration.Load();
			config.Payment = new PaymentConfiguration()
			{
				//account that does not match the receiver email
				Account = "nonexistent@gmail.com",
				ValidatePosting = false
			};

			result = PaymentService.ProcessPaymentNotification(form, config);
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void PaymentService_ProcessPaymentNotification_Signup_Test()
		{
			bool result = false;
			var user = TestHelper.GetAnyUser();
			var form = HttpUtility.ParseQueryString("txn_type=subscr_signup&subscr_id=I-NVAXUVL43J2K&last_name=Bay+Gondra&residence_country=ES&mc_currency=USD&item_name=Premium+subscription+prsync.com&business=jorgebg%40tagpoint.es&amount3=1.00&recurring=1&verify_sign=A5uQFvjc.UJtfUjlRK-VIf9WWQ8HAB.lr09Q6suFuLIwAXgrSXUK-wlA&payer_status=verified&payer_email=jorgebaygondra%40gmail.com&first_name=Jorge&receiver_email=jorgebg%40tagpoint.es&payer_id=6QLAGBE3FQGSS&reattempt=1&item_number=prsync_com-premium-yearly&subscr_date=05%3a19%3a46+Apr+23%2c+2012+PDT&custom=40&charset=windows-1252&notify_version=3.4&period3=1+Y&mc_amount3=1.00&ipn_track_id=aa34d42571888");

			var config = SiteConfiguration.Load();
			config.Payment = new PaymentConfiguration()
			{
				//account that does not match the receiver email
				Account = form["receiver_email"],
				ValidatePosting = false
			};

			UsersService.RollbackPayment(Convert.ToInt32(form["custom"]));

			result = PaymentService.ProcessPaymentNotification(form, config);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void PaymentService_ProcessPaymentNotification_PaymentTest()
		{
			bool result = false;
			var user = TestHelper.GetAnyUser();
			var form = HttpUtility.ParseQueryString("transaction_subject=Premium+subscription+prsync.com&payment_date=05%3a19%3a48+Apr+23%2c+2012+PDT&txn_type=subscr_payment&subscr_id=I-NVAXUVL43J2K&last_name=Bay+Gondra&residence_country=ES&item_name=Premium+subscription+prsync.com&payment_gross=1.00&mc_currency=USD&business=jorgebg%40tagpoint.es&payment_type=instant&protection_eligibility=Ineligible&verify_sign=AZxbwZ9bPVPFFf7hCCNemacLJwlCAMxalBW8qkIXRQzrCdwRbdX1j.YU&payer_status=verified&payer_email=jorgebaygondra%40gmail.com&txn_id=73K42468XH5803514&receiver_email=jorgebg%40tagpoint.es&first_name=Jorge&payer_id=6QLAGBE3FQGSS&receiver_id=DGGNURBDR4S2L&item_number=prsync_com-premium-yearly&payment_status=Completed&payment_fee=0.33&mc_fee=0.33&mc_gross=1.00&custom=40&charset=windows-1252&notify_version=3.4&ipn_track_id=aa34d42571888");

			var config = SiteConfiguration.Load();
			config.Payment = new PaymentConfiguration()
			{
				Account = form["receiver_email"],
				Currency = form["mc_currency"],
				NameYearly = form["item_number"],
				PriceYearly = Convert.ToDecimal(form["mc_gross"], CultureInfo.InvariantCulture),
				ValidatePosting = false
			};

			result = PaymentService.ProcessPaymentNotification(form, config);
			Assert.IsTrue(result);

			form = HttpUtility.ParseQueryString("txn_type=subscr_cancel&subscr_id=I-NVAXUVL43J2K&last_name=Bay+Gondra&residence_country=ES&mc_currency=USD&item_name=Premium+subscription+prsync.com&business=jorgebg%40tagpoint.es&amount3=1.00&recurring=1&verify_sign=Az-7lYaCY5RXQ2BJcd7C3M0cHeqTAERtip9YprYNwrrzrMhmGos.x1b6&payer_status=verified&payer_email=jorgebaygondra%40gmail.com&first_name=Jorge&receiver_email=jorgebg%40tagpoint.es&payer_id=6QLAGBE3FQGSS&reattempt=1&item_number=prsync_com-premium-yearly&subscr_date=05%3a29%3a34+Apr+23%2c+2012+PDT&custom=40&charset=windows-1252&notify_version=3.4&period3=1+Y&mc_amount3=1.00&ipn_track_id=1d883377631d2");
			result = PaymentService.ProcessPaymentNotification(form, config);
			Assert.IsFalse(result);

		}

		[TestMethod]
		public void PaymentService_ProcessPaymentNotification_SampleTest()
		{
			bool result = false;
			var user = TestHelper.GetAnyUser();
			var form = HttpUtility.ParseQueryString(String.Format("transaction_subject=Premium+subscription+prsync.com&payment_date=06%3a19%3a01+Apr+27%2c+2012+PDT&txn_type=subscr_payment&subscr_id=I-GENRLLMKSY32&last_name=User&residence_country=US&item_name=Premium+subscription+prsync.com&payment_gross=55.00&mc_currency=USD&business=info_1220350160_biz%40tagpoint.es&payment_type=instant&protection_eligibility=Ineligible&verify_sign=A4dJROhD9xMflDCSEQLgb.eZ4zupAfKAXN1fJSh5-ejOc4cnWYaxmOi7&payer_status=verified&test_ipn=1&payer_email=soport_1220350310_per%40tagpoint.es&txn_id=3K569650T8211403W&receiver_email=info_1220350160_biz%40tagpoint.es&first_name=Test&payer_id=9CNSH8BSPE2FC&receiver_id=AW3QSMCN4SKJY&item_number=prsync_com-premium-monthly&payment_status=Completed&payment_fee=1.90&mc_fee=1.90&mc_gross=55.00&custom={0}&charset=windows-1252&notify_version=3.4&ipn_track_id=10705e90cb6cf", user.Id));

			var config = SiteConfiguration.Load();
			config.Payment = new PaymentConfiguration()
			{
				Account = form["receiver_email"],
				Currency = form["mc_currency"],
				NameYearly = form["item_number"],
				PriceYearly = Convert.ToDecimal(form["mc_gross"], CultureInfo.InvariantCulture),
				ValidatePosting = false
			};

			result = PaymentService.ProcessPaymentNotification(form, config);
			Assert.IsTrue(result);
		}

	    [TestMethod]
	    public void PaymentService_Paypal_Tls_Test()
	    {
	        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
	        ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
            var client = new WebClient();
	        var text = client.DownloadString("https://tlstest.paypal.com/");
            Assert.AreEqual("PayPal_Connection_OK", text);
	    }
	}
}
