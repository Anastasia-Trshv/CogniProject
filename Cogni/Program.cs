using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Authentication;
using Cogni.Authentication.Abstractions;
using Cogni.Database.Context;
using Cogni.Database.Repositories;
using Cogni.Database.Seeding;
using Cogni.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(option => option.AddPolicy(
    name: "Default",
    policy =>
    {
        policy.SetIsOriginAllowed(origin => 
            Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
            uri.Host == "localhost"
        );
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    }));
try
{
    builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
}
catch (Exception ex) { }

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "CogniApi.xml");
    opt.IncludeXmlComments(filePath);
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { "api" } // ���������, ��� ����� ������������ ����������� � ��������� � �������� "api"
        }
    });
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<SakinorvaService>();
builder.Services.AddScoped<IUserTagRepository, UserTagRepository>();
builder.Services.AddScoped<IUserTagService, UserTagService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IMbtiRepository, MbtiRepository>();
builder.Services.AddScoped<IMbtiService, MbtiService>();
builder.Services.AddScoped<IImageConverterService, ImageConverterService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(builder.Configuration["Token:Key"]),
            ValidateLifetime = true
        };
    });

var app = builder.Build();

if (args.Contains("--seed-dev", StringComparer.OrdinalIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CogniDbContext>();
    await dbContext.Database.MigrateAsync();
    await DevelopmentDatabaseSeeder.SeedAsync(dbContext);
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Default");

if (Environment.GetEnvironmentVariable("MIGRATE")?.ToLower() == "true")
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CogniDbContext>();
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
}

app.UseHttpsRedirection();
app.UseMiddleware<TokenExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
