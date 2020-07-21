using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Orders.Services
{
    public static class ExtendedXMLFunctionality
    {
        public static T DeserializeToObject<T>(this string xml) where T : class
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            byte[] xmlToBytes = Encoding.UTF8.GetBytes(xml);

            using (MemoryStream ms = new MemoryStream(xmlToBytes))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    return (T)serializer.Deserialize(sr);
                }
            }
        }
        public static string SerializeToXml<T>(this T model) where T : class
        {
            XmlSerializer serToXml = new XmlSerializer(model.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                serToXml.Serialize(textWriter, model);
                return textWriter.ToString();
            }
        }

    }
}

