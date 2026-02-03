using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CompanyFeeds.FeedClient
{
	public delegate void WebResponseHandler(FeedProxy sender, Stream responseStream);
}
