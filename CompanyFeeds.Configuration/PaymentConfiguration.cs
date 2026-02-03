using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CompanyFeeds.Configuration
{
	public class PaymentConfiguration 
	{
		public PaymentConfiguration()
		{
			ValidatePosting = true;
		}
		/// <summary>
		/// Paypal url
		/// </summary>
		[XmlAttribute]
		public string ServiceUrl
		{
			get;
			set;
		}

		/// <summary>
		/// The url of the action that handles the PaymentNotification (IPN)
		/// </summary>
		[XmlAttribute]
		public string NotificationUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Paypal Account
		/// </summary>
		[XmlAttribute]
		public string Account
		{
			get;
			set;
		}
		/// <summary>
		/// Price of the subscription per year
		/// </summary>
		[XmlAttribute]
		public decimal PriceYearly
		{
			get;
			set;
		}

		/// <summary>
		/// Code of the yearly subscription "product"
		/// </summary>
		[XmlAttribute]
		public string NameYearly
		{
			get;
			set;
		}

		/// <summary>
		/// Price of the subscription per year
		/// </summary>
		[XmlAttribute]
		public decimal PriceMonthly
		{
			get;
			set;
		}

		/// <summary>
		/// Code of the monthly subscription "product"
		/// </summary>
		[XmlAttribute]
		public string NameMonthly
		{
			get;
			set;
		}

		/// <summary>
		/// Code of the currency (3 char code, ISO-4217)
		/// </summary>
		[XmlAttribute]
		public string Currency
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if the IPN handler will repost the data. True: default
		/// </summary>
		[XmlAttribute]
		public bool ValidatePosting
		{ 
			get; 
			set; 
		}
	}
}
