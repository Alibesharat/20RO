using Kavenegar;
using Kavenegar.Core.Models;
using NotifCore.models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotifCore
{

    public class kaveSMS : ISMS<List<SendResult>>
    {
        public List<string> phoneNumbers { get; set; }
        public string message { get; set; }
        public string template { get; set; }

        static HttpClient client = new HttpClient();

        public async Task<(bool statuse, string ErrorMessage, List<SendResult> result)> SendNotifyAsync()
        {
            try
            {

                var receptors = phoneNumbers;
                var api = new KavenegarApi("6E657A6A752B7A59734773667A7236427168645A48513D3D");
                var result = await api.Send("", receptors, message);
                return (true, "", result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Source == "Kavenegar.Core")
                {
                    //ApiException
                    return (false, ex.InnerException.Message, null);
                }
                else
                {
                    //HttpExeption 
                    return (false, "ارتباط  با سرور برقرار نشد", null);
                }
            }
        }


        public async Task<Exception> SendNotifyWithTemplateAsync(string phonenumber, string link, MessageTemplate template)
        {

            try
            {
                string Key = "6E657A6A752B7A59734773667A7236427168645A48513D3D";
                SendSmsViewModel vm = new SendSmsViewModel
                {
                    receptor = phonenumber,
                    template = template.ToString(),
                    token = link,
                    token2 = "",
                    token3 = "",
                    type = "sms"
                };
                var result = await client.GetAsync($"https://api.kavenegar.com/v1/{Key}/verify/lookup.json?receptor={vm.receptor}&template={vm.template}&token={vm.token}");
                result.EnsureSuccessStatusCode();
                var data = await result.Content.ReadAsAsync<ResultSmsViewModel>();
                return new Exception("yes");
            }
            catch (Exception ex)
            {

                return ex;
            }


        }


    }

    public enum MessageTemplate
    {
      
        Bisroverify,
        Bistogeton,
        Bistrogetoff,
        Bisrobreif


    }
}
