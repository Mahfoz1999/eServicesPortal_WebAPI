namespace eServicesPortal_DTO.Mail;

public record MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
