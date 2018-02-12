using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class CollectionsEntities : ApiBase
	{
		#region Public Properties

		public override string Name
		{
			get { return "collections/entities"; }
		}

		public override string URL
		{
			get { return "collections/entities.json"; }
		}

		public override string ResourceType
		{
			get { return "collections"; }
		}

		public override string ObjectType
		{
			get { return "entities"; }
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
				Parameter count = Parameters.FirstOrDefault(x => x.Key == "count");

				if (id == null || string.IsNullOrEmpty(id.Value))
				{
					return false;
				}

				if (count != null)
				{
					int value;
					if (int.TryParse(count.Value, out value))
					{
						if (value > 200)
							return false;
					}
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
			Parameters.Add(new Parameter("count", ParamStatus.Optional, ParamType.number, 200));
			Parameters.Add(new Parameter("max_position", ParamStatus.Optional, ParamType.number));
			Parameters.Add(new Parameter("min_position", ParamStatus.Optional, ParamType.number));
		}

		#endregion
	}
}
