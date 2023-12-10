using System.Net.Mail;
using System.Net;
using Vezeeta.API.Email_Service;

namespace Vezeeta.API.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("ebtesammahmoud200@gmail.com", "tnmpcerxrevpehxr");
            Client.Send("ebtesammahmoud200@gmail.com", email.To, email.Subject, email.Body);


        }
    }
}
