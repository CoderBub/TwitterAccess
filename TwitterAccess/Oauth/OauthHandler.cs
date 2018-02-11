using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace TwitterAccess.Oauth
{
	public class OauthHandler
	{
		#region Private Constant Strings

		/// URI of the Twitter OAuth API.
		private const string OAuthApiUri = "https://api.twitter.com/";

		private const string OAuth1 = "oauth/";

		private const string OAuth2 = "oauth2/";

		private const string REQUEST_TOKEN = OAuthApiUri + OAuth1 + "request_token";

		private const string AUTHENTICATE = OAuthApiUri + OAuth1 + "authenticate";

		private const string ACCESS_TOKEN = OAuthApiUri + OAuth1 + "access_token";

		private const string TOKEN = OAuthApiUri + OAuth2 + "token";

		#endregion

		#region Public Properties

		public enum Method { GET, POST };

		public string CallBackUrl { get; set; }

		public string CallBackMethod { get; set; }

		public string ConsumerKey { get; set; }

		public string ConsumerSecret { get; set; }

		public string Signature { get; set; }

		public string Token { get; set; }

		public string TokenSecret { get; set; }

		public string Verifier { get; set; }

		public string UserAgent { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// This method sets the Token property with the Authentication token to use
		/// when getting a user to authenticate the application to make api calls on
		/// the user's behalf.
		/// </summary>
		/// <returns>URL to redirect the user to in order to authenticate the application.</returns>
		public string GetAuthenticationToken()
		{
			if (string.IsNullOrEmpty(ConsumerKey) || string.IsNullOrEmpty(ConsumerSecret))
				throw new Exception("You must provide both a consumer key and consumer secret to retrieve an Authentication token.");

			if (string.IsNullOrEmpty(CallBackUrl))
				CallBackUrl = "oob";

			return RequestToken();
		}

		/// <summary>
		/// This method is used to confirm that all authentication data has been set in the
		/// object (Consumer Key and Secret, and Authentication token and token verifier),
		/// then this calls the private method to get the access token.
		/// </summary>
		public void GetAccessToken()
		{
			if (string.IsNullOrEmpty(ConsumerKey) || string.IsNullOrEmpty(ConsumerSecret))
				throw new Exception("You must provide both a consumer key and consumer secret to retrieve an Authentication token.");

			if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(Verifier))
				throw new Exception("You must provide an authentication token and verifier.");

			RetrieveAccessToken();
		}

		/// <summary>
		/// Generates an authentication signature that can be used to make api calls 
		/// that require authorization.
		/// </summary>
		/// <param name="url">The url of the api resource.</param>
		/// <param name="method">The method used to transmit the request (GET or POST).</param>
		/// <param name="postData">The data to be used as either a query string or post data.</param>
		/// <param name="authorizedUrl">The parameter used to return the authorized url.</param>
		/// <param name="authorizedPostData">The parameter used to return the normalized post data.</param>
		public void GetApiSignature(string url, Method method, string postData, out string authorizedUrl, out string authorizedPostData)
		{
			ConstructAuthWebRequest(method, url, postData, out authorizedUrl, out authorizedPostData);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Get the link to Twitter's authorization page for this application.
		/// </summary>
		/// <returns>The url with a valid request token.</returns>
		private string RequestToken()
		{
			string ret = null;

			string response = oAuthWebRequest(Method.POST, REQUEST_TOKEN, String.Empty);
			if (response.Length > 0)
			{
				//response contains token and token secret.  We only need the token.
				NameValueCollection qs = HttpUtility.ParseQueryString(response);
				if (qs["oauth_token"] != null)
				{
					ret = AUTHENTICATE + "?oauth_token=" + qs["oauth_token"];
					this.Token = qs["oauth_token"];
				}

				if (qs["oauth_token_secret"] != null)
					this.TokenSecret = qs["oauth_token_secret"];
			}
			else
				throw new WebException(
					"Can't get the link to the Twitter authorization page.");

			return ret;
		}

		/// <summary>
		/// This method retrieves the Access Token and Token Secret to allow the application
		/// to make api calls on behalf of a specific user.  These values are set in public
		/// properties that the application can access.
		/// </summary>
		private void RetrieveAccessToken()
		{
			string response = oAuthWebRequest(Method.POST, ACCESS_TOKEN, String.Empty);

			if (response.Length > 0)
			{
				//Store the Token and Token Secret
				NameValueCollection qs = HttpUtility.ParseQueryString(response);
				if (qs["oauth_token"] != null)
					this.Token = qs["oauth_token"];
				else
					throw new WebException("An access token wasn't sent by Twitter.");

				if (qs["oauth_token_secret"] != null)
					this.TokenSecret = qs["oauth_token_secret"];
				else
					throw new WebException("An access token secret wasn't sent by Twitter.");
			}
			else
				throw new WebException("Can't exchange the request token for an access token.");
		}

		/// <summary>
		/// Submit a web request using oAuth.
		/// </summary>
		/// <param name="method">GET or POST</param>
		/// <param name="url">The full url, including the querystring.</param>
		/// <param name="postData">Data to post (querystring format)</param>
		/// <returns>The web server response.</returns>
		private string oAuthWebRequest(Method method, string url, string postData)
		{
			String authorizedUrl, authorizedPostData;

			ConstructAuthWebRequest(method, url, postData, out authorizedUrl, out authorizedPostData);

			return (WebResponseData(method, authorizedUrl, authorizedPostData));
		}

		/// <summary>
		/// Web Request Wrapper
		/// </summary>
		/// <param name="method">Http Method</param>
		/// <param name="url">Full url to the web resource</param>
		/// <param name="postData">Data to post in querystring format</param>
		/// <returns>The web server response.</returns>
		private string WebResponseData(Method method, string url, string postData)
		{
			HttpWebRequest webRequest = null;
			StreamWriter requestWriter = null;
			string responseData = "";

			webRequest = CreateHttpWebRequest(url);
			webRequest.Method = method.ToString();
			webRequest.ServicePoint.Expect100Continue = false;
			webRequest.UserAgent = UserAgent;

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
					throw;
				}
				finally
				{
					requestWriter.Close();
					requestWriter = null;
				}
			}

			responseData = WebResponseGet(webRequest);

			webRequest = null;

			return responseData;
		}

		/// <summary>
		/// Process the web response.
		/// </summary>
		/// <param name="webRequest">The request object.</param>
		/// <returns>The response data.</returns>
		private string WebResponseGet(HttpWebRequest webRequest)
		{
			StreamReader responseReader = null;
			string responseData = "";

			try
			{
				responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
				responseData = responseReader.ReadToEnd();
			}
			catch
			{
				throw;
			}
			finally
			{
				webRequest.GetResponse().GetResponseStream().Close();
				responseReader.Close();
				responseReader = null;
			}

			return responseData;
		}

		/// <summary>
		/// Gets an HttpWebRequest object to use.
		/// </summary>
		/// <param name="url">URL to use.</param>
		/// <returns>The HttpWebRequest object.</returns>
		private static HttpWebRequest CreateHttpWebRequest(String url)
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

			return (oHttpWebRequest);
		}

		/// <summary>
		/// Constructs the URL and POST data to use for a web request using
		/// oAuth.
		/// </summary>
		/// <param name="method">GET or POST</param>
		/// <param name="url">The full url, including the querystring.</param>
		/// <param name="postData">Data to post (querystring format)</param>
		/// <param name="authorizedUrl">The url with authorization information added.</param>
		/// <param name="authorizedPostData">The POST data with authorization information added.</param>
		private void ConstructAuthWebRequest(Method method, string url, string postData, out String authorizedUrl, out String authorizedPostData)
		{
			string outUrl = "";
			string querystring = "";

			Oauth1_0a Oauth1 = new Oauth1_0a();
			//Setup postData for signing.
			//Add the postData to the querystring.
			if (method == Method.POST)
			{
				if (postData.Length > 0)
				{
					//Decode the parameters and re-encode using the oAuth UrlEncode method.
					NameValueCollection qs = HttpUtility.ParseQueryString(postData);
					postData = "";
					foreach (string key in qs.AllKeys)
					{
						if (postData.Length > 0)
						{
							postData += "&";
						}
						qs[key] = HttpUtility.UrlDecode(qs[key]);
						qs[key] = Oauth1.UrlEncode(qs[key]);
						postData += key + "=" + qs[key];

					}
					if (url.IndexOf("?") > 0)
					{
						url += "&";
					}
					else
					{
						url += "?";
					}
					url += postData;
				}
			}

			Uri uri = new Uri(url);

			string nonce = Oauth1.GenerateNonce();
			string timeStamp = Oauth1.GenerateTimeStamp();

			//Generate Signature
			string sig = Oauth1.GenerateSignature(uri,
				this.CallBackUrl,
				this.ConsumerKey,
				this.ConsumerSecret,
				this.Token,
				this.TokenSecret,
				this.Verifier,
				method.ToString(),
				timeStamp,
				nonce,
				out outUrl,
				out querystring);

			querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);

			//Convert the querystring to postData
			if (method == Method.POST)
			{
				postData = querystring;
				querystring = "";
			}

			if (querystring.Length > 0)
			{
				outUrl += "?";
			}

			authorizedUrl = outUrl + querystring;
			authorizedPostData = postData;
		}

		#endregion
	}
}
