using LoggerManager;
using SharePointBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointScannerCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Load();
            Console.ReadLine();
        }

        private static void Load()
        {
            string userName = string.Empty, password = string.Empty;
            string siteUrl = string.Empty;
            //SPBrowser browser = new SPBrowser(userName, password);
            Logger log = LoggerFactory.GetInstance(@"D:\test.log",LogLevel.DEBUG);
            log.Info("Start");
            log.Debug("Test");
        }
    }
}
