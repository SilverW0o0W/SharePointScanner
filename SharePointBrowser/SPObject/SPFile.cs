using Microsoft.SharePoint.Client;
using System;

namespace SharePointBrowser.SharePointObject
{
    public class SPFile : SPObject
    {
        public SPFile(ClientContext context, File msFile, string parentUrl) : base(context, ObjectLevel.File, msFile, parentUrl)
        {
            this.Id = new Guid();
            this.DisplayName = msFile.Title;
            this.Url = msFile.ServerRelativeUrl;
        }
    }
}