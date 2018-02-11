using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class ListsStatuses : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// lists/statuses
		/// </summary>
		public override string Name
		{
			get { return "lists/statuses"; }
		}

		/// <summary>
		/// lists/statuses.json
		/// </summary>
		public override string URL
		{
			get { return "lists/statuses.json"; }
		}

		/// <summary>
		/// lists
		/// </summary>
		public override string ResourceType
		{
			get { return "lists"; }
		}

		/// <summary>
		/// statuses
		/// </summary>
		public override string ObjectType
		{
			get { return "statuses"; }
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

		#region Public Methods

		public override bool CheckRequiredParameters()
		{
			if (base.CheckRequiredParameters())
			{
				Parameter listID = Parameters.FirstOrDefault(x => x.Key == "list_id");
				Parameter slug = Parameters.FirstOrDefault(x => x.Key == "slug");

				if (((listID != null) && (!string.IsNullOrEmpty(listID.Value))) || ((slug != null) && (!string.IsNullOrEmpty(slug.Value))))
					return true;
			}

			return false;
		}

		#endregion

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("list_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("slug", ParamStatus.Optional));
			Parameters.Add(new Parameter("owner_screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("owner_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("since_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("max_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("count", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_rts", ParamStatus.Optional));
		}

		#endregion
	}
}
