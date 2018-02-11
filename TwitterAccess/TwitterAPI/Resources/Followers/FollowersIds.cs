﻿using System.Collections.Generic;
using System.Linq;

namespace TwitterAccess.TwitterAPI
{
	public class FollowersIds : ApiBase
	{
		#region Public Properties

		/// <summary>
		/// followers/ids
		/// </summary>
		public override string Name
		{
			get { return "followers/ids"; }
		}

		/// <summary>
		/// followers/ids.json
		/// </summary>
		public override string URL
		{
			get { return "followers/ids.json"; }
		}

		/// <summary>
		/// followers
		/// </summary>
		public override string ResourceType
		{
			get { return "followers"; }
		}

		/// <summary>
		/// ids
		/// </summary>
		public override string ObjectType
		{
			get { return "ids"; }
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
				Parameter par = Parameters.First(x => x.Key == "count");
				if (!string.IsNullOrEmpty(par.Value))
				{
					if (int.Parse(par.Value) > 5000)
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

			Parameters.Add(new Parameter("user_id", ParamStatus.Optional));
			Parameters.Add(new Parameter("screen_name", ParamStatus.Optional));
			Parameters.Add(new Parameter("cursor", ParamStatus.Optional));
			Parameters.Add(new Parameter("stringify_ids", ParamStatus.Optional));
			Parameters.Add(new Parameter("count", ParamStatus.Optional));
		}

		#endregion
	}
}
