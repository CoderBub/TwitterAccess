using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class ListsOwnerships : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// lists/ownerships
		/// </summary>
		public override string Name
		{
			get { return "lists/ownerships"; }
		}

		/// <summary>
		/// lists/ownerships.json
		/// </summary>
		public override string URL
		{
			get { return "lists/ownerships.json"; }
		}

		/// <summary>
		/// lists
		/// </summary>
		public override string ResourceType
		{
			get { return "lists"; }
		}

		/// <summary>
		/// ownerships
		/// </summary>
		public override string ObjectType
		{
			get { return "ownerships"; }
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
			Parameters.Add(new Parameter("count", ParamStatus.Optional));
			Parameters.Add(new Parameter("cursor", ParamStatus.Optional));
		}

		#endregion
	}
}
