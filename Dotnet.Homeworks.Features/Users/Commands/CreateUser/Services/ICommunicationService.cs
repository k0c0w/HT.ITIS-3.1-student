using Dotnet.Homeworks.Shared.MessagingContracts.Email;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;

public interface ICommunicationService
{
    public Task SendEmailAsync(SendEmail sendEmailDto);
}