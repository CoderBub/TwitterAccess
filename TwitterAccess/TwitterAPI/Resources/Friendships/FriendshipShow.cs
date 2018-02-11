using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class FriendshipShow : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// friendships/show
		/// </summary>
		public override string Name
		{
			get { return "friendships/show"; }
		}

		/// <summary>
		/// friendships/show.json
		/// </summary>
		public override string URL
		{
			get { return "friendships/show.json"; }
		}

		/// <summary>
		/// friendships
		/// </summary>
		public override string ResourceType
		{
			get { return "friendships"; }
		}

		/// <summary>
		/// show
		/// </summary>
		public override string ObjectType
		{
			get { return "show"; }
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

			Parameters.Add(new Parameter("source_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("source_screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("target_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("target_screen_name", ParamStatus.Optional));
		}

		#endregion
	}
}
