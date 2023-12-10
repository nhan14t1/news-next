#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NEWS.Services.AutoMappings;
using NEWS.WebAPI.Extensions;
using NEWS.WebAPI.JwtUtils;
using NEWS.WebAPI.Middlewares;
using NEWS.WebAPI.Services;
using System.Text;
using System.Text.Json;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
 .SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
 .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
 .AddEnvironmentVariables()
 .Build();

// Add services to the container.
var services = builder.Services;
services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
var config = new MapperConfiguration(c =>
{
    c.AddProfile<MappingConfigProfile>();
});
services.AddSingleton<IMapper>(op=> config.CreateMapper());

var jwtTokenConfig = configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
services.AddSingleton(jwtTokenConfig);
services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfig.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
        ValidAudience = jwtTokenConfig.Audience,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
services.AddSingleton<IFileService, FileService>();

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

services.AddDatabase(configuration)
                .AddRepositories()
                .AddServices();

//services.AddHostedService<TimedHostedService>();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseFileServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
