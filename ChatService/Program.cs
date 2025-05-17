using System.Reflection;
using ChatService.Abstractions;
using ChatService.Controllers;
using Cogni.Database.Context;
using Cogni.Authentication;
using ChatService.Models;
using ChatService.Repository;
using ChatService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Minio;
using RabbitMQ.Client;
using StackExchange.Redis;
using ChatService.CustomSwaggerGen;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApi", Version = "v1" });
    opt.DocumentFilter<CustomSigRDocsGen>();
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    opt.AddSignalRSwaggerGen();
});

// DEV-ONLY
builder.Services.AddCors(options =>
{
    options.AddPolicy("DEV-CHATS-AllowFrontend",
        policy => policy
            .SetIsOriginAllowed(origin => 
                Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                uri.Host == "localhost"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

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

builder.Services.AddDbContext<CogniDbContext>();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.Map("/Swagger/inject.js", async context =>
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "ChatService.SwaggerExt.inject.js";
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("JavaScript file not found.");
                return;
            }
            context.Response.ContentType = "application/javascript";
            await stream.CopyToAsync(context.Response.Body);
        }
    });
    app.UseSwaggerUI(c => {
        c.InjectJavascript("/Swagger/inject.js");
    });
    app.UseCors("DEV-CHATS-AllowFrontend");
}

app.UseMiddleware<TokenExceptionHandlingMiddleware>();

app.MapHub<ChatHubController>("chat/hub");

app.MapControllers();

app.Run();
