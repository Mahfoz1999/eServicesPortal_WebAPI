using eServicesPortal_DTO.Mail;
using MediatR;

namespace eServicesPortal_Commends.Commends.MailCommends.Commend;

public record SendEmailCommend(MailRequest mailRequest) : IRequest;
