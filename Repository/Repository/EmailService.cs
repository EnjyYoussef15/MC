using MCSHiPPERS_Task.Repository.IRepository;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using System.Net.Http;

namespace MCSHiPPERS_Task.Repository.Repository
{
    public class EmailService : IEmailService
    {
        

        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _username = "demo.system11@gmail.com";
        private readonly string _password = "lklgxfzytaiumprr";


        public async Task<bool> SendPasswordResetEmail(string userEmail, string link)
        {

            var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            };

            var from = new MailAddress(_username, "MC");
            var subject = "Password Reset";
            var to = new MailAddress(userEmail);

           
            var msg = new MailMessage(from, to)
            {
                Subject = subject,
                Body = link,
                IsBodyHtml = false,

            };

            try
            {
                await smtpClient.SendMailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

           
            




        }
    }
}
