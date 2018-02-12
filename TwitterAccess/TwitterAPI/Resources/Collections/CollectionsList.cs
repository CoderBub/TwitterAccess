using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class CollectionsList : ApiBase
	{
		#region Public Properties

		public override string Name
		{
			get { return "collections/list"; }
		}

		public override string URL
		{
			get { return "collections/list.json"; }
		}

		public override string ResourceType
		{
			get { return "collections"; }
		}

		public override string ObjectType
		{
			get { return "list"; }
		}

		public override string MethodType
		{
			get { return "GET"; }
		}

		public override bool NeedsAuthorization
		{
			get { return true; }
		}

		public override bool RateLimited
		{
			get { return true; }
		}

		public string Results { get; set; }

		#endregion

		#region Public Methods

		public override bool CheckRequiredParameters()
		{
			if (base.CheckRequiredParameters())
			{
				Parameter userID = Parameters.FirstOrDefault(x => x.Key == "user_id");
				Parameter screenName = Parameters.FirstOrDefault(x => x.Key == "screen_name");
				Parameter count = Parameters.FirstOrDefault(x => x.Key == "count");

				if ((screenName == null || string.IsNullOrEmpty(screenName.Value)) || (userID == null || string.IsNullOrEmpty(userID.Value)))
				{
					return false;
				}

				if (count != null)
				{
					int value;
					if (int.TryParse(count.Value, out value))
					{
						if (value > 200)
							return false;
					}
				}

				return true;
			}

			return false;
		}

		#endregion

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("user_id", ParamStatus.Required, ParamType.number));
			Parameters.Add(new Parameter("screen_name", ParamStatus.Required, ParamType.text));
			Parameters.Add(new Parameter("tweet_id", ParamStatus.Optional, ParamType.number));
			Parameters.Add(new Parameter("count", ParamStatus.Optional, ParamType.number, 200));
			Parameters.Add(new Parameter("cursor", ParamStatus.Optional, ParamType.number));
		}

		#endregion
	}
}
