using Application;
using Application.ServiceBusConsumers;
using Azure.Messaging.ServiceBus;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Settings;
using Infrastructure.Services;
using Microservices.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var serviceBusConnectionString = builder.Configuration["ServiceBusSettings:ConnectionString"];
builder.Services.AddSingleton(new ServiceBusClient(serviceBusConnectionString));
builder.Services.AddSingleton<IServiceBusManager, ServiceBusManager>();

builder.Services.AddTransient<ShipmentCreatedConsumer>();
builder.Services.AddTransient<TestConsumer>();
builder.Services.Configure<ServiceBusHostedServiceSettings>(builder.Configuration.GetSection("ServiceBusHostedServiceSettings"));


builder.Services.AddScoped<IEventServiceBusServiceTask, EventServiceBusServiceTask>();

builder.Services.AddConsumerServiceBusConnection(x =>
{
    var topicName = builder.Configuration["ServiceBusSettings:TopicName"];
    var subscriptionName = builder.Configuration["ServiceBusSettings:SubscriptionName"];

    if (string.IsNullOrEmpty(topicName))
    {
        throw new ArgumentNullException(nameof(topicName), "ServiceBusSettings:TopicName is not set in the configuration.");
    }

    if (string.IsNullOrEmpty(subscriptionName))
    {
        throw new ArgumentNullException(nameof(subscriptionName), "ServiceBusSettings:SubscriptionName is not set in the configuration.");
    }

    x.AddConsumer<ShipmentCreatedConsumer>(topicName, subscriptionName);
    x.AddConsumer<TestConsumer>("testtopic", "testsuscription");
});

builder.Services.AddHostedService<ServiceBusHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
