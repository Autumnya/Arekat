using Arekat.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Arekat.Server.Helpers;

var builder = WebApplication.CreateBuilder(args);
IHostEnvironment env = builder.Environment;
var appsettings = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Configuration
    .AddIniFile("appsettings.ini", optional: true, reloadOnChange: true)
    .AddIniFile($"appsettings.{env.EnvironmentName}.ini", true, true);
builder.Services.AddControllers();
builder.Services.AddDbContext<BaseContext>(ServiceLifetime.Transient);
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        var signKey = appsettings.GetValue<string>("Jwt:SecretKey");
        if (string.IsNullOrEmpty(signKey))
        {
            throw new Exception("No jwt sign key");
        }
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey)),
            ValidIssuer = appsettings.GetValue<string>("Jwt:Issuer"),
            ValidAudience = appsettings.GetValue<string>("Jwt:Audience"),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero // 将时钟偏差设置为零，避免延迟问题
        };
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("User", policy => policy.RequireRole("User", "SuperAdmin"))
    .AddPolicy("ChartDesigner", policy => policy.RequireRole("ChartDesigner"))
    .AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new JwtHelper(appsettings));
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod(); //允许任何http方法
        builder.AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
