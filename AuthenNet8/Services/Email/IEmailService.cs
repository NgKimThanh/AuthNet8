namespace AuthenNet8.Services.Email
{
    public interface IEmailService
    {
        Task Send(string toEmail, string subject, string body);
    }
}
