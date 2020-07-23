using Orders.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Helpers
{
    public static class XmlHelper 
    {
        public static List<T> ReadFromXml<T>(string path) where T : class
        {
            var reader = new StreamReader(path);
            string dataSource = reader.ReadToEnd();
            reader.Close();

            return dataSource.DeserializeToObject<List<T>>();
        }
        public static void WriteToXml<T>(T entity, string path) where T : class
        {
            string xml = entity.SerializeToXml();

            if (File.Exists(path))
            {
                File.WriteAllText(path, xml);
            }

        }
    }
}
