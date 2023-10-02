using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.MainProject.Services.Publishers
{
    public interface IEmailPublisher : IBus
    {
        Task PublishAsync(SendEmail sendEmailMessage)
        {
            return PublishAsync(sendEmailMessage);
        }
    }
}
