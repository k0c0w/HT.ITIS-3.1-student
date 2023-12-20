﻿using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IBus _publisher;

    public CommunicationService(IBus publisher)
    {
        _publisher = publisher;
    }

    public Task SendEmailAsync(SendEmail sendEmailPublish)
    {
        return _publisher.Publish(sendEmailPublish);
    }
}