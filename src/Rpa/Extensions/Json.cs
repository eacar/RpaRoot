using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Rpa.Extensions
{
    public static class JsonExtensions
    {
        public static T FromJson<T>(this string jsonString)
        {
            try
            {
                return (T)JsonConvert.DeserializeObject(jsonString, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception($"JsonString: {jsonString}", ex);
            }
        }
        public static dynamic FromJson(this string jsonString, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(jsonString, type);
            }
            catch (Exception ex)
            {
                throw new Exception($"JsonString: {jsonString}", ex);
            }
        }
        public static string ToJson(this object[] objs)
        {
            if (objs != null && objs.Length > 0)
                return JsonConvert.SerializeObject(objs);

            return string.Empty;
        }
        public static string ToJson(this object obj, JsonSerializerSettings settings = null)
        {
            settings = settings ?? new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Include,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(obj, settings);
        }
        public static DataTable ToDataTable(this string json)
        {
            JArray jsonArray = JArray.Parse(json);

            var trgArray = new JArray();
            foreach (JObject row in jsonArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        public static string ToFixedJson(this string json)
        {
            var regex = new Regex("\"(?<key>.*?)\"\\W?:\\W?\"(?<value>.*?)\"(?=,\".*?\"\\W?:|}$)");
            return regex.Replace(json, new MatchEvaluator(m => {
                var key = m.Groups["key"].Value;
                var val = m.Groups["value"].Value;
                return $"\"{key}\":{JsonConvert.SerializeObject(val)}";
            }));
        }

        public static bool IsJsonValid(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}