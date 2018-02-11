using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class UsersShow : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// users/show
		/// </summary>
		public override string Name
		{
			get { return "users/show"; }
		}

		/// <summary>
		/// users/show.json
		/// </summary>
		public override string URL
		{
			get { return "users/show.json"; }
		}

		/// <summary>
		/// users
		/// </summary>
		public override string ResourceType
		{
			get { return "users"; }
		}

		/// <summary>
		/// show
		/// </summary>
		public override string ObjectType
		{
			get { return "show"; }
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
				Parameter id = Parameters.FirstOrDefault(x => x.Key == "user_id");
				Parameter name = Parameters.FirstOrDefault(x => x.Key == "screen_name");

				if (((id != null) && (!string.IsNullOrEmpty(id.Value))) || ((name != null) && (!string.IsNullOrEmpty(name.Value))))
					return true;
			}

			return false;
		}

		#endregion

		#region Protected Methods

		protected override void GetParameters()
		{
			Parameters = new List<Parameter>();

			Parameters.Add(new Parameter("user_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional));
		}

		#endregion
	}
}
