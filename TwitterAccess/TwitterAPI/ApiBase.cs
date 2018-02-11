using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAccess.TwitterAPI
{
	/// <summary>
	/// The Base Class for All TwitterAPI objects.  This class
	/// contains common characteristics and functionality that must
	/// be implemented by all TwitterAPI objects.
	/// </summary>
	public abstract class ApiBase
	{
		#region Protected Fields

		/// <summary>
		/// The URL for the Twitter Rest API.
		/// </summary>
		protected const string RestApiUrl = "https://api.twitter.com/1.1/";

		#endregion

		#region Public Abstract Properties

		/// <summary>
		/// The Name of the API resource.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Specific URL defined by the inheriting class;
		/// </summary>
		public abstract string URL { get; }

		/// <summary>
		/// The type of api resource to be called (help, users, statuses, etc..)
		/// This is used to make a call to the rate limit api.
		/// This is defined by the inheriting class.
		/// </summary>
		public abstract string ResourceType { get; }
		
		/// <summary>
		/// The specific object type of the Api resource to be called.  This is used to
		/// make a call to the rate limit api. This is defined by the inheriting class.
		/// </summary>
		public abstract string ObjectType { get; }
	
		/// <summary>
		/// The Web Request method to be used to make this Api call (GET or POST)
		/// This is defined by the inheriting class.
		/// </summary>
		public abstract string MethodType { get; }
		
		/// <summary>
		/// Determines whether or not the specific api resource requires authorization.
		/// This is defined by the inheriting class.
		/// </summary>
		public abstract bool NeedsAuthorization { get; }
		
		/// <summary>
		/// Determines whether or not the specific api resource is rate limited.
		/// This is defined by the inheriting class.
		/// </summary>
		public abstract bool RateLimited { get; }
		
		#endregion

		#region Public Properties

		/// <summary>
		/// List of paramaters, both required and optional, used to query
		/// the api resource.
		/// </summary>
		public List<Parameter> Parameters { get; set; }

		/// <summary>
		/// JSON results returned from the Twitter API parsed into a string.
		/// </summary>
		public string Response { get; set; }

		#endregion

		#region Constructor

		public ApiBase()
		{
			GetParameters();
		}

		#endregion

		#region Public Virtual Methods

		/// <summary>
		/// Used to get the specific url needed to make the api resource call.
		/// </summary>
		/// <returns>URL as a string</returns>
		public virtual string GetUrl()
		{
			return RestApiUrl + URL;
		}

		/// <summary>
		/// This method takes the list of parameters and formats them into an acceptable 
		/// query string.
		/// </summary>
		/// <returns>Query string</returns>
		public virtual string GetQuery()
		{
			StringBuilder query = new StringBuilder();

			if (CheckRequiredParameters())
			{
				foreach (Parameter param in Parameters)
				{
					if (!string.IsNullOrEmpty(param.Value))
					{
						if (query.Length > 0)
							query.Append("&");

						query.Append(param.Key + "=" + param.Value);
					}
				}
			}

			return query.ToString();
		}

		/// <summary>
		/// Checks to make sure all required paramters have a valid value. For resources
		/// with no required parameters (only optional parameters), this base method will
		/// suffice.  For resources with required parameters, override this method, call the
		/// base method to pick up this functionality, then add checks for the required parameters.
		/// </summary>
		/// <returns>True or false</returns>
		public virtual bool CheckRequiredParameters()
		{
			if (Parameters == null)
				return false;

			if (Parameters.All(x => String.IsNullOrEmpty(x.Value)))
				return false;

			List<Parameter> required = Parameters.Where(x => x.Status == ParamStatus.Required).ToList();
			if (required != null)
			{
				if (required.Any(x => string.IsNullOrEmpty(x.Value)))
					return false;
			}

			return true;
		}

		#endregion

		#region Protected Abstract Methods

		/// <summary>
		/// Gets ths list of both required and optional parameters for this query.
		/// </summary>
		/// <returns>List of paramter objects</returns>
		protected abstract void GetParameters();

		#endregion
	}
}
