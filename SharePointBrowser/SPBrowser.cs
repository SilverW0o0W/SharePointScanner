using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using SharePointBrowser.SharePointObject;
using System.Linq.Expressions;
using LoggerManager;
using static SharePointBrowser.SPItemExporter;

namespace SharePointBrowser
{
    public class SPBrowser
    {
        private Logger log = LoggerFactory.GetInstance(LogLevel.DEBUG);
        private ClientContext context;
        ExceptionHandlingScope scope;
        private SPItemExporter exporter;
        private string originUrl;
        private string userName;
        private SecureString password;

        public string ExportFilePath { get; set; }
        public FileType ExportFileType { get; set; }

        public SPBrowser(string userName, string password)
        {
            this.userName = userName;
            this.password = GetPassword(password);
            this.exporter = new SPItemExporter(log);
        }

        public SPBrowser(string userName, string password, string exportFilePath, FileType type)
        {
            this.userName = userName;
            this.password = GetPassword(password);
            this.ExportFilePath = exportFilePath;
            this.ExportFileType = type;
            this.exporter = new SPItemExporter(log);
        }

        public bool Initialize(string url)
        {
            bool isSuccess = false;
            try
            {
                originUrl = url;
                context = new ClientContext(url);
                scope = new ExceptionHandlingScope(context);
                context.Credentials = new SharePointOnlineCredentials(userName, password);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("Initialize browser failed. Reason: {0}.", ex.ToString());
                isSuccess = false;
            }
            return isSuccess;
        }
        public void Export()
        {
            SPSite spSite = new SPSite(context);
            List<SPObject> objects = spSite.RootWeb.Lists.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
            exporter.Export(objects, this.ExportFilePath, this.ExportFileType);
        }

        public List<SPObject> Load(string url, NodeLevel level)
        {
            throw new NotImplementedException();
        }

        public List<SPObject> Load(SPObject spObject, NodeLevel level)
        {
            List<SPObject> childObjects = null;
            switch (level)
            {
                case NodeLevel.Site:
                    SPSite spSite = spObject as SPSite;
                    childObjects = spSite.Webs.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
                    break;
                case NodeLevel.Web:
                    SPWeb spWeb = spObject as SPWeb;
                    childObjects = spWeb.Lists.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
                    break;
                case NodeLevel.Library:
                    SPList spList = spObject as SPList;
                    childObjects = spList.Folders.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
                    break;
                case NodeLevel.Folder:
                    SPFolder spFolder = spObject as SPFolder;
                    childObjects = spFolder.Folders.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
                    break;
                case NodeLevel.FolderContent:
                    spFolder = spObject as SPFolder;
                    childObjects = spFolder.Files.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
                    break;
                case NodeLevel.File:
                    break;
                default:
                    break;
            }
            return childObjects;
        }

        private SecureString GetPassword(string password)
        {
            SecureString ss = new SecureString();
            foreach (char item in password)
            {
                ss.AppendChar(item);
            }
            return ss;
        }

        public static SPObject ConvertToInfo(SPObject spObject)
        {
            return spObject;
        }
    }
}
