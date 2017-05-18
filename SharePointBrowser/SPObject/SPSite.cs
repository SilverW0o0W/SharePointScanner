using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointBrowser.SPObject
{
    public class SPSite : SPObject
    {
        public List<SPWeb> Webs { get { return GetWebs(); } }

        public SPSite(ClientContext context) : base(context, context.ApplicationName)
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

        private List<SPWeb> GetWebs()
        {
            List<SPWeb> spWebs = new List<SPWeb>();
            Site msSite = this.msObject as Site;
            WebCollection webCollection = msSite.RootWeb.Webs;
            this.Load(webCollection);
            foreach (Web msWeb in webCollection)
            {
                SPWeb tempWeb = new SPWeb(this.context, msWeb, msSite.Url);
                spWebs.Add(tempWeb);
            }
            return spWebs;
        }

        public SPWeb GetWebById(Guid id)
        {
            Site msSite = this.msObject as Site;
            Web msWeb = msSite.OpenWebById(id);
            this.Load(msWeb);
            SPWeb spWeb = new SPWeb(context, msWeb, this.Url);
            return spWeb;
        }
    }
}
