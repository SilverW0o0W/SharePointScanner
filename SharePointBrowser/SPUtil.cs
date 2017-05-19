using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharePointBrowser.SharePointObject;
using System.IO;
using System.Xml;

namespace SharePointBrowser
{
    public class SPUtil
    {
        public enum FileType
        {
            TXT,
            XML
        }

        private static TextWriter writer;
        public static bool Import(List<SPObject> spObjects, string filePath, FileType type)
        {
            bool result = false;
            using (writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                switch (type)
                {
                    case FileType.TXT:
                        result = ImportTxt(spObjects);
                        break;
                    case FileType.XML:
                        result = ImportXml(spObjects);
                        break;
                    default:
                        result = false;
                        break;
                }
            }
            return result;
        }

        private static bool ImportTxt(List<SPObject> spObjects)
        {
            bool result = false;
            try
            {
                TextWriter textWriter = writer;
                textWriter.WriteLine("Start writing.Time: {0}", DateTime.Now.ToString());
                foreach (SPObject item in spObjects)
                {
                    string message = string.Format("Name: {0}. ID: {1}. URL: {2}.", item.DisplayName, item.Id, item.Url);
                    textWriter.WriteLine(message);
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private static bool ImportXml(List<SPObject> spObjects)
        {
            bool result = false;
            try
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Items");
                    foreach (SPObject item in spObjects)
                    {
                        xmlWriter.WriteStartElement("Item");
                        xmlWriter.WriteAttributeString("Name", item.DisplayName);
                        xmlWriter.WriteAttributeString("ID", item.Id.ToString());
                        xmlWriter.WriteAttributeString("URL", item.Url);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    //xmlWriter.Flush();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

    }
}
