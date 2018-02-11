using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class StatusesUserTimeline : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// statuses/user_timeline.json
		/// </summary>
		public override string Name
		{
			get { return "statuses/user_timeline.json"; }
		}

		/// <summary>
		/// statuses/user_timeline.json
		/// </summary>
		public override string URL
		{
			get { return "statuses/user_timeline.json"; }
		}

		/// <summary>
		/// statuses
		/// </summary>
		public override string ResourceType
		{
			get { return "statuses"; }
		}

		/// <summary>
		/// user_timeline
		/// </summary>
		public override string ObjectType
		{
			get { return "user_timeline"; }
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
						if (value > 200)
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

			Parameters.Add(new Parameter("user_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("since_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("count", ParamStatus.Optional));
			Parameters.Add(new Parameter("max_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("trim_user", ParamStatus.Optional));
			Parameters.Add(new Parameter("exclude_replies", ParamStatus.Optional));
			Parameters.Add(new Parameter("contributor_details", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_rts", ParamStatus.Optional));
		}

		#endregion
	}
}
