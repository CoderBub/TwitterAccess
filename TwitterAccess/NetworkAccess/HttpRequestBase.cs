using System;
using System.IO;
using System.Net;

namespace TwitterAccess.NetworkAccess
{
	/// <summary>
	/// Abstract base class for classes that analyze network information obtained
	/// via HTTP Web requests.
	/// </summary>
	/// <remarks>
	/// This base class implements properties related to HTTP Web requests, a
	/// BackgroundWorker instance, and properties, methods, and events related to
	/// the BackgroundWorker.  The derived class must implement a method to start
	/// an analysis and implement the <see cref="BackgroundWorker_DoWork" />
	/// method.
	/// </remarks>
	public abstract class HttpRequestBase
	{
		#region Properties

		public enum Method { GET, POST };

		/// User agent to use for all Web requests.
		private static string UserAgent
		{
			get
			{
				return "TweetyTrap";
			}
		}

		/// The timeout to use for HTTP Web requests, in milliseconds.
		private const Int32 HttpWebRequestTimeoutMs = 30000;

		/// Time to wait between retries to the HTTP Web service, in seconds.  The
		/// length of the array determines the number of retries: three array
		/// elements means there will be up to three retries, or four attempts
		/// total.
		protected static Int32[] HttpRetryDelaysSec = new Int32[] { 1, 3, 5, };

		protected static HttpStatusCode[] HttpStatusCodesToFailImmediately = new HttpStatusCode[]
		{
            // Occurs when information about a user who has "protected" status
            // is requested, for example.
            HttpStatusCode.Unauthorized,

            // Occurs when the Twitter search API returns a tweet from a user,
            // but then refuses to provide a list of the people followed by the
            // user, probably because the user has protected her account.
            // (But if she has protected her account, why is the search API
            // returning one of her tweets?)
            HttpStatusCode.NotFound,

            // Starting with version 1.1 of the Twitter API, a single HTTP
            // status code (429, "rate limit exceeded") is used for all
            // rate-limit responses.
            (HttpStatusCode)429,

            // Not sure about what causes this one.
            HttpStatusCode.Forbidden,
        };

		#endregion

		#region Web Request Methods

		/// <summary>
		/// Gets an HttpWebRequest object to use.
		/// </summary>
		/// <param name="url">URL to use.</param>
		/// <returns>The HttpWebRequest object.</returns>
		protected static HttpWebRequest CreateHttpWebRequest(String url)
		{
			HttpWebRequest oHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

			// Get the request to work if there is a Web proxy that requires
			// authentication.  More information:
			//
			// http://dangarner.co.uk/2008/03/18/webrequest-proxy-authentication/

			// Although Credentials is a static property that needs to be set once
			// only, setting it here guarantees that no Web request is ever made
			// before the credentials are set.

			WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
			oHttpWebRequest.ServicePoint.Expect100Continue = false;
			oHttpWebRequest.UserAgent = UserAgent;
			oHttpWebRequest.Timeout = HttpWebRequestTimeoutMs;

			return (oHttpWebRequest);
		}

		/// <summary>
		/// Web Request Wrapper
		/// </summary>
		/// <param name="method">Http Method</param>
		/// <param name="url">Full url to the web resource</param>
		/// <param name="postData">Data to post in querystring format</param>
		/// <returns>The web server response.</returns>
		protected HttpWebResponse SendWebRequest(Method method, string url, string postData)
		{
			HttpWebRequest webRequest = null;
			StreamWriter requestWriter = null;

			webRequest = CreateHttpWebRequest(url);
			webRequest.Method = method.ToString();

			if (method == Method.POST)
			{
				webRequest.ContentType = "application/x-www-form-urlencoded";

				//POST the data.
				requestWriter = new StreamWriter(webRequest.GetRequestStream());
				try
				{
					requestWriter.Write(postData);
				}
				catch
				{
					throw new Exception("The system failed to write the post data to the web request.");
				}
				finally
				{
					requestWriter.Close();
					requestWriter = null;
				}
			}

			return GetWebResponseWithRetries(webRequest);
		}

