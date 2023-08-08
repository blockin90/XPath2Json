using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using XPath2Json.Transform;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using XPath2Json.XPath;

namespace XPath2Json
{
    internal class Program
    {
        private static void ApplyJsonXslTransformation(XPathNavigator navigator, XslCompiledTransform xsl)
        {
            var xargs = new XsltArgumentList();
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms)) {
#if DEBUG
                var writer = new XslJsonWriter(sw);
#else
                var writer = new XslJsonWriter(sw);
#endif
                xsl.Transform(navigator, xargs, writer);
                writer.Flush();
#if DEBUG
                ms.Position = 0;
                Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
#endif
            }
        }

        private static void ApplyXmlXslTransformation(XPathNavigator navigator, XslCompiledTransform xsl)
        {
            var xmlDocMemoryStream = new MemoryStream();

            var writer = XmlWriter.Create(xmlDocMemoryStream, new XmlWriterSettings() { Indent = false } );
            var xargs = new XsltArgumentList();
            xsl.Transform(navigator, xargs, writer);
            xmlDocMemoryStream.Position = 0;

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings()
            {
                Converters = new[]
                {
                    new XmlNodeConverter()
                }
            });

            var doc = new XmlDocument();
            doc.Load(xmlDocMemoryStream);
            doc.DocumentElement.RemoveAllAttributes();
            var converter = (XmlNodeConverter)jsonSerializer.Converters[0];
            converter.OmitRootObject = true;

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms, Encoding.UTF8, 1024, true))
            using (var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Newtonsoft.Json.Formatting.None }) {
                jsonSerializer.Serialize(jsonWriter, doc.DocumentElement);
                
                jsonWriter.Flush();
#if DEBUG
                ms.Position = 0;
                Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
#endif
            }

            xmlDocMemoryStream.Dispose();
        }

        static void Main(string[] args)
        {
            int count = 1000000;
            var xml = File.ReadAllText(@"TestData\data.xml");
            
            var xmlXsl = new XslCompiledTransform();
            
            xmlXsl.Load(@"TestData\fromXmlTransform.xslt", XsltSettings.TrustedXslt, new XmlUrlResolver());

            var xmlNoRootXsl = new XslCompiledTransform();
            xmlNoRootXsl.Load(@"TestData\fromXmlTransform_NoRoot.xslt", XsltSettings.TrustedXslt, new XmlUrlResolver());

            var jsonXsl = new XslCompiledTransform();
            jsonXsl.Load(@"TestData\fromJsonTransform.xslt", XsltSettings.TrustedXslt, new XmlUrlResolver());


            var jsonFile = File.ReadAllText(@"TestData\data.json");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < count; i++) {
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                ApplyXmlXslTransformation(doc.CreateNavigator(), xmlXsl);
            }
            sw.Stop();
            Console.WriteLine("xml->xml->json:" + sw.ElapsedMilliseconds / 1000);

            sw.Restart();
            for (int i = 0; i < count; i++) {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                ApplyJsonXslTransformation(doc.CreateNavigator(), xmlNoRootXsl);
            }
            sw.Stop();
            Console.WriteLine("xml->json:" + sw.ElapsedMilliseconds / 1000);

            sw.Restart();

            for (int i = 0; i < count; i++) {

                var obj = JsonConvert.DeserializeObject<JObject>(jsonFile, new JsonSerializerSettings()
                {
                    FloatParseHandling = FloatParseHandling.Decimal
                });
                var nav = new JsonXPathNavigator(obj);
                ApplyJsonXslTransformation(nav, jsonXsl);
            }
            sw.Stop();
            Console.WriteLine("json->json:" + sw.ElapsedMilliseconds / 1000);            
        }
    }
}
