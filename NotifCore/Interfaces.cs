using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotifCore
{

    public interface INotify<T>
    {
        string message { get; set; }

        Task<(bool statuse, string ErrorMessage, T result)> SendNotifyAsync();
        Task<Exception> SendNotifyWithTemplateAsync(string phonenumber, string link, MessageTemplate template);
        
    }


    public interface ISMS<T> : INotify<T>
    {

        List<string> phoneNumbers { get; set; }

          

    }

    public interface IEmail<T> : INotify<T>
    {
        List<string> Emails { get; set; }
    }


    public interface ISiganlR<T> : INotify<T>
    {
        List<string> SiganlRIds { get; set; }
    }
}
