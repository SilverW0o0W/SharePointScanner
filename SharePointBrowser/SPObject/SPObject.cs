using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharePointBrowser.SharePointObject
{
    public enum ObjectLevel
    {
        Site,
        Web,
        Library,
        Folder,
        File
    }
    public abstract class SPObject : ISPObject
    {
        protected ClientContext context;
        //protected List<SPObject> children;
        protected ClientObject msObject;

        public string DisplayName { get; protected set; }
        public Guid Id { get; protected set; }
        public string Url { get; protected set; }
        public string ParentUrl { get; private set; }
        public ObjectLevel Level { get; private set; }

        public SPObject(ClientContext context, ObjectLevel level, string parentUrl)
        {
            this.context = context;
            this.Level = level;
            this.ParentUrl = parentUrl;
        }

        public SPObject(ClientContext context, ObjectLevel level, ClientObject msObject, string parentUrl)
        {
            this.context = context;
            this.Level = level;
            this.msObject = msObject;
            this.ParentUrl = parentUrl;
        }

        #region interface ISPObject

        //public ISPObject Parent { get { return GetParent(); } }

        //public abstract void Load();

        //protected abstract ISPObject GetParent();
        #endregion

        public void Load<T>(T clientObject, params Expression<Func<T, object>>[] retrievals) where T : ClientObject
        {
            context.Load(clientObject, retrievals);
            context.ExecuteQuery();
        }

        public void Reload()
        {
            this.Load(this.msObject);
            ReloadChild();
        }

        protected abstract void ReloadName();

        public abstract void ReloadChild();
    }

    internal class SPObjectInformation
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public SPObjectInformation()
        {

        }

        public SPObjectInformation(Guid id, string displayName, string url)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.Url = url;
        }
    }
}
