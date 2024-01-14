using System.Net.Mail;

namespace MCSHiPPERS_Task.Repository.IRepository
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetEmail(string userEmail, string link);
    }
}
