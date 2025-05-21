using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Presentation.Models;

namespace Presentation.Services;

public class EmailServiceBusListener : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IEmailService _emailService;

    public EmailServiceBusListener(ServiceBusClient client, IEmailService emailService)
    {
        _processor = client.CreateProcessor("emailqueue", new ServiceBusProcessorOptions());
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        var message = JsonSerializer.Deserialize<SendVerificationCodeDto>(body);

        if (message != null)
        {
            await _emailService.SendVerificationEmailAsync(message);
        }

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        // Handle the error
        Console.WriteLine($"Error: {args.Exception.Message}");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await _processor.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}

