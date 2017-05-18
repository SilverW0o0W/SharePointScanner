using System;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace SharePointBrowser.SPObject
{
    internal class SPList : SPObject
    {
        public List<SPFolder> Folders { get { return GetFolders(); } }

        public SPList(ClientContext context, List msList, string parentUrl) : base(context, parentUrl)
        {
            this.context = context;
            this.msObject = msList;
            this.Id = msList.Id;
            this.DisplayName = msList.Title;
            this.Url = string.Format("{0}/{1}", parentUrl, this.DisplayName);
        }

        private List<SPFolder> GetFolders()
        {
            List<SPFolder> folders = new List<SPFolder>();
            List msList = this.msObject as List;
            FolderCollection folderCollection = msList.RootFolder.Folders;
            this.Load(folderCollection);
            foreach (Folder msFolder in folderCollection)
            {
                SPFolder tempFolder = new SPFolder(this.context, msFolder, this.Url);
                folders.Add(tempFolder);
            }
            return folders;
        }
    }
}