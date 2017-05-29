using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharePointBrowser.SharePointObject;
using System.IO;
using System.Xml;
using LoggerManager;

namespace SharePointBrowser
{
    public class SPItemExporter
    {
        public enum FileType
        {
            TXT,
            XML
        }

        private Logger log;
        private TextWriter writer;

        public SPItemExporter(Logger log)
        {
            this.log = log;
        }

        public bool Export(List<SPObject> spObjects, string filePath, FileType type)
        {
            bool result = false;

            log.Info("Start to import ");
            using (writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                switch (type)
                {
                    case FileType.TXT:
                        result = ExportTxt(spObjects);
                        break;
                    case FileType.XML:
                        result = ExportXml(spObjects);
                        break;
                    default:
                        result = false;
                        break;
                }
            }
            return result;
        }

        private bool ExportTxt(List<SPObject> spObjects)
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

        private bool ExportXml(List<SPObject> spObjects)
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
