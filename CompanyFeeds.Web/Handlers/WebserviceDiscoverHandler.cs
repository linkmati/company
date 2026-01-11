using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace CompanyFeeds.Web.Handlers
{
	public class WebserviceDiscoverHandler : IHttpHandlerFactory
	{
		/// <summary>
		/// Searches all Services and tries to find a class with the specified name
		/// </summary>
		private Type GetServiceType(string TypeName)
		{
			// Todo: Caching mechanism for assembly checks
			foreach (Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type ClassType = loadedAssembly.GetType(TypeName);
				if (ClassType != null)
					return ClassType;
			}
			return null;
		}


		/// <summary>
		/// Use one of the visible attributes in the assembly to get a reference to the "System.Web.Extentions" Library.
		/// </summary>
		Assembly _ajaxAssembly = null;
		Assembly AjaxAssembly
		{
			get
			{
				if (_ajaxAssembly == null)
					_ajaxAssembly = typeof(GenerateScriptTypeAttribute).Assembly;
				return _ajaxAssembly;
			}
		}

		// Used to remember which Factory has been used
		private IHttpHandlerFactory UsedHandlerFactory;

		/// <summary>
		/// Checks the request type and returns the corresponding httpd handler or factory
		/// </summary>
		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			IHttpHandler HttpHandler = null;

			try
			{
				// Request Hosting permissions
				new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

				// Try to get the type associated with the request (On a name to type basis)
				Type WebServiceType = this.GetServiceType(Path.GetFileNameWithoutExtension(pathTranslated));

				// if we did not find any send it on to the original ajax script service handler.
				if (WebServiceType == null)
				{
					// [REFLECTION] Get the internal class System.Web.Script.Services.ScriptHandlerFactory create it.
					IHttpHandlerFactory ScriptHandlerFactory = (IHttpHandlerFactory)System.Activator.CreateInstance(AjaxAssembly.GetType("System.Web.Script.Services.ScriptHandlerFactory"));
					UsedHandlerFactory = ScriptHandlerFactory;
					return ScriptHandlerFactory.GetHandler(context, requestType, url, pathTranslated);
				}

				// [REFLECTION] get the Handlerfactory : RestHandlerFactory (Handles Javascript proxy Generation and actions)
				IHttpHandlerFactory JavascriptHandlerFactory = (IHttpHandlerFactory)System.Activator.CreateInstance(AjaxAssembly.GetType("System.Web.Script.Services.RestHandlerFactory"));

				// [REFLECTION] Check if the current request is a Javasacript method
				// JavascriptHandlerfactory.IsRestRequest(context);
				System.Reflection.MethodInfo IsScriptRequestMethod = JavascriptHandlerFactory.GetType().GetMethod("IsRestRequest", BindingFlags.Static | BindingFlags.NonPublic);
				if ((bool)IsScriptRequestMethod.Invoke(null, new object[] { context }))
				{
					// Remember the used factory for later in ReleaseHandler
					UsedHandlerFactory = JavascriptHandlerFactory;

					// Check and see if it is a Javascript Request or a request for a Javascript Proxy.
					bool IsJavascriptDebug = string.Equals(context.Request.PathInfo, "/jsdebug", StringComparison.OrdinalIgnoreCase);
					bool IsJavascript = string.Equals(context.Request.PathInfo, "/js", StringComparison.OrdinalIgnoreCase);
					if (IsJavascript || IsJavascriptDebug)
					{

						// [REFLECTION] fetch the constructor for the WebServiceData Object
						ConstructorInfo WebServiceDataConstructor = AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Type), typeof(bool) }, null);

						// [REFLECTION] fetch the constructor for the WebServiceClientProxyGenerator
						ConstructorInfo WebServiceClientProxyGeneratorConstructor = AjaxAssembly.GetType("System.Web.Script.Services.WebServiceClientProxyGenerator").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(bool) }, null);

						// [REFLECTION] get the method from WebServiceClientProxy to create the javascript : GetClientProxyScript
						MethodInfo GetClientProxyScriptMethod = AjaxAssembly.GetType("System.Web.Script.Services.ClientProxyGenerator").GetMethod("GetClientProxyScript", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData") }, null);

						// [REFLECTION] We invoke : 
						// new WebServiceClientProxyGenerator(url,false).WebServiceClientProxyGenerator.GetClientProxyScript(new WebServiceData(WebServiceType));
						string Javascript = (string)GetClientProxyScriptMethod.Invoke(
						  WebServiceClientProxyGeneratorConstructor.Invoke(new Object[] { url, IsJavascriptDebug })
						, new Object[] {
                            WebServiceDataConstructor.Invoke(new object[] { WebServiceType, false }) 
                            }
						);

						// The following caching code was copied from the original assembly, read with Reflector, comments were added manualy.
						#region Caching

						// Check the assembly modified time and use it as caching http header
						DateTime AssemblyModifiedDate = GetAssemblyModifiedTime(WebServiceType.Assembly);

						// See "if Modified since" was requested in the http headers, and check it with the assembly modified time
						string s = context.Request.Headers["If-Modified-Since"];

						DateTime TempDate;
						if (((s != null) && DateTime.TryParse(s, out TempDate)) && (TempDate >= AssemblyModifiedDate))
						{
							context.Response.StatusCode = 0x130;
							return null;
						}

						// Add HttpCaching data to the http headers
						if (!IsJavascriptDebug && (AssemblyModifiedDate.ToUniversalTime() < DateTime.UtcNow))
						{
							HttpCachePolicy cache = context.Response.Cache;
							cache.SetCacheability(HttpCacheability.Public);
							cache.SetLastModified(AssemblyModifiedDate);
						}
						#endregion

						// Set Add the javascript to a new custom handler and set it in HttpHandler.
						HttpHandler = new JavascriptProxyHandler(Javascript);
						return HttpHandler;
					}
					else
					{
						IHttpHandler JavascriptHandler = (IHttpHandler)System.Activator.CreateInstance(AjaxAssembly.GetType("System.Web.Script.Services.RestHandler"));

						// [REFLECTION] fetch the constructor for the WebServiceData Object
						ConstructorInfo WebServiceDataConstructor = AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Type), typeof(bool) }, null);

						// [REFLECTION] get method : JavaScriptHandler.CreateHandler
						MethodInfo CreateHandlerMethod = JavascriptHandler.GetType().GetMethod("CreateHandler", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData"), typeof(string) }, null);

						// [REFLECTION] Invoke CreateHandlerMethod :
						// HttpHandler = JavaScriptHandler.CreateHandler(WebServiceType,false);
						HttpHandler = (IHttpHandler)CreateHandlerMethod.Invoke(JavascriptHandler, new Object[]
                                {
                                    WebServiceDataConstructor.Invoke(new object[] { WebServiceType, false })
                                ,   context.Request.PathInfo.Substring(1)
                                }
						);
					}
					return HttpHandler;
				}
				else
				{
					// Remember the used factory for later in ReleaseHandler
					IHttpHandlerFactory WebServiceHandlerFactory = new WebServiceHandlerFactory();
					UsedHandlerFactory = WebServiceHandlerFactory;

					// [REFLECTION] Get the method CoreGetHandler
					MethodInfo CoreGetHandlerMethod = UsedHandlerFactory.GetType().GetMethod("CoreGetHandler", BindingFlags.NonPublic | BindingFlags.Instance);

					// [REFLECTION] Invoke the method CoreGetHandler :
					// WebServiceHandlerFactory.CoreGetHandler(WebServiceType,context,context.Request, context.Response);
					HttpHandler = (IHttpHandler)CoreGetHandlerMethod.Invoke(UsedHandlerFactory, new object[]
{
    WebServiceType, context, context.Request, context.Response
}
					);
					return HttpHandler;
				}
			}
			// Because we are using Reflection, errors generated in reflection will be an InnerException, 
			// to get the real Exception we throw the InnerException it.
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Release the handler in the used factory
		/// </summary>
		/// <param name="handler"></param>
		public void ReleaseHandler(IHttpHandler handler)
		{
			if (UsedHandlerFactory != null)
				UsedHandlerFactory.ReleaseHandler(handler);

		}

		private static DateTime GetAssemblyModifiedTime(Assembly assembly)
		{
			DateTime lastWriteTime = File.GetLastWriteTime(new Uri(assembly.GetName().CodeBase).LocalPath);
			return new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day, lastWriteTime.Hour, lastWriteTime.Minute, 0);
		}


	}

	/// <summary>
	/// A custom handler to deliver the generated Javascript.
	/// </summary>
	internal class JavascriptProxyHandler : IHttpHandler
	{
		public JavascriptProxyHandler(string _javascript)
		{
			Javascript = _javascript;
		}

		string Javascript = "";

		#region IHttpHandler Members

		bool IHttpHandler.IsReusable
		{
			get
			{
				return false;
			}
		}

		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			context.Response.Write(this.Javascript);
		}

		#endregion
	}

}
