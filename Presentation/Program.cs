using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

app.UseCors(app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
