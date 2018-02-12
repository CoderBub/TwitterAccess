using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class RateLimitCheck : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// application/rate_limit_status
		/// </summary>
		public override string Name
		{
			get { return "application/rate_limit_status"; }
		}

		/// <summary>
		/// application/rate_limit_status.json
		/// </summary>
		public override string URL
		{
			get { return "application/rate_limit_status.json"; }
		}

		/// <summary>
		/// application
		/// </summary>
		public override string ResourceType
		{
			get { return "application"; }
		}

		/// <summary>
		/// rate_limit_status
		/// </summary>
		public override string ObjectType
		{
			get { return "rate_limit_status"; }
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

			Parameters.Add(new Parameter("resources", ParamStatus.Optional, ParamType.text));
		}

		#endregion
	}
}
