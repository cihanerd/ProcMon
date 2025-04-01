using Microsoft.OpenApi.Models;
using ProcessMonitor.Application;
using ProcessMonitor.Domain;
using ProcessMonitor.Infrastructure;

const string CORS_POLICY = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.WebHost.UseIISIntegration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Process Monitor API",
        Version = "v1",
        Description = "This API provides real-time monitoring of system processes, including CPU and memory usage.",
        Contact = new OpenApiContact
        {
            Name = "Cihan Erdogan",
            Email = "erdogan.cihan@outlook.com"
        }
    });
});

builder.Services.AddTransient<IProcessRepository, ProcessRepository>();
builder.Services.AddSingleton<IProcessMonitorHub, SignalRProcessMonitorHub>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CORS_POLICY, opt =>
        opt.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(CORS_POLICY);
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<ProcessMonitorHub>(SignalRConsts.ProcessHub);

app.MapControllers();

app.Run();
