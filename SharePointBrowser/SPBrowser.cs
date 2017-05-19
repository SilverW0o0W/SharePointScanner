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
using static SharePointBrowser.SPUtil;

namespace SharePointBrowser
{
    public class SPBrowser
    {
        private static Logger log = LoggerFactory.GetInstance();
        private ClientContext context;
        ExceptionHandlingScope scope;
        private string originUrl;
        private string userName;
        private SecureString password;

        public string ImportFilePath { get; set; }
        public FileType ImportFileType { get; set; }

        public SPBrowser(string userName, string password)
        {
            this.userName = userName;
            this.password = GetPassword(password);
        }

        public SPBrowser(string userName, string password, string importFilePath, FileType type)
        {
            this.userName = userName;
            this.password = GetPassword(password);
            this.ImportFilePath = importFilePath;
            this.ImportFileType = type;
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
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        public void Import()
        {
            SPSite spSite = new SPSite(context);
            List<SPObject> objects = spSite.RootWeb.Lists.ConvertAll(new Converter<SPObject, SPObject>(ConvertToInfo));
            SPUtil.Import(objects, this.ImportFilePath, this.ImportFileType);
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
