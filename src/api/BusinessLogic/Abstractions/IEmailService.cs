using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IEmailService
    {
        Task<Result> SendEmailAsync(string email, string subject, string body);
    }
}
