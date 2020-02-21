using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitmq
{
    public class Publisher : IHostedService
    {

        public virtual string HostName { get; set; }


        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string Exchange { get; set; }

        public virtual string RouteKey { get; set; }


        public virtual void Publish(object data)
        {
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password

            };
            var connection = factory.CreateConnection();
            var  channel = connection.CreateModel();
            channel.ConfirmSelect();
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            channel.BasicPublish(Exchange, RouteKey, properties, body);
            if (!channel.WaitForConfirms(TimeSpan.FromSeconds(60)))
            {
                throw new Exception($"投递消息到broker失败,data为{data}");
            }
            channel.Close();
            connection.Close();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
