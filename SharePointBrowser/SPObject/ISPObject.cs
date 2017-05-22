using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointBrowser.SharePointObject
{
    public interface ISPObject
    {
        void Reload();
        void ReloadChild();
        //ISPObject Parent { get; }
    }
}
