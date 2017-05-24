using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SharePointBrowser.SharePointObject
{
    public class SPFolder : SPObject
    {
        private List<SPFolder> folders;
        private List<SPFile> files;
        public List<SPFolder> Folders { get { return GetFolders(); } }
        public List<SPFile> Files { get { return GetFiles(); } }

        public SPFolder(ClientContext context, Folder msFolder, string parentUrl) : base(context, ObjectLevel.Folder, msFolder, parentUrl)
        {
            this.Id = new Guid();
            this.DisplayName = msFolder.Name;
            this.Url = msFolder.ServerRelativeUrl;
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
            Folder msFolder = this.msObject as Folder;
            FolderCollection folderCollection = msFolder.Folders;
            this.Load(folderCollection);
            foreach (Folder msChildFolder in folderCollection)
            {
                SPFolder tempFolder = new SPFolder(this.context, msChildFolder, this.Url);
                folders.Add(tempFolder);
            }
            return folders;
        }

        private List<SPFile> GetFiles(bool reload = true)
        {
            if (!reload)
            {
                reload = files == null;
            }
            if (!reload)
            {
                return files;
            }
            files = new List<SPFile>();
            Folder msFolder = this.msObject as Folder;
            FileCollection fileCollection = msFolder.Files;
            this.Load(fileCollection);
            foreach (File msChildFile in fileCollection)
            {
                SPFile tempFile = new SPFile(this.context, msChildFile, this.Url);
                files.Add(tempFile);
            }
            return files;
        }

        public override void ReloadChild()
        {
            files = null;
            folders = null;
        }

        protected override void ReloadName()
        {
            Folder msFolder = this.msObject as Folder;
            this.DisplayName = msFolder.Name;
        }
    }
}
