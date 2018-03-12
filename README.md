# Sending Email using ASP.NET Core
A simple example of a sending email project using ASP.NET Core.

```
This tutorial is under construction!
```


What you need to do
-------------------

Considering that you know a little about .Net Core, open the 'Visual Studio Code' and create a new mvc project.

Open the file 'appsettings.json' and put yours configurations like the example below:

```json
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "EmailSettings": {
    "FromName": "Your name",
    "FromAddress": "you@domain.com",
    "ToEmail": "",
    "CcEmail": "",
    "BccEmail": "",
    "ServerAddress": "smtp.gmail.com",
    "ServerPort": 587,
    "ServerUseSsl": true,
    "Username": "",
    "Password": ""
  }
}
```

Configure this file using the data in the table below (or other server), Gmail and Hotmail worked with me:

| Server Name  | SMTP Address | Port  | SSL |
| ------------- | ------------- | ------------- | ------------- |
| Gmail | smtp.gmail.com | 587 | Yes |
| Hotmail | smtp.live.com | 587 | Yes |


Create a directory called 'Code' in the root folder of your project and in this folder create a class file called 'EmailSettings.cs' with the following content:

```csharp
namespace SendEmail.Code
{
    public class EmailSettings
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public bool ServerUseSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
```

Register class in 'startup.cs':

```csharp
...
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings")); // Add this line

            services.AddMvc();
        }
...
```

Create a directory called 'Services' in the root folder of your project and in this folder create an Email sender interface called 'IEmailSender.cs' with the following content:

```csharp
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, List<Attachment> attachments);
    }
}
```

Now create a service class file called 'EmailSender.cs' which will implement this interface:

```csharp
using Microsoft.Extensions.Options;
using SendEmail.Code;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSettings _emailSettings { get; }

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<Attachment> attachments)
        {
            Execute(email, subject, message, attachments).Wait();

            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string message, List<Attachment> attachments)
        {
            try
            {
                var toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.FromAddress, _emailSettings.FromName)
                };

                mail.To.Add(new MailAddress(toEmail));

                if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                    mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                if (!string.IsNullOrEmpty(_emailSettings.BccEmail))
                    mail.Bcc.Add(new MailAddress(_emailSettings.BccEmail));

                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mail.Attachments.Add(item);
                    }
                }

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.ServerAddress, _emailSettings.ServerPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    smtp.EnableSsl = _emailSettings.ServerUseSsl;

                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
```

Register email sender service in 'startup.cs', so that it can be injected in controllers:

```csharp
...
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>(); // Add this line

            services.AddMvc();
        }
...
```

Inject email sender service in controller constructor and then invoke SendEmailAsync() method of it:

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Models;
using SendEmail.Services;

namespace SendEmail.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender; // Add this

        public HomeController(IEmailSender emailSender) // Add this
        {
            _emailSender = emailSender;
        }

        // // Add this method
        public async Task TestAction()
        {
           await _emailSender.SendEmailAsync("your_friend@domain.com", "This is the subject", $"This is the message.");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

```


License
-------

This example application is [MIT Licensed](https://github.com/anzolin/AspNetCoreSendEmail/blob/master/LICENSE).


About the author
----------------

Hello everyone, my name is Diego Anzolin Ferreira. I'm a .NET developer from Brazil. I hope you will enjoy this simple example application as much as I enjoy developing it. If you have any problems, you can post a [GitHub issue](https://github.com/anzolin/AspNetCoreSendEmail/issues). You can reach me out at diego@anzolin.com.br.
