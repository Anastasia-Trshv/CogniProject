using ChatService.Abstractions;
using ChatService.Controllers;
using ChatService.Database.Context;
using ChatService.Models;
using ChatService.Repository;
using ChatService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using RabbitMQ.Client;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowFrontend",
//         policy => policy
//             .WithOrigins("http://localhost:5173")
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .AllowCredentials());
// });

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisHost = builder.Configuration["Redis:Host"];
    var redisPort = builder.Configuration["Redis:Port"];
    var redisUser = builder.Configuration["Redis:User"];
    var redisPassword = builder.Configuration["Redis:Password"] ;
    var configuration = ConfigurationOptions.Parse($"{redisHost}:{redisPort}");
    configuration.User = redisUser;
    configuration.Password = redisPassword;
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddDbContext<AppDbContext>();

var rabbitMqConnectionString = builder.Configuration.GetValue<string>("RabbitMQ:ConnectionString");
builder.Services.AddSingleton(await new ConnectionFactory { Uri = new Uri(rabbitMqConnectionString) }.CreateConnectionAsync());

builder.Services.AddMinio(client =>
{
    var minioEndpoint = builder.Configuration.GetValue<string>("Minio:Endpoint");
    var minioAccessKey = builder.Configuration.GetValue<string>("Minio:AccessKey");
    var minioSecretKey = builder.Configuration.GetValue<string>("Minio:SecretKey");

    client.WithEndpoint(minioEndpoint)
          .WithCredentials(minioAccessKey, minioSecretKey)
          .WithSSL(false);
});

builder.Services.AddSignalR();
builder.Services.AddScoped<ChatHubController>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IHubService, HubService>();
builder.Services.AddTransient<IRedisRepository, RedisRepository>();
builder.Services.AddHostedService<RabbitMqConsumerService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations.");
    }
}

// app.UseCors("AllowFrontend");

app.MapHub<ChatHubController>("api/chatHub");

app.MapControllers();

app.Run();