		/// <summary>
		/// Process the web response.
		/// </summary>
		/// <param name="webRequest">The request object.</param>
		/// <returns>The response data.</returns>
		protected string GetWebResponse(HttpWebResponse webResponse)
		{
			StreamReader responseReader = null;
			string responseData = "";

			try
			{
				responseReader = new StreamReader(webResponse.GetResponseStream());
				responseData = responseReader.ReadToEnd();
			}
			catch
			{
				throw new Exception("The system failed to read the response data from the web response.");
			}
			finally
			{
				webResponse.GetResponseStream().Close();
				responseReader.Close();
				responseReader = null;
			}

			return responseData;
		}

		/// <summary>
		/// Gets a HttpWebResponse object.  Retries after an error.
		/// </summary>
		/// <param name="webRequest">HttpWebRequest object that contains the
		/// authorized url and any POST data or GET query strings to be sent.
		/// </param>
		/// <returns>The HttpWebResponse object.</returns>
		/// <remarks>
		/// If the request fails and the HTTP status code is not one of the codes
		/// specified in <paramref name="aeHttpStatusCodesToFailImmediately" />,
		/// the request is retried.  If the retries also fail, an exception is
		/// thrown.
		/// <para>
		/// If the request fails with one of the HTTP status code contained in
		/// <paramref name="aeHttpStatusCodesToFailImmediately" />, an exception is
		/// thrown immediately.
		/// </para>
		/// <para>
		/// In either case, it is always up to the caller to handle the exceptions.
		/// This method never ignores an exception; it either retries it and throws
		/// it if all retries fail, or throws it immediately.
		/// </para>
		/// </remarks>
		protected HttpWebResponse GetWebResponseWithRetries(HttpWebRequest webRequest)
		{
			Int32 iMaximumRetries = HttpRetryDelaysSec.Length;
			Int32 iRetriesSoFar = 0;

			while (true)
			{
				try
				{
					HttpWebResponse response = GetWebResponseNoRetries(webRequest);
					return (response);
				}
				catch (Exception oException)
				{
					if (!(oException is WebException))
						throw oException;

					if (iRetriesSoFar == iMaximumRetries)
						throw (oException);

					// If the status code is one of the ones specified in
					// HttpStatusCodesToFailImmediately, rethrow the exception
					// without retrying the request.
					if (HttpStatusCodesToFailImmediately != null && oException is WebException)
					{
						if (WebExceptionHasHttpStatusCode((WebException)oException, HttpStatusCodesToFailImmediately))
							throw (oException);
					}

					Int32 iSeconds = HttpRetryDelaysSec[iRetriesSoFar];

					System.Threading.Thread.Sleep(1000 * iSeconds);
					iRetriesSoFar++;
				}
			}
		}

		/// <summary>
		/// Gets an HttpWebResponse object.  Does not retry after an error.
		/// </summary>
		/// <param name="webRequest">HttpWebRequest object to use.</param>
		/// <returns>
		/// The HttpWebResponse object.
		/// </returns>
		private HttpWebResponse GetWebResponseNoRetries(HttpWebRequest webRequest)
		{
			webRequest.Timeout = HttpWebRequestTimeoutMs;
			return (HttpWebResponse)webRequest.GetResponse();
		}

		#endregion

		#region Exception Methods

		/// <summary>
		/// Determines whether a WebException has an HttpWebResponse with one of a
		/// specified set of HttpStatusCodes.
		/// </summary>
		/// <param name="oWebException">The WebException to check.</param>
		/// <param name="aeHttpStatusCodes">One or more HttpStatus codes to look for.</param>
		/// <returns>
		/// true if <paramref name="oWebException" /> has an HttpWebResponse with
		/// an HttpStatusCode contained within <paramref name="aeHttpStatusCodes" />.
		/// </returns>
		protected Boolean WebExceptionHasHttpStatusCode(WebException oWebException, params HttpStatusCode[] aeHttpStatusCodes)
		{
			if (!(oWebException.Response is HttpWebResponse))
				return (false);

			HttpWebResponse oHttpWebResponse = (HttpWebResponse)oWebException.Response;

			return (Array.IndexOf<HttpStatusCode>(aeHttpStatusCodes, oHttpWebResponse.StatusCode) >= 0);
		}

		#endregion
	}
}
