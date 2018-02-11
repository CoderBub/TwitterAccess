using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class UsersContributors : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// users/contributors
		/// </summary>
		public override string Name
		{
			get { return "users/contributors"; }
		}

		/// <summary>
		/// users/contributors.json
		/// </summary>
		public override string URL
		{
			get { return "users/contributors.json"; }
		}

		/// <summary>
		/// users
		/// </summary>
		public override string ResourceType
		{
			get { return "users"; }
		}

		/// <summary>
		/// contributors
		/// </summary>
		public override string ObjectType
		{
			get { return "contributors"; }
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
