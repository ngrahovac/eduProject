using System.Net.Mail;

namespace eduProjectWebAPI.Services
{
    public static class MailSender
    {
        public static void SendActivationEmail(string destinationAddress, string confirmationLink)
        {
            string sourceAddress = "eduprojectnet@gmail.com";
            string sourceAddressPassword = "arkanovi4"; //Must hide this!

            MailMessage message = new MailMessage();
            MailAddress sourceAddressMail = new MailAddress(sourceAddress);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                
            message.From = sourceAddressMail;
            message.To.Add(destinationAddress);
            message.Subject = "eduProject aktivacija naloga";
            message.Body = "Poštovani, da biste aktivirali Vaš nalog, potrebno je da klikete na sljedeći URL: " + confirmationLink;
            message.IsBodyHtml = true;

            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(sourceAddress, sourceAddressPassword);

            smtpClient.Send(message);
            smtpClient.Dispose();
        }
    }
}
