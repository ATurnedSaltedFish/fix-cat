using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace CrawlerNpro.toolkit
{
   public class JsonHelper
    {
        /// <summary>
        /// DeserializeJson
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public JObject JsonDeserialize(string strJson)
        {
            var jobject =(JObject)JsonConvert.DeserializeObject(strJson);
            return jobject;
        }

        /// <summary>
        /// SerializeJson
        /// </summary>
        /// <param name="objJson"></param>
        /// <returns></returns>
        public string JsonSerialize(object objJson)
        {
            var json = JsonConvert.SerializeObject(objJson);
            return json;
        }

        public static JArray DeserializeObjectJArray(string jsonString)
        {
            JArray obj = new JArray(jsonString);
            return obj as JArray;
        }

        /// <summary>
        ///json tostream
        /// </summary>
        /// <returns></returns>
        public  Stream ToStream()
        {
            Stream outStream = new MemoryStream();
            var strJsonText = this.ToString();
            if (string.IsNullOrWhiteSpace(strJsonText))
            {
                throw new Exception("不能转换空的json");
            }
            StreamWriter sw = new StreamWriter(outStream);
             sw.WriteAsync(strJsonText);
             sw.FlushAsync();
            outStream.Position = 0;
            return outStream;
        }

        /// <summary>
        ///Serializer Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public string SerializerJson<T>( List<T> list) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(list.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, list);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }
        /// <summary>
        /// DeserializeJson
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public T DeserializeJsonTo<T>(string strJson) where T:class 
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson));
            T jsonObject = (T)serializer.ReadObject(ms);
            ms.Close();
            return jsonObject;
        }
    }
}
