using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class UsersContributees : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// users/contributees
		/// </summary>
		public override string Name
		{
			get { return "users/contributees"; }
		}

		/// <summary>
		/// users/contributees.json
		/// </summary>
		public override string URL
		{
			get { return "users/contributees.json"; }
		}

		/// <summary>
		/// users
		/// </summary>
		public override string ResourceType
		{
			get { return "users"; }
		}

		/// <summary>
		/// contributees
		/// </summary>
		public override string ObjectType
		{
			get { return "contributees"; }
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
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional));
			Parameters.Add(new Parameter("skip_status", ParamStatus.Optional));
		}

		#endregion
	}
}
