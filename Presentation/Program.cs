using Azure.Messaging.ServiceBus;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connection = builder.Configuration["ServiceBusConnection"];
builder.Services.AddSingleton<ServiceBusClient>(sp => new ServiceBusClient(connection));
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHostedService<EmailServiceBusListener>();

var app = builder.Build();

app.UseCors(app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
