using System.Collections.Generic;
using System.Text;

namespace TwitterAccess.TwitterAPI
{
	public class UsersLookup : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// users/lookup
		/// </summary>
		public override string Name
		{
			get { return "users/lookup"; }
		}

		/// <summary>
		/// users/lookup.json
		/// </summary>
		public override string URL
		{
			get { return "users/lookup.json"; }
		}

		/// <summary>
		/// users
		/// </summary>
		public override string ResourceType
		{
			get { return "users"; }
		}

		/// <summary>
		/// lookup
		/// </summary>
		public override string ObjectType
		{
			get { return "lookup"; }
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
			Parameters.Add(new Parameter("include_entities", ParamStatus.Optional));
		}

		public override string GetQuery()
		{
			StringBuilder query = new StringBuilder();

			if (CheckRequiredParameters())
			{
				foreach (Parameter param in Parameters)
				{
					if (!string.IsNullOrEmpty(param.Value))
					{
						if (query.Length > 0)
							query.Append("&");

						if (param.Key == "user_id" || param.Key == "screen_name")
							param.Value = param.Value.Replace(",","%2C");

						query.Append(param.Key + "=" + param.Value);
					}
				}
			}

			return query.ToString();
		}

		#endregion
	}
}
