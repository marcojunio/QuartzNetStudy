using System.Text.Json;
using System.Text.Json.Serialization;
using Quartz;
using QuartzImplementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TestingCalledService>();

builder.Services.AddQuartz(q => { ConfigurationsQuartz.SettingJobs(q, builder.Configuration); });

builder.Services.AddQuartzHostedService(e => e.WaitForJobsToComplete = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<Middleware>();

app.MapControllers();

app.Run();