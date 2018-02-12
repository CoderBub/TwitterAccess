using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class BlocksIds : ApiBase
	{
		#region Public Properties

		public override string Name
		{
			get { return "blocks/ids"; }
		}

		public override string URL
		{
			get { return "blocks/ids.json"; }
		}

		public override string ResourceType
		{
			get { return "blocks"; }
		}

		public override string ObjectType
		{
			get { return "ids"; }
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
			Parameters.Add(new Parameter("stringify_ids", ParamStatus.Optional, ParamType.boolean));
		}

		#endregion
	}
}
