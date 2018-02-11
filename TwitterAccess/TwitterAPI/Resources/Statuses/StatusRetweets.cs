using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class StatusRetweets : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// statuses/retweets/:id
		/// </summary>
		public override string Name
		{
			get { return "statuses/retweets/:id"; }
		}

		/// <summary>
		/// statuses/retweets/{0}.json
		/// </summary>
		public override string URL
		{
			get { return "statuses/retweets/{0}.json"; }
		}

		/// <summary>
		/// statuses
		/// </summary>
		public override string ResourceType
		{
			get { return "statuses"; }
		}

		/// <summary>
		/// retweets/:id
		/// </summary>
		public override string ObjectType
		{
			get { return "retweets/:id"; }
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
			get { return true; }
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

		public override string  GetUrl()
		{
			Parameter par = Parameters.FirstOrDefault(x => x.Key == "id");
			return string.Format(RestApiUrl + URL, par.Value);
		}

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

			Parameters.Add(new Parameter("id", ParamStatus.Required));
			Parameters.Add(new Parameter("count", ParamStatus.Optional));
			Parameters.Add(new Parameter("trim_user", ParamStatus.Optional));
		}

		#endregion
	}
}
