using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO.Compression;
using System.Xml;
using System.IO;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.FeedClient
{
	public class FeedProxy
	{
		#region Props
		public FeedClientConfiguration Configuration
		{
			get;
			set;
		}

		private WebResponseHandler _responseHandler;

		public WebResponseHandler ResponseHandler
		{
			get
			{
				return _responseHandler;
			}
			set
			{
				_responseHandler = value;
			}
		}

		/// <summary>
		/// Original stream length of the last stream retrieved, before decompressing.
		/// </summary>
		public long LastStreamLength
		{
			get;
			set;
		}

		#endregion
	
		/// <summary>
		/// Instanciates a new Feed proxy with a async response handler.
		/// </summary>
		/// <param name="handler">In order to do get feeds asynchronously.</param>
		public FeedProxy(WebResponseHandler handler, FeedClientConfiguration config) : this(config)
		{
			this.ResponseHandler = handler;
		}

		public FeedProxy(FeedClientConfiguration config)
		{
			Configuration = config;
		}

		#region Create web client
		private WebClient CreateWebClient()
		{
			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;
			if (Configuration.Proxy != null && Configuration.Proxy.UseProxy)
			{
				client.Proxy = new WebProxy(Configuration.Proxy.Address, Configuration.Proxy.Port);
				client.Proxy.Credentials = new NetworkCredential(Configuration.Proxy.UserName, Configuration.Proxy.Password, Configuration.Proxy.Domain);
			}
			client.Headers["Accept-Encoding"] = "gzip";
			client.Headers["Accept"] = "*/*";
			client.Headers["Accept-Language"] = Configuration.AcceptLanguage;
			client.Headers["User-Agent"] = Configuration.UserAgent;
			return client;
		} 
		#endregion

		#region Get Feeds Async
		public void GetFeedAsync(string url)
		{ 
			if (this.ResponseHandler == null)
			{
				throw new ArgumentException("A response handler must be defined in order to get feeds asynchronously.");
			}
			WebClient client = this.CreateWebClient();
			client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
			client.OpenReadAsync(new Uri(url));
		}

		public Stream GetFeed(string url)
		{
			WebClient client = this.CreateWebClient();
			Stream stream = client.OpenRead(url);
			if (client.ResponseHeaders["Content-Length"] != null)
			{
				this.LastStreamLength = Convert.ToInt32(client.ResponseHeaders["Content-Length"]);
			}
			
			if (client.ResponseHeaders["Content-Encoding"] != null && client.ResponseHeaders["Content-Encoding"].ToLower().Contains("gzip"))
			{
				stream = new GZipStream(stream, CompressionMode.Decompress);
			}
			return stream;
		}

		private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			WebClient client = (WebClient)sender;
			Stream result;
			if (client.ResponseHeaders["Content-Encoding"] != null && client.ResponseHeaders["Content-Encoding"].ToLower().Contains("gzip"))
			{
				result = new GZipStream(e.Result, CompressionMode.Decompress);
			}
			else
			{
				result = e.Result;
			}
			client.Dispose();
			ResponseHandler(this, result);
		}
		#endregion

		#region Get Feeds sync
		public Stream GetFeeds(string url)
		{
			WebClient client = this.CreateWebClient();

			Stream result = client.OpenRead(new Uri(url));
			if (client.ResponseHeaders["Content-Encoding"] != null && client.ResponseHeaders["Content-Encoding"].ToLower().Contains("gzip"))
			{
				result = new GZipStream(result, CompressionMode.Decompress);
			}
			return result;
		}
		#endregion
	}
}
