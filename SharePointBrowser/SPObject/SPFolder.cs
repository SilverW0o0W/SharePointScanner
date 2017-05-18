using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SharePointBrowser.SPObject
{
    class SPFolder : SPObject
    {
        private Folder msFolder;

        public SPFolder(ClientContext context, Folder msFolder, string parentUrl) : base(context, parentUrl)
        {
            this.context = context;
            this.msObject = msFolder;
            this.Id = new Guid();
            this.DisplayName = msFolder.Name;
            this.Url = string.Format("{0}/{1}", parentUrl, this.DisplayName);
        }
    }
}
