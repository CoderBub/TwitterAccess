using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class StatusShow : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// statuses/show
		/// </summary>
		public override string Name
		{
			get { return "statuses/show"; }
		}

		/// <summary>
		/// statuses/show.json
		/// </summary>
		public override string URL
		{
			get { return "statuses/show.json"; }
		}

		/// <summary>
		/// statuses
		/// </summary>
		public override string ResourceType
		{
			get { return "statuses"; }
		}

		/// <summary>
		/// show/:id
		/// </summary>
		public override string ObjectType
		{
			get { return "show/:id"; }
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

			Parameters.Add(new Parameter("id", ParamStatus.Required));
			Parameters.Add(new Parameter("trim_user", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_my_retweet", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional));
		}

		#endregion
	}
}
