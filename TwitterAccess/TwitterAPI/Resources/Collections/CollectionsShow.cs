using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class CollectionsShow : ApiBase
	{
		#region Public Properties

		public override string Name
		{
			get { return "collections/show"; }
		}

		public override string URL
		{
			get { return "collections/show.json"; }
		}

		public override string ResourceType
		{
			get { return "collections"; }
		}

		public override string ObjectType
		{
			get { return "show"; }
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

		#region Public Methods

		public override bool CheckRequiredParameters()
		{
			if (base.CheckRequiredParameters())
			{
				Parameter id = Parameters.FirstOrDefault(x => x.Key == "id");

				if (id == null || string.IsNullOrEmpty(id.Value))
				{
					return false;
				}

				return true;
			}

			return false;
		}

		#endregion

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("id", ParamStatus.Required, ParamType.text));
		}

		#endregion
	}
}
