using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TwitterAccess.TwitterAPI
{
	public class SearchTweets : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// search/tweets
		/// </summary>
		public override string Name
		{
			get { return "search/tweets"; }
		}

		/// <summary>
		/// search/tweets.json
		/// </summary>
		public override string URL
		{
			get { return "search/tweets.json"; }
		}

		/// <summary>
		/// search
		/// </summary>
		public override string ResourceType
		{
			get { return "search"; }
		}

		/// <summary>
		/// tweets
		/// </summary>
		public override string ObjectType
		{
			get { return "tweets"; }
		}

		/// <summary>
		/// GET
		/// </summary>
		public override string MethodType
		{
			get { return "GET"; }
		}

		/// <summary>
		/// true
		/// </summary>
		public override bool NeedsAuthorization
		{
			get { return false; }
		}

		/// <summary>
		/// true
		/// </summary>
		public override bool RateLimited
		{
			get { return true; }
		}

		#endregion

		#region Public Methods

		public override bool CheckRequiredParameters()
		{
			if (base.CheckRequiredParameters())
			{
				Parameter count = Parameters.FirstOrDefault(x => x.Key == "count");

				if (count != null)
				{
					int value;
					if (int.TryParse(count.Value, out value))
					{
						if (value > 100)
							return false;
					}
				}
			}

			return true;
		}

		#endregion

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("q", ParamStatus.Required));
			Parameters.Add(new Parameter("geocode", ParamStatus.Optional));
			Parameters.Add(new Parameter("lang", ParamStatus.Optional));
			Parameters.Add(new Parameter("locale", ParamStatus.Optional));
			Parameters.Add(new Parameter("result_type", ParamStatus.Optional));
			Parameters.Add(new Parameter("count", ParamStatus.Optional));
			Parameters.Add(new Parameter("until", ParamStatus.Optional));
			Parameters.Add(new Parameter("since_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("max_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional));
			Parameters.Add(new Parameter("callback", ParamStatus.Optional));
		}

		public override string GetQuery()
		{
			StringBuilder query = new StringBuilder();

			if (CheckRequiredParameters())
			{

				foreach (Parameter param in Parameters)
				{
					if (param.Key == "q")
					{
						if (query.Length > 0)
							query.Append("&");

						query.Append(param.Key + "=" + HttpUtility.UrlPathEncode(param.Value));
					}
					else if (!string.IsNullOrEmpty(param.Value))
					{
						if (query.Length > 0)
							query.Append("&");

						query.Append(param.Key + "=" + param.Value);
					}
				}
			}

			return query.ToString();
		}

		#endregion
	}
}
