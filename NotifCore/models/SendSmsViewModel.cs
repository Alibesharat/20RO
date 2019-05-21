using System;
using System.Collections.Generic;
using System.Text;

namespace NotifCore.models
{
    public class SendSmsViewModel
    {
        public string receptor { get; set; }
        public string token { get; set; }
        public string token2 { get; set; }
        public string token3 { get; set; }
        public string template { get; set; }
        public string type { get; set; }
    }
}
