using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ChatService.Entities;
using ChatService.Controllers;
using ChatService.Database.Context;
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
            _logger.LogWarning($"[Consumer] Event Received: {message}");
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    var invokedEvent = InvokedEvent.DeserializeEvent(message);
                    _logger.LogWarning($"[Consumer] Event deserealized: {invokedEvent}, msg: {message}");
                    if (invokedEvent == null) {
                        _logger.LogError($"[Consumer] Event could not be deserialized: {message}");
                        return;
                    };
                    var recievers = await invokedEvent.GetRecievers(db);
                    _logger.LogWarning($"[Consumer] Event recievers: {recievers}");
                    foreach (var reciever in recievers){
                        _logger.LogWarning($"[Consumer] Event sent to: {reciever}");
                        await _hubService.SendMessage(invokedEvent.type, reciever, message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing RabbitMQ message: {ex}");
            }
        };
        await channel.BasicConsumeAsync(queueName.QueueName, autoAck: true, consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken); // Keep it alive
    }
}
