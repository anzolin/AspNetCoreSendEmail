using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<Attachment> attachments);
    }
}
