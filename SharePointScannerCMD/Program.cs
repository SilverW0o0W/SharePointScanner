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
            SPBrowser browser = new SPBrowser(userName, password, siteUrl);
            browser.Load();
            List<string> items = browser.Load();
            using (TextWriter writer = new StreamWriter(@"D:\data.txt"))
            {
                foreach (string item in items)
                {
                    writer.WriteLine(item);
                }
            }
        }
    }
}
