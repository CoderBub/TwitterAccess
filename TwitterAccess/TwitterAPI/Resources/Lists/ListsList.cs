using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
	public class ListsList : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// lists/list
		/// </summary>
		public override string Name
		{
			get { return "lists/list"; }
		}

		/// <summary>
		/// lists/list.json
		/// </summary>
		public override string URL
		{
			get { return "lists/list.json"; }
		}

		/// <summary>
		/// lists
		/// </summary>
		public override string ResourceType
		{
			get { return "lists"; }
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

		#region Protected Members

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("user_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("reverse", ParamStatus.Optional));
		}

		#endregion
	}
}
