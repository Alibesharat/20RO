using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shared
{
    public static class ConnectApi
    {

        static HttpClient client = new HttpClient();



        /// <summary>
        /// ارسال دیتا بصورت  پست به روش سنتی
        /// </summary>
        /// <param name="post_values">داده های که باید ارسال شوند</param>
        /// <param name="Gateway">درگاه ای پی آِی</param>
        /// <returns></returns>
        public static async Task<string> Send_With_Post(Dictionary<string, string> post_values, string Gateway)
        {
            string result = "";
            string post_string = "";
            foreach (KeyValuePair<string, string> post_value in post_values)
            {

                post_string += post_value.Key + "=" + HttpUtility.UrlEncode(post_value.Value) + "&";


            }
            post_string = post_string.TrimEnd('&');
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Gateway);
           


            objRequest.ContentLength = post_string.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            //objRequest.ContentType = "application/json";
            objRequest.MediaType = "application/json";
            objRequest.Accept = "application/json";
            objRequest.Method = "POST";
            //objRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            StreamWriter myWriter = null;
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(post_string);
            myWriter.Close();
            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = await responseStream.ReadToEndAsync();
                    responseStream.Close();
                }

            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = await reader.ReadToEndAsync();
                   

                }

            }




            return result;
        }


        /// <summary>
        /// ارسال دیتا بصورت گت به روش سنتی
        /// </summary>
        /// <param name="post_values">داده های که باید ارسال شوند</param>
        /// <param name="Gateway">درگاه ای پی آِی</param>
        /// <returns></returns>
        public static async Task<string> Send_With_Get(Dictionary<string, string> post_values, string GatewaySend)
        {
            string result = "";
            string post_string = "";

            foreach (var item in post_values)
            {
                post_string += $"{item.Key}={HttpUtility.UrlEncode(item.Value)}&";
            }
            post_string = post_string.TrimEnd('&');
            var data = await new Uri($"{GatewaySend}?{post_string}").GetJsonAsync();
            result = data.ToString();
            return result;
        }



        /// <summary>
        // آپلود فایل
        /// </summary>
        /// <param name="file"></param>
        /// <param name="GatewaySend"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Uplodadfile(IFormFile file, string GatewaySend)
        {
            var fileByte = file.ToByte();
            ByteArrayContent byteContent = new ByteArrayContent(fileByte);
            var client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
            //client.DefaultRequestHeaders.Add("Upload", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            HttpResponseMessage reponse = await client.PostAsync(GatewaySend, byteContent);
            return reponse;
        }




        /// <summary>
        /// دریافت اطلاعات از طریق http 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="Url"></param>
        /// <param name="methode"> def = Post </param>
        /// <returns></returns>
        public static async Task<T> GetDataFromHttpClientAsync<T>(object value, string Url, ApiMethode methode = ApiMethode.Post)
        {
            T Data = default(T);
            try
            {
                ServicePointManager.Expect100Continue = false;
                switch (methode)
                {
                    case ApiMethode.Post:
                      
                        var result = await client.PostAsJsonAsync(Url, value);
                        result.EnsureSuccessStatusCode();
                        Data = await result.Content.ReadAsAsync<T>();
                        break;
                    case ApiMethode.Get:
                        var results = await client.GetAsync(Url);
                        results.EnsureSuccessStatusCode();
                        Data = await results.Content.ReadAsAsync<T>();
                        // Data = JsonConvert.DeserializeObject<T>(results);

                        break;
                }
                return Data;
            }
            catch (System.Exception ex)
            {

                //TODO : Log Ex
                return Data;
            }
        }




    }

    public static class JsonExt
    {
        public static async Task<string> GetJsonAsync(this Uri uri)
        {
            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(uri);
                return jsonString;
            }
        }
    }


    public class ResultObject
    {
        public bool status { get; set; }

        public string message { get; set; }

        public string value { get; set; }

        public string error { get; set; }

        public string PicID { get; set; }

        public string DecodeError { get { return error.Base64Decode(); } }


    }


    /// <summary>
    /// متد ارتباط http
    /// </summary>
    public enum ApiMethode
    {

        Post,
        Get
    }



}
