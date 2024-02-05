using System.Formats.Asn1;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using static System.Net.WebRequestMethods;

namespace converting_xml_to_xml.html_
{
    internal class Program
    {
        private static readonly Dictionary<string, byte[]> _xslDataCache = new Dictionary<string, byte[]>();
        static void Main(string[] args)
        {

            using (MemoryStream xmlMemory = new MemoryStream())
            using (StreamReader xmlMemoryReader = new StreamReader(xmlMemory, Encoding.UTF8))
            {

                string[] DirFiles = Directory.GetFiles("XML\\");
                List<string> files = (from a in Directory.GetFiles("XML\\") select Path.GetFileName(a)).ToList();

                XmlDocument documents = new XmlDocument();
                for (int i = 0; i < files.Count; i++)
                {
                    var xml = XDocument.Load(DirFiles[i]);
                    documents.LoadXml(xml.ToString());
                    
                    XmlProcessingInstruction? instruction = documents.SelectSingleNode("//processing-instruction(\"xml-stylesheet\")") as XmlProcessingInstruction;

                    int hrefIndex = 0;
                    if (instruction == null ||
                        string.IsNullOrEmpty(instruction.Value) ||
                        (hrefIndex = instruction.Value.IndexOf("href=\"", StringComparison.InvariantCultureIgnoreCase)) < 0) ;

                    hrefIndex += 6;
                    string href = instruction.Value.Substring(hrefIndex, instruction.Value.Length - hrefIndex);
                    href = href.Trim('\"', ' ');

                    if ( href == "https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_Big/ZU/07/Common.xsl")
                    {
                        string[] seconds = Directory.GetFiles("XSL\\ReestrExtractBigZu\\");

                        XPathDocument myXPathDoc = new XPathDocument(DirFiles[i]);
                        XslTransform myXslTrans = new XslTransform();

                        myXslTrans.Load(seconds[0]);
                        XmlTextWriter myWriter = new XmlTextWriter("result\\" + files[i] + ".html", null);
                        myXslTrans.Transform(myXPathDoc, null, myWriter);
                        if (System.IO.File.Exists("XML\\" + files[i]))
                        {
                            System.IO.File.Delete("XML\\" + files[i]);
                        }
                    } 
                    else if ( href == "https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_Big/OKS/07/Common.xsl")
                    {
                        string[] seconds = Directory.GetFiles("XSL\\ReestrExtractBigOks\\");

                        XPathDocument myXPathDoc = new XPathDocument(DirFiles[i]);
                        XslTransform myXslTrans = new XslTransform();

                        myXslTrans.Load(seconds[0]);
                        XmlTextWriter myWriter = new XmlTextWriter("result\\" + files[i] + ".html", null);
                        myXslTrans.Transform(myXPathDoc, null, myWriter);
                        if (System.IO.File.Exists("XML\\" + files[i]))
                        {
                            System.IO.File.Delete("XML\\" + files[i]);
                        }
                    }
                    else if (href == "https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_Big/ROOM/07/Common.xsl")
                    {
                        string[] seconds = Directory.GetFiles("XSL\\ReestrExtractBigRoom\\");

                        XPathDocument myXPathDoc = new XPathDocument(DirFiles[i]);
                        XslTransform myXslTrans = new XslTransform();

                        myXslTrans.Load(seconds[0]);
                        XmlTextWriter myWriter = new XmlTextWriter("result\\" + files[i] + ".html", null);
                        myXslTrans.Transform(myXPathDoc, null, myWriter);
                        if (System.IO.File.Exists("XML\\" + files[i]))
                        {
                            System.IO.File.Delete("XML\\" + files[i]);
                        }
                    }
                    else if (href == "https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_List/07/Common.xsl")
                    {
                        string _localHref = null;
                        byte[] xslData = null;
                        MemoryStream outputStream = new MemoryStream();
                        MemoryStream xmlStream1 = new MemoryStream();
                        using (WebClient client = new WebClient())
                        {

                            xml.Save(xmlStream1);
                            try
                            {
                                xslData = client.DownloadData(href);                                
                            }
                            catch
                            {
                                _localHref = href.Replace("https://portal.rosreestr.ru/", Directory.GetCurrentDirectory().Replace("\\", "/") + "/LocalXsl/");
                                xslData = client.DownloadData(_localHref);
                            }
                            _xslDataCache[href] = xslData;
                        }

                        XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
                        {
                            ConformanceLevel = ConformanceLevel.Document,
                            DtdProcessing = DtdProcessing.Parse,
                            CloseInput = true,
                            IgnoreWhitespace = true,
                            IgnoreComments = true,
                        };
                        XmlTextWriter myWriter = new XmlTextWriter("result\\" + files[i] + ".html", null);
                        xmlStream1.Seek(0, SeekOrigin.Begin);
                        using (MemoryStream xslStream = new MemoryStream(xslData))
                        using (XmlReader xslReader = XmlReader.Create(xslStream, xmlReaderSettings, _localHref ?? href))
                        using (XmlReader xmlReader = XmlReader.Create(xmlStream1, xmlReaderSettings)) 
                        {
                            XslCompiledTransform xslt = new XslCompiledTransform();
                            xslt.Load(xslReader, new XsltSettings(true, true), new XmlUrlResolver());

                            xslt.Transform(xmlReader, null, myWriter);
                        }
                        outputStream.Seek(0, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(outputStream))
                        {
                             reader.ReadToEnd();
                        }
                        if (System.IO.File.Exists("XML\\" + files[i]))
                        {
                            System.IO.File.Delete("XML\\" + files[i]);
                        }
                    }
                }
                Console.WriteLine("Конвертация завершена!");
            }
        }
    }
}