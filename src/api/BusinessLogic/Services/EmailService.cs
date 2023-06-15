using BusinessLogic.Abstractions;
using BusinessLogic.Options;
using FluentResults;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _settings;

        public EmailService(IOptions<EmailOptions> settings)
        {
            _settings = settings.Value;
        }

        public async Task<Result> SendEmailAsync(string email, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            var addressFrom = new MailboxAddress(_settings.Username,
                                                 _settings.Address);
            var addressTo = new MailboxAddress(string.Empty,
                                               email);

            emailMessage.From.Add(addressFrom);
            emailMessage.To.Add(addressTo);
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body,
            };

            using var client = new SmtpClient();

            string smtpServer = _settings.SmtpServer;
            int port = int.Parse(_settings.Port);

            try
            {
                await client.ConnectAsync(smtpServer,
                                      port,
                                      SecureSocketOptions.StartTls);
                client.Authenticate(addressFrom.Address,
                                    _settings.Password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }

            return Result.Ok();
        }
    }
}
