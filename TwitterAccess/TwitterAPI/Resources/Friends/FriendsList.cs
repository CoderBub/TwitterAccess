using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class FriendsList : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// friends/list
		/// </summary>
		public override string Name
		{
			get { return "friends/list"; }
		}

		/// <summary>
		/// friends/list.json
		/// </summary>
		public override string URL
		{
			get { return "friends/list.json"; }
		}

		/// <summary>
		/// friends
		/// </summary>
		public override string ResourceType
		{
			get { return "friends"; }
		}

		/// <summary>
		/// list
		/// </summary>
		public override string ObjectType
		{
			get { return "list"; }
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

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("user_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("cursor", ParamStatus.Optional));
			Parameters.Add(new Parameter("skip_status", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_user_entities", ParamStatus.Optional));
		}

		#endregion
	}
}
