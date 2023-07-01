using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.WebUI.EmailServices
{
    public interface IMailSender
    {
        // smtp => gmail hotmail
        // api => sendgrid,

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}