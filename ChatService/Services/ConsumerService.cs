using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Cogni.Database.Entities;
using Cogni.Database.Context;
using ChatService.Controllers;
using RabbitMQ.Client.Events;
using ChatService.Models;
using ChatService.Abstractions;


namespace ChatService.Services;
public class RabbitMqConsumerService : BackgroundService
{
    private readonly IConnection _rabbitMqConnection;
    private readonly IHubService _hubService;
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    public RabbitMqConsumerService(
        IConnection rabbitMqConnection,
        IHubService hubService,
        ILogger<RabbitMqConsumerService> logger,
        IServiceScopeFactory scopeFactory
    ){
        _rabbitMqConnection = rabbitMqConnection;
        _hubService = hubService;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await _rabbitMqConnection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync("invoked_event", ExchangeType.Fanout, durable: true);

        var queueName = await channel.QueueDeclareAsync("", durable: false, exclusive: true, autoDelete: true);
        await channel.QueueBindAsync(queueName.QueueName, "invoked_event", "");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("[Consumer] Event Received: {Message}", message);
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    var invokedEvent = InvokedEvent.DeserializeEvent(message);
                    _logger.LogInformation("[Consumer] Event deserealized: {InvokedEvent}, msg: {Message}", invokedEvent, message);
                    if (invokedEvent == null) {
                        _logger.LogError("[Consumer] Event could not be deserialized: {Message}", message);
                        return;
                    };
                    var recievers = await invokedEvent.GetRecievers(db);
                    _logger.LogInformation("[Consumer] Event recievers: {Receivers}", recievers);
                    foreach (var reciever in recievers){
                        _logger.LogInformation("[Consumer] Event sent to: {Receiver}", reciever);
                        invokedEvent.PreSend(reciever);
                        await _hubService.SendMessage(invokedEvent.type, reciever, invokedEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing RabbitMQ message");
            }
        };
        await channel.BasicConsumeAsync(queueName.QueueName, autoAck: true, consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken); // Keep it alive
    }
}
