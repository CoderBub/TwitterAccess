using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
    public class AccountSettings : ApiBase
    {
        #region Public Properties

        public override string Name
        {
            get { return "account/settings"; }
        }

        public override string URL
        {
            get { return "account/settings.json"; }
        }

        public override string ResourceType
        {
            get { return "account"; }
        }

        public override string ObjectType
        {
            get { return "settings"; }
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

		#region Protected Methods

		protected override void GetParameters()
        {
            Parameters = new List<Parameter>();
        }

		#endregion
	}
}
