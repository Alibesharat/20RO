using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WepApplication.Controllers
{
    public class EmailController : Controller
    {
        private SmtpClient smtpClient;
        public EmailController(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public async Task<IActionResult> Index()
        {
            await this.smtpClient.SendMailAsync(new MailMessage(
                from: "1368.ali.b@gmail.com",
          to: "sample1.app@noname.test",
          subject: "Test message subject",
          body: "Test message body"
          ));

            return Json("Email Was snd");
        }
    }
}