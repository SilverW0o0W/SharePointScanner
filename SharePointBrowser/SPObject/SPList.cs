using System;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace SharePointBrowser.SharePointObject
{
    public class SPList : SPObject
    {
        private List<SPFolder> folders;
        public List<SPFolder> Folders { get { return GetFolders(); } }
        private SPFolder rootFolder;
        public SPFolder RootFolder { get { return GetRootFolder(); } }

        public SPList(ClientContext context, List msList, string parentUrl) : base(context, ObjectLevel.Library, msList, parentUrl)
        {
            this.Id = msList.Id;
            this.DisplayName = msList.Title;
            this.Url = string.Format("{0}/{1}", parentUrl, this.DisplayName);
        }

        private List<SPFolder> GetFolders(bool reload = true)
        {
            if (!reload)
            {
                reload = folders == null;
            }
            if (!reload)
            {
                return folders;
            }
            folders = new List<SPFolder>();
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

        private ListItemCollection GetListItemCollection(string queryXml)
        {
            List msList = this.msObject as List;
            ListItemCollection listItemCollection = null;
            CamlQuery query = new CamlQuery();
            try
            {
                query.ViewXml = queryXml;
                listItemCollection = msList.GetItems(query);
            }
            catch (Exception)
            {
                listItemCollection = null;
            }
            return listItemCollection;
        }

        public SPFolder GetFolderByName(string name)
        {
            ListItemCollection listItemCollection = null;
            SPFolder spFolder = null;
            string queryXml;
            queryXml = string.Format("{0}{1}{2}", "<View><Query><Where><Contains><FieldRef Name='Title'/><Value Type='Text'>", name, "</Value></Contains></Where></Query></View>");
            listItemCollection = GetListItemCollection(queryXml);
            if (listItemCollection == null || listItemCollection.Count < 1)
            {
                return null;
            }
            else
            {
                try
                {
                    Folder msFolder = listItemCollection[0].Folder;
                    spFolder = new SPFolder(this.context, msFolder, this.Url);
                }
                catch (Exception)
                {
                    spFolder = null;
                }
            }
            return spFolder;
        }

        private SPFolder GetRootFolder(bool reload = true)
        {
            if (!reload)
            {
                reload = rootFolder == null;
            }
            if (!reload)
            {
                return rootFolder;
            }
            try
            {
                List msList = this.msObject as List;
                Folder msFolder = msList.RootFolder;
                this.Load(msFolder);
                rootFolder = new SPFolder(this.context, msFolder, this.Url);
            }
            catch (Exception)
            {
                rootFolder = null;
            }
            return rootFolder;
        }

        public override void ReloadChild()
        {
            rootFolder = null;
            folders = null;
        }
    }
}