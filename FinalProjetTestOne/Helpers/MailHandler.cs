using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FinalProjetTestOne.Helpers
{
    public class MailHandler
    {
        public void SendMail(string email, string name, string body)
        {
            try
            {
                var message = new MailMessage();
                message.Body = $"Dear{name},<br/><br/>{ body }";
                message.IsBodyHtml = true;
                message.From = new MailAddress("testfordeveloperlast2020@gmail.com");
                message.To.Add(email);
                var smptClient = new SmtpClient();
                smptClient.Host = "smtp.gmail.com"; 
                smptClient.Port = 587;
                smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smptClient.UseDefaultCredentials = false;
                smptClient.Credentials = new NetworkCredential("testfordeveloperlast2020@gmail.com", "@Admin1234");
                smptClient.EnableSsl = true;
                smptClient.Send(message);
            }
            catch (Exception)
            {
                throw;
            }    
        }
    }
}
