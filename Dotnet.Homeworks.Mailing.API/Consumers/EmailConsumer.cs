using Dotnet.Homeworks.Mailing.API.Dto;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.Consumers;

public class EmailConsumer : IEmailConsumer
{
    private readonly IMailingService _mailingService;

    public EmailConsumer(IMailingService mailing)
    {
        _mailingService = mailing;
    }

    public Task Consume(ConsumeContext<SendEmail> context)
    {
        var eMail = ConstructEMail(context.Message);
        
        return _mailingService.SendEmailAsync(eMail);
    }

    private EmailMessage ConstructEMail(SendEmail message)
    {
        return new EmailMessage(message.ReceiverEmail, message.Subject, $"Dear {message.ReceiverName},\n{message.Content}");
    }
}