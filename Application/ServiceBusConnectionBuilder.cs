using Application.ServiceBusConsumers;
using Azure.Messaging.ServiceBus;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Builders
{
     public class ServiceBusConnectionBuilder
     {
            private readonly IServiceBusManager _serviceBusManager;
            private readonly IServiceProvider _serviceProvider;
            private readonly IConfiguration _configuration;

            public ServiceBusConnectionBuilder(IServiceBusManager serviceBusManager,IServiceProvider serviceProvider, IConfiguration configuration)
         {
                _serviceBusManager = serviceBusManager;
                _serviceProvider = serviceProvider;
                _configuration = configuration;
            }

         public ServiceBusConnectionBuilder AddConsumer<TConsumer>(string topicName, string suscrptionName) 
             where TConsumer : IServiceBusConsumer
         {
                var connectinStringServiceBus = _configuration["ServiceBusSettings:ConnectionString"];
                var serviceBusClient = new ServiceBusClient(connectinStringServiceBus);
                _serviceBusManager.AddConsumer<TConsumer>(topicName, suscrptionName, serviceBusClient, _serviceProvider);
             return this;
         }
     }
}
