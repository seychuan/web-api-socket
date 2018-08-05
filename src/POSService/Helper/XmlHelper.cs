using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace POSService.Helper
{
    public static class XmlHelper
    {
        public static string ToXmlString<T>(T obj)
        {
            var xs = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8
            };
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var xml = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    xs.Serialize(xmlWriter, obj, ns);
                    memoryStream.Position = 0;
                }
                using (StreamReader sr = new StreamReader(memoryStream))
                {
                    xml = sr.ReadToEnd();
                }
            }
            return xml;
        }

        public static T ToObject<T>(string xml)
        {
            // TODO: Implement
            return default(T);
        }
    }
}
