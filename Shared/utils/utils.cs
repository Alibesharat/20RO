using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class utils
    {

        /// <summary>
        /// الگوی هش کردن رشته
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string hashedString(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var res = "";
            foreach (char item in text)
            {
                res += "g$#5!A";
                res += item;
            }
            return res;

        }


        /// <summary>
        /// الگوی دی هش کردن رشته
        /// </summary>
        /// <param name="Hashed"></param>
        /// <returns></returns>
        public static object DehashedString(this string Hashed)
        {
            if (string.IsNullOrEmpty(Hashed))
                return string.Empty;
            var res = Hashed.Base64Decode();
            res = res.Replace("g$#5!A", String.Empty);
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in res.Split('_'))
            {

                var name = item.Split(':')[0];
                string value = "";
                if (item.Contains(".jpg") && (!item.Contains("default.jpg")))
                {
                    var picpath = $"{data.Last().Value}_{item}";
                    var key = data.Last().Key;
                    data.Remove(key);
                    data.Add(key, picpath);
                }
                else
                {
                    if (item.Split(':')[1] == "http")
                    {
                        value = $"{ item.Split(':')[1]}:{item.Split(':')[2]}";
                    }
                    else
                    {
                        value = item.Split(':')[1];
                    }
                    data.Add(name, value);
                }
            }
            return data;


        }




        /// <summary>
        /// HashedObject
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="props">GetType().GetProperties()</param>
        /// <returns></returns>
        public static string HashedObject(this object obj, PropertyInfo[] props)
        {
            string result = "";
            try
            {
                JObject json = JObject.FromObject(obj);

                foreach (var prop in json.Properties())
                {
                    string value = prop.Value.ToString().hashedString();
                    string name = prop.Name.ToString().hashedString();
                    result += $"{ name}:{value}_";
                }
                result = result.TrimEnd('_');
                result = result.Base64Encode();

            }
            catch
            {
                result = "-1";

            }
            return result;
        }


        /// <summary>
        /// دیکود
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Base64Decode(this string s)
        {
            if (s.IsBase64())
            {
                byte[] data = Convert.FromBase64String(s);
                string decodedString = Encoding.UTF8.GetString(data);
                return decodedString;
            }
            return s;
        }

        /// <summary>
        /// انکود
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static bool IsBase64(this string base64String)
        {


            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            return true;



        }

        /// <summary>
        /// تبدیل فایل به بایت
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ToByte(this IFormFile file)
        {

            byte[] filebyte = null;
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    filebyte = ms.ToArray();
                    //  string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data

                }
            }
            return filebyte;


        }

        /// <summary>
        /// ایجاد رشته یکتا
        /// </summary>
        /// <returns></returns>
        public static string UniqGenerate()
        {
            return DateTime.Now.ToString("ssmmhhddMMyy");
        }

        /// <summary>
        /// دریافت نویگیش
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static async Task<Navigation> GetNavigation(string origin, string distance)
        {
            string access_token = "pk.eyJ1IjoiYm5hbGluaWEiLCJhIjoiY2pzbGhlaW1iMnl6czN5bzY4aGtlNGRjMyJ9.w5OCvI1DM3vHw4iPcDsSPw";
            string geometries = "geojson";
            string profile = "driving";
            string Route = "https://api.mapbox.com/directions/v5/mapbox";

            var api = $"{Route}/{profile}/{origin};{distance}?geometries={geometries}&access_token={access_token}";
            var data = await ConnectApi.GetDataFromHttpClientAsync<Navigation>(null, api, ApiMethode.Get);
            return data;

          
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        
    }
}
