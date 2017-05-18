using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using SharePointBrowser.SPObject;
using System.Linq.Expressions;

namespace SharePointBrowser
{
    public enum Level
    {
        Site,
        Web,
        Library,
        Folder,
        File
    }
    public class SPBrowser
    {
        private ClientContext context;
        ExceptionHandlingScope scope;
        private string originUrl;
        private string userName;
        private SecureString password;

        public SPBrowser(string userName, string password, string url)
        {
            this.userName = userName;
            this.password = GetPassword(password);
            Initialize(url);
        }

        private bool Initialize(string url)
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
        public List<string> Load()
        {
            SPSite spSite = new SPSite(context);

            //spSite.GetWebById(new Guid());
            //List<SPWeb> spWebs = spSite.Webs;
            //SPList spList = spWebs[0].Lists[0];
            List<SPFolder> folders = spSite.Webs[0].Lists[0].Folders;
            List<string> itemNames = new List<string>();
            Web web = context.Web;

            List list;
            ListCollection listCollection = web.Lists;
            context.Load(web, spWeb => spWeb.Lists);
            context.ExecuteQuery();

            list = listCollection.GetByTitle("folder26");
            context.Load(list);
            context.ExecuteQuery();
            Console.WriteLine(list.ItemCount);
            CamlQuery query = new CamlQuery();
            query.ViewXml = "<View><Query></Query></View>";
            //query.ViewXml = "<View><Query><Where><Contains><FieldRef Name='Title'/><Value Type='Text'>announce</Value></Contains></Where></Query></View>";
            ListItemCollection collListItem = list.GetItems(query);

            context.Load(collListItem);
            context.ExecuteQuery();
            int i = 0;
            foreach (ListItem item in collListItem)
            {
                context.Load(item, spItem => spItem.DisplayName);
                context.ExecuteQuery();
                itemNames.Add(item.DisplayName);
                Console.WriteLine(i);
                i++;
            }
            //foreach (User user in userCollection)
            //{
            //    Console.WriteLine(user.Email);
            //}
            return itemNames;
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
    }
}
