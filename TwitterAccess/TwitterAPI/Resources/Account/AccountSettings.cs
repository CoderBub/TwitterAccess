using System.Collections.Generic;

namespace TwitterAccess.TwitterAPI
{
    public class AccountSettings : ApiBase
    {
        #region Public Properties

        /// <summary>
        /// account/settings
        /// </summary>
        public override string Name
        {
            get { return "account/settings"; }
        }

        /// <summary>
        /// account/settings.json
        /// </summary>
        public override string URL
        {
            get { return "account/settings.json"; }
        }

        /// <summary>
        /// account
        /// </summary>
        public override string ResourceType
        {
            get { return "account"; }
        }

        /// <summary>
        /// rate_limit_status
        /// </summary>
        public override string ObjectType
        {
            get { return "settings"; }
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

        public string Results { get; set; }

        #endregion

        protected override void GetParameters()
        {
            Parameters = new List<Parameter>();
        }
    }
}
