using eServicesPortal_Commends.Commends.MailCommends.Commend;
using eServicesPortal_DTO.Mail;
using MailKit.Security;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eServicesPortal_Commends.Commends.MailCommends.CommendHandler;

public class SendEmailCommendHandler : IRequestHandler<SendEmailCommend>
{
    private readonly MailSettings _mailSettings;
    public SendEmailCommendHandler(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
    public async Task Handle(SendEmailCommend request, CancellationToken cancellationToken)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Username);
        email.To.Add(MailboxAddress.Parse(request.mailRequest.ToEmail));
        email.Subject = request.mailRequest.Subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = request.mailRequest.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}
