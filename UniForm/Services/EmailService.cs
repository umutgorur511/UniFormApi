using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    private readonly bool _enableSsl;

    public EmailService(IConfiguration configuration) {
        var smtpSettings = configuration.GetSection("SmtpSettings");
        _smtpServer = smtpSettings["Server"];
        _smtpPort = int.Parse(smtpSettings["Port"]);
        _smtpUser = smtpSettings["User"];
        _smtpPass = smtpSettings["Password"];
        _enableSsl = bool.Parse(smtpSettings["EnableSsl"]);
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body) {
        try {
            using (var smtpClient = new SmtpClient(_smtpServer)) {
                smtpClient.Port = _smtpPort;
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                smtpClient.EnableSsl = _enableSsl;

                var mailMessage = new MailMessage {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email send failed: {ex.Message}");
            return false;
        }
    }
}
