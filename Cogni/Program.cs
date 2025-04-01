using Cogni.Abstraction.Services;
using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Authentication;
using Cogni.Authentication.Abstractions;
using Cogni.Database.Context;
using Cogni.Database.Repositories;
using Cogni.Services;
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
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    }));

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

var redisConnection = builder.Configuration.GetValue<string>("Redis:ConnectionString");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddDbContext<CogniDbContext>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddTransient<IUserTagRepository, UserTagRepository>();
builder.Services.AddTransient<IUserTagService, UserTagService>();
builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IFriendRepository, FriendRepository>();
builder.Services.AddTransient<IFriendService, FriendService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<IMbtiRepository, MbtiRepository>();
builder.Services.AddTransient<IMbtiService, MbtiService>();
builder.Services.AddTransient<IImageConverterService, ImageConverterService>();
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

            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(builder.Configuration["Token:Key"]),
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Default");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
