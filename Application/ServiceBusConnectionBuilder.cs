using Application.ServiceBusConsumers;
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

     public ServiceBusConnectionBuilder(IServiceBusManager serviceBusManager)
     {
         _serviceBusManager = serviceBusManager;
     }

     public ServiceBusConnectionBuilder AddConsumer<TConsumer>(string topicName, string suscrptionName) 
         where TConsumer : IServiceBusConsumer
     {
         _serviceBusManager.AddConsumer<TConsumer>(topicName, suscrptionName);
         return this;
     }
 }
}
