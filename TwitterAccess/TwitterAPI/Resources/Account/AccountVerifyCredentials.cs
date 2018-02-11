using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI.Resources.Account
{
	public class AccountVerifyCredentials : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// account/verify_credentials
		/// </summary>
		public override string Name
		{
			get { return "account/verify_credentials"; }
		}

		/// <summary>
		/// account/verify_credentials.json
		/// </summary>
		public override string URL
		{
			get { return "account/verify_credentials.json"; }
		}

		/// <summary>
		/// account
		/// </summary>
		public override string ResourceType
		{
			get { return "account"; }
		}

		/// <summary>
		/// rate_limit_status
		/// </summary>
		public override string ObjectType
		{
			get { return "verify_credentials"; }
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

		public string Results { get; set; }

		#endregion

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional, ParamType.boolean));
			Parameters.Add(new Parameter("skip_status", ParamStatus.Optional, ParamType.boolean));
			Parameters.Add(new Parameter("include_email", ParamStatus.Optional, ParamType.boolean));
		}

		#endregion
	}
}
