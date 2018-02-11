using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace TwitterAccess.TwitterAPI
{
	public class FollowersList : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// followers/list
		/// </summary>
		public override string Name
		{
			get { return "followers/list"; }
		}

		/// <summary>
		/// followers/list.json
		/// </summary>
		public override string URL
		{
			get { return "followers/list.json"; }
		}

		/// <summary>
		/// followers
		/// </summary>
		public override string ResourceType
		{
			get { return "followers"; }
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
