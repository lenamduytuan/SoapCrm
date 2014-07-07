using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ModernSoapApp.Helper.Entities
{
    public static class SerializationExtensions
    {
        public static string Serialize(this object value)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(memoryStream, value);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memoryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static T Deserialize<T>(this string value)
        {
            using (var stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var dataContractSerializer = new DataContractJsonSerializer(typeof(T));
                return (T)dataContractSerializer.ReadObject(stream);
            }
        }

        public static string ToJson(this object value)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            return json;
        }

        public static T JsonToObject<T>(this string value)
        {
            if (value == null)
                return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }

        public static string Format(this string value, params object[] parameters)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (parameters == null)
                return value;

            return string.Format(value, parameters);
        }
    }
}
