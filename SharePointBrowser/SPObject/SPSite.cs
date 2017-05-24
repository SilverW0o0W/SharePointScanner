using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointBrowser.SharePointObject
{
    public class SPSite : SPObject
    {
        private List<SPWeb> webs;
        public List<SPWeb> Webs { get { return GetWebs(); } }
        private SPWeb rootWeb;
        public SPWeb RootWeb { get { return GetRootWeb(); } }

        public SPSite(ClientContext context) : base(context, ObjectLevel.Site, string.Empty)
        {
            this.msObject = context.Site;
            Initialize();
        }

        private void Initialize()
        {
            this.Load(this.msObject);
            Site msSite = this.msObject as Site;
            this.Id = msSite.Id;
            this.Url = msSite.ServerRelativeUrl;
            this.DisplayName = this.Url;
        }

        //public override void Load()
        //{
        //    Initialize();
        //}

        private List<SPWeb> GetWebs(bool reload = true)
        {
            if (!reload)
            {
                reload = webs == null;
            }
            if (!reload)
            {
                return webs;
            }
            webs = new List<SPWeb>();
            Site msSite = this.msObject as Site;
            WebCollection webCollection = msSite.RootWeb.Webs;
            this.Load(webCollection);
            foreach (Web msWeb in webCollection)
            {
                SPWeb tempWeb = new SPWeb(this.context, msWeb, msSite.Url);
                webs.Add(tempWeb);
            }
            return webs;
        }

        private SPWeb GetRootWeb(bool reload = true)
        {
            if (!reload)
            {
                reload = rootWeb == null;
            }
            if (!reload)
            {
                return rootWeb;
            }
            Site msSite = this.msObject as Site;
            Web msWeb = msSite.RootWeb;
            this.Load(msWeb);
            rootWeb = new SPWeb(this.context, msWeb, this.Url);
            return rootWeb;
        }

        public SPWeb GetWebById(Guid id)
        {
            Site msSite = this.msObject as Site;
            Web msWeb = msSite.OpenWebById(id);
            this.Load(msWeb);
            SPWeb spWeb = new SPWeb(context, msWeb, this.Url);
            return spWeb;
        }

        public override void ReloadChild()
        {
            rootWeb = null;
            webs = null;
        }

        protected override void ReloadName()
        {
            Site msSite = this.msObject as Site;
            this.DisplayName = msSite.Url;
        }
    }
}
