using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SharePointBrowser.SPObject
{
    internal class SPWeb : SPObject
    {
        public List<SPList> Lists { get { return GetLists(); } }

        public SPWeb(ClientContext context, Web msWeb, string parentUrl) : base(context, parentUrl)
        {
            this.msObject = msWeb;
            this.DisplayName = msWeb.Title;
            this.Url = msWeb.Url;
            this.Id = msWeb.Id;
        }

        //public override void Load()
        //{

        //}

        private List<SPList> GetLists()
        {
            List<SPList> lists = new List<SPList>();
            Web msWeb = this.msObject as Web;
            ListCollection listCollection = msWeb.Lists;
            this.Load(listCollection);
            foreach (List list in listCollection)
            {
                SPList tempList = new SPList(this.context, list, msWeb.Url);
                lists.Add(tempList);
            }
            return lists;
        }

        public SPList GetListByUrl(string url)
        {
            SPList spList = null;
            Web msWeb = this.msObject as Web;
            List msList = msWeb.GetList(url);
            this.Load(msList);
            spList = new SPList(context, msList, this.Url);
            return spList;
        }
    }
}
