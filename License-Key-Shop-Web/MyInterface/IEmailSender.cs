namespace License_Key_Shop_Web.MyInterface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml);
    }
}
