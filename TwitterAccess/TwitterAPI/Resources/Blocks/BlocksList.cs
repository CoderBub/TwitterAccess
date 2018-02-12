using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class BlocksList : ApiBase
	{
		#region Public Properties

		public override string Name
		{
			get { return "blocks/list"; }
		}

		public override string URL
		{
			get { return "blocks/list.json"; }
		}

		public override string ResourceType
		{
			get { return "blocks"; }
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

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("cursor", ParamStatus.Optional, ParamType.number));
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional, ParamType.boolean));
			Parameters.Add(new Parameter("skip_status", ParamStatus.Optional, ParamType.boolean));
		}

		#endregion
	}
}
