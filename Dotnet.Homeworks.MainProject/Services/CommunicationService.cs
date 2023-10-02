using Dotnet.Homeworks.MainProject.Services.Publishers;
using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.MainProject.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IEmailPublisher _publisher;

    public CommunicationService(IEmailPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task SendEmailAsync(SendEmail sendEmailPublish)
    {
        return _publisher.PublishAsync(sendEmailPublish);
    }
}