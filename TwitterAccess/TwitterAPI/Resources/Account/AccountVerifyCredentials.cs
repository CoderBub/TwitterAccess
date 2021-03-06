﻿using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class AccountVerifyCredentials : ApiBase
	{
		#region Public Properties

		public override string Name
		{
			get { return "account/verify_credentials"; }
		}

		public override string URL
		{
			get { return "account/verify_credentials.json"; }
		}

		public override string ResourceType
		{
			get { return "account"; }
		}

		public override string ObjectType
		{
			get { return "verify_credentials"; }
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
