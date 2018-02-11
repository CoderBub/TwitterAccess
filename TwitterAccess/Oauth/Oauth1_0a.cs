using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TwitterAccess.Oauth
{
	public class Oauth1_0a : OauthBase
	{
		protected const string OAuthVersion = "1.0";
		protected const string OAuthVerifier = "oauth_verifier";

		protected const string HMACSHA1SignatureType = "HMAC-SHA1";
		protected const string PlainTextSignatureType = "PLAINTEXT";
		protected const string RSASHA1SignatureType = "RSA-SHA1";

		/// <summary>
		/// Generate the signature base that is used to produce the signature
		/// </summary>
		/// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
		/// <param name="callbackURL">Url used to redirect the user to enter their credentials and authorize an application.</param>
		/// <param name="consumerKey">The consumer key</param>
		/// <param name="token">The token, if available. If not available pass null or an empty string</param>
		/// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
		/// <param name="verifier">Value used by an application to verify that an application has been authorized by a user.</param>
		/// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
		/// <param name="timeStamp">The current time stamp calculated as the time between now and 1/1/1970 12:00 AM.</param>
		/// <param name="nonce">A random string used to make the network call unique.</param>
		/// <param name="normalizedUrl">Used to return the prepared, normalized url to be used to make the request.</param>
		/// <param name="normalizedRequestParameters">Used to return the prepared, normalized parameters to generate either the query string or post data.</param>
		/// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
		/// <returns>The signature base</returns>
		public string GenerateSignatureBase(Uri url, string callbackURL, string consumerKey, string token, string tokenSecret, string verifier, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters)
		{
			if (token == null)
			{
				token = string.Empty;
			}

			if (tokenSecret == null)
			{
				tokenSecret = string.Empty;
			}

			if (callbackURL == null)
			{
				callbackURL = string.Empty;
			}

			if (string.IsNullOrEmpty(consumerKey))
			{
				throw new ArgumentNullException("consumerKey");
			}

			if (string.IsNullOrEmpty(httpMethod))
			{
				throw new ArgumentNullException("httpMethod");
			}

			if (string.IsNullOrEmpty(signatureType))
			{
				throw new ArgumentNullException("signatureType");
			}

			normalizedUrl = null;
			normalizedRequestParameters = null;

			List<QueryParameter> parameters = GetQueryParameters(url.Query);
			parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
			parameters.Add(new QueryParameter(OAuthNonceKey, nonce));
			parameters.Add(new QueryParameter(OAuthTimestampKey, timeStamp));
			parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signatureType));
			parameters.Add(new QueryParameter(OAuthConsumerKeyKey, consumerKey));

			if (!string.IsNullOrEmpty(callbackURL))
			{
				parameters.Add(new QueryParameter(OAuthCallbackKey, callbackURL));
			}

			if (!string.IsNullOrEmpty(token))
			{
				parameters.Add(new QueryParameter(OAuthTokenKey, token));
			}

			if (!string.IsNullOrEmpty(verifier))
			{
				parameters.Add(new QueryParameter(OAuthVerifier, verifier));
			}

			parameters.Sort(new QueryParameterComparer());

			normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
			if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
			{
				normalizedUrl += ":" + url.Port;
			}
			normalizedUrl += url.AbsolutePath;
			normalizedRequestParameters = NormalizeRequestParameters(parameters);

			StringBuilder signatureBase = new StringBuilder();
			signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
			signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
			signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

			return signatureBase.ToString();
		}

		/// <summary>
		/// Generate the signature value based on the given signature base and hash algorithm
		/// </summary>
		/// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
		/// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
		/// <returns>A base64 string of the hash value</returns>
		public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
		{
			return ComputeHash(hash, signatureBase);
		}

		/// <summary>
		/// Generates a signature using the HMAC-SHA1 algorithm
		/// </summary>      
		/// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
		/// <param name="callbackURL">Url used to redirect the user to enter their credentials and authorize an application.</param>
		/// <param name="consumerKey">The consumer key</param>
		/// <param name="consumerSecret">The consumer seceret</param>
		/// <param name="token">The token, if available. If not available pass null or an empty string</param>
		/// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
		/// <param name="verifier">Value used by an application to verify that an application has been authorized by a user.</param>
		/// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
		/// <param name="timeStamp">The current time stamp calculated as the time between now and 1/1/1970 12:00 AM.</param>
		/// <param name="nonce">A random string used to make the network call unique.</param>
		/// <param name="normalizedUrl">Used to return the prepared, normalized url to be used to make the request.</param>
		/// <param name="normalizedRequestParameters">Used to return the prepared, normalized parameters to generate either the query string or post data.</param>
		/// <returns>A base64 string of the hash value</returns>
		public string GenerateSignature(Uri url, string callbackURL, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier, string httpMethod, string timeStamp, string nonce, out string normalizedUrl, out string normalizedRequestParameters)
		{
			return GenerateSignature(url, callbackURL, consumerKey, consumerSecret, token, tokenSecret, verifier, httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
		}

		/// <summary>
		/// Generates a signature using the specified signatureType 
		/// </summary>      
		/// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
		/// <param name="callbackURL">Url used to redirect the user to enter their credentials and authorize an application.</param>
		/// <param name="consumerKey">The consumer key</param>
		/// <param name="consumerSecret">The consumer seceret</param>
		/// <param name="token">The token, if available. If not available pass null or an empty string</param>
		/// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
		/// <param name="verifier">Value used by an application to verify that an application has been authorized by a user.</param>
		/// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
		/// <param name="timeStamp">The current time stamp calculated as the time between now and 1/1/1970 12:00 AM.</param>
		/// <param name="nonce">A random string used to make the network call unique.</param>
		/// <param name="signatureType">The type of signature to use</param>
		/// <param name="normalizedUrl">Used to return the prepared, normalized url to be used to make the request.</param>
		/// <param name="normalizedRequestParameters">Used to return the prepared, normalized parameters to generate either the query string or post data.</param>		
		/// <returns>A base64 string of the hash value</returns>
		public string GenerateSignature(Uri url, string callbackURL, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters)
		{
			normalizedUrl = null;
			normalizedRequestParameters = null;

			switch (signatureType)
			{
				case SignatureTypes.PLAINTEXT:
					return HttpUtility.UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));
				case SignatureTypes.HMACSHA1:
					string signatureBase = GenerateSignatureBase(url, callbackURL, consumerKey, token, tokenSecret, verifier, httpMethod, timeStamp, nonce, HMACSHA1SignatureType, out normalizedUrl, out normalizedRequestParameters);

					HMACSHA1 hmacsha1 = new HMACSHA1();
					hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret)));

					return GenerateSignatureUsingHash(signatureBase, hmacsha1);
				case SignatureTypes.RSASHA1:
					throw new NotImplementedException();
				default:
					throw new ArgumentException("Unknown signature type", "signatureType");
			}
		}

	}
}
