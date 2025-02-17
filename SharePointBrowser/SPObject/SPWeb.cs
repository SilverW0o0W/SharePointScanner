﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SharePointBrowser.SharePointObject
{
    public class SPWeb : SPObject
    {
        private List<SPList> lists;
        public List<SPList> Lists { get { return GetLists(); } }

        public SPWeb(ClientContext context, Web msWeb, string parentUrl) : base(context, ObjectLevel.Web, msWeb, parentUrl)
        {
            this.DisplayName = msWeb.Title;
            this.Url = msWeb.ServerRelativeUrl;
            this.Id = msWeb.Id;
        }

        //public override void Load()
        //{

        //}

        private List<SPList> GetLists(bool reload = true)
        {
            if (!reload)
            {
                reload = lists == null;
            }
            if (!reload)
            {
                return lists;
            }
            lists = new List<SPList>();
            Web msWeb = this.msObject as Web;
            ListCollection listCollection = msWeb.Lists;
            this.Load(listCollection);
            foreach (List msList in listCollection)
            {
                SPList tempList = new SPList(this.context, msList, msWeb.Url);
                lists.Add(tempList);
            }
            return lists;
        }

        public SPList GetListByName(string name)
        {
            SPList spList = null;
            Web msWeb = this.msObject as Web;
            string url = string.Format("{0}/{1}", this.Url, name);
            List msList = msWeb.GetList(url);
            this.Load(msList);
            spList = new SPList(context, msList, this.Url);
            return spList;
        }

        public override void ReloadChild()
        {
            lists = null;
        }

        protected override void ReloadName()
        {
            Web web = this.msObject as Web;
            this.DisplayName = web.Title;
        }
    }
}
