using System.Net;
using System.Net.Mail;

namespace e_commerce_platform.Models;


public interface IEmailService
{

    Task SendEmailAsync(string email, string subject, string message);
}

public class SmtpEmailService : IEmailService
{
    private IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using (var clinet = new SmtpClient(_configuration["Email:Host"]))
        {
            clinet.UseDefaultCredentials = false;
            clinet.Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);

            clinet.Port = 587;
            clinet.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:Username"]!),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);
            await clinet.SendMailAsync(mailMessage);
        }
    }
}