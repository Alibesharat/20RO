using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaCoreLogger
{
   public class CoreLog
    {
        public DateTime DateTime { get; set; }
        public string RequsetPath { get; set; }

        public string ErrorMessage { get; set; }

        public Exception Exception { get; set; }

        public string UserIp { get; set; }

        public string UserAgent { get; set; }
    }
}
