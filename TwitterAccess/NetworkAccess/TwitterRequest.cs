using System;
using System.Net;
using System.Text;
using TwitterAccess.Oauth;
using TwitterAccess.TwitterAPI;

namespace TwitterAccess.NetworkAccess
{
	/// <summary>
	/// Class used to access the Twitter Api.
	/// </summary>
	public class TwitterRequest : HttpRequestBase
	{
		#region Public Properties

		/// <summary>
		/// Twitter Consumer Key for the application making the network request.
		/// </summary>
		public string ConsumerKey { get; set; }

		/// <summary>
		/// Twitter Consumer Secret for the application making the network request.
		/// </summary>
		public string ConsumerSecret { get; set; }

		/// <summary>
		/// Token obtained from the Twitter user for whom the application is making 
		/// the network request on behalf of.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// Token secret obtained from the Twitter user for whom the application is making 
		/// the network request on behalf of.
		/// </summary>
		public string TokenSecret { get; set; }

		/// <summary>
		/// Verifier used by the application to obtain an Authorization Token in order
		/// to make network requests on behalf of a Twitter user.
		/// </summary>
		public string Verifier { get; set; }

		#endregion

		#region Private Properties

		/// <summary>
		/// The url that is returned by the Oauth Handler.  Suitable for using in a web request.
		/// </summary>
		private string AuthorizedUrl { get; set; }

		/// <summary>
		/// The normalized post data returned by the Oauth Handler.
		/// </summary>
		private string AuthorizedPostData { get; set; }

		/// <summary>
		/// Used to generate authentication tokens and signatures for API calls requiring
		/// authorization.
		/// </summary>
		private OauthHandler oAuthHandler { get; set; }

		/// <summary>
		/// URI of the Twitter REST API.
		/// </summary>
		private const String RestApiUri = "https://api.twitter.com/1.1/";

		/// <summary>
		/// URI of the Twitter Rate Limit Api.
		/// </summary>
		private const String RateLimitUri = "https://api.twitter.com/1.1/application/rate_limit_status.json";

		#endregion

		#region OAuth Methods

		/// <summary>
		/// Gets an authentication url to be used to redirect a Twitter user to enter
		/// their credentials and authorize the application to make api calls on the 
		/// user's behalf.
		/// </summary>
		/// <returns>Twitter authentication url.</returns>
		public string GetAuthenticationLink()
		{
			InitializeOauthHandler();

			oAuthHandler.ConsumerKey = ConsumerKey;
			oAuthHandler.ConsumerSecret = ConsumerSecret;

			string authenticationLink = oAuthHandler.GetAuthenticationToken();

			try
			{
				Token = oAuthHandler.Token;
				TokenSecret = oAuthHandler.TokenSecret;

				return authenticationLink;
			}
			catch (WebException exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// Gets the Token Key and Secret for the application to make api calls on 
		/// behalf of a Twitter user.
		/// </summary>
		public void GetAccessToken()
		{
			InitializeOauthHandler();

			oAuthHandler.ConsumerKey = ConsumerKey;
			oAuthHandler.ConsumerSecret = ConsumerSecret;
			oAuthHandler.Token = Token;
			oAuthHandler.Verifier = Verifier;

			try
			{
				oAuthHandler.GetAccessToken();
				Token = oAuthHandler.Token;
				TokenSecret = oAuthHandler.TokenSecret;
			}
			catch (WebException exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// Uses the Oauth keys and secrets to generate an authentication signature for
		/// the API url and query that you want to send to twitter.
		/// </summary>
		/// <param name="url">The url for your api call.</param>
		/// <param name="query">The query string to include in your api call.</param>
		private void GenerateSignature(string url, string query)
		{
			InitializeOauthHandler();

			string authUrl, postData;
			oAuthHandler.ConsumerKey = ConsumerKey;
			oAuthHandler.ConsumerSecret = ConsumerSecret;
			oAuthHandler.Token = Token;
			oAuthHandler.TokenSecret = TokenSecret;

			//TODO:  Change this so that the method is not always set to GET
			oAuthHandler.GetApiSignature(url, OauthHandler.Method.GET, query, out authUrl, out postData);

			AuthorizedUrl = authUrl;
			AuthorizedPostData = postData;
		}

		/// <summary>
		/// Initializes the OauthHandler object if it has not already bee instantiated.
		/// </summary>
		private void InitializeOauthHandler()
		{
			if (oAuthHandler == null)
				oAuthHandler = new OauthHandler();
		}

		#endregion

		#region Network Access Methods

		/// <summary>
		/// Makes a call to the twitter API and returns the contents of the response object
		/// in a string format.
		/// </summary>
		/// <param name="apiCall">The API object holding information about the type of call to be performed</param>
		/// <returns>String representing JSON data returned from Twitter API</returns>
		public string GetTwitterData(ApiBase apiCall)
		{
			string url = apiCall.URL;
			string query = apiCall.GetQuery();

			Method newMethod = (Method)Enum.Parse(typeof(Method), apiCall.MethodType);

			if (newMethod == Method.GET)
			{
				url += "?" + query;
				query = "";
			}

			if (apiCall.NeedsAuthorization)
			{
				CheckConsurmerKeyAndSecret();
				CheckTokenKeyAndSecret();
			}

			GenerateSignature(url, query);

			try
			{
				HttpWebResponse response = SendWebRequest(newMethod, AuthorizedUrl, AuthorizedPostData);
				return GetWebResponse(response);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		#endregion

		#region Validate Api Call Methods

		/// <summary>
		/// For API calls that require authorization, this method confirms that the Consurmer Key
		/// and Consumer Secret properties have been populated.  If not, an exception is thrown.
		/// </summary>
		private void CheckConsurmerKeyAndSecret()
		{
			if (string.IsNullOrEmpty(ConsumerKey) || string.IsNullOrEmpty(ConsumerSecret))
			{
				StringBuilder message = new StringBuilder();
				message.Append("You must provide your ");

				if (string.IsNullOrEmpty(ConsumerKey))
				{
					message.Append("Consumer Key ");

					if (string.IsNullOrEmpty(ConsumerSecret))
						message.Append("and Consumer Secret ");
				}
				else
					message.Append("Consumer Secret ");

				message.Append("in order to make this Api call.");

				throw new Exception(message.ToString());
			}
		}

		/// <summary>
		/// For API calls that require authorization, this method confirms that the Token
		/// and Token Secret properties have been populated.  If not, an exception is thrown. 
		/// </summary>
		private void CheckTokenKeyAndSecret()
		{
			if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(TokenSecret))
			{
				StringBuilder message = new StringBuilder();
				message.Append("You must provide your ");

				if (string.IsNullOrEmpty(Token))
				{
					message.Append("Token ");

					if (string.IsNullOrEmpty(TokenSecret))
						message.Append("and Token Secret ");
				}
				else
					message.Append("Token Secret ");

				message.Append("in order to make this Api call.");

				throw new Exception(message.ToString());
			}
		}

		public string CheckRateLimit()
		{
			GenerateSignature(RateLimitUri, String.Empty);

			HttpWebResponse response = SendWebRequest(Method.GET, AuthorizedUrl, AuthorizedPostData);
			return GetWebResponse(response);
		}

		#endregion
	}
}
