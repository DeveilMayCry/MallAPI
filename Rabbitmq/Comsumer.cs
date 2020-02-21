using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitmq
{
    public abstract class Comsumer : IHostedService
    {

        public virtual string HostName { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string Exchange { get; set; }

        public virtual string BindKey { get; set; }

        public virtual string QueueName { get; set; }



        /// <summary>
        /// 消息处理逻辑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void CustomComsumer_Received(object sender, BasicDeliverEventArgs e);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password

            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, true, false, false, null);
            channel.ExchangeDeclare(Exchange, "direct", true, false, null);
            channel.QueueBind(QueueName, Exchange, BindKey, null);
            var customComsumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(QueueName, false, customComsumer);
            customComsumer.Received += CustomComsumer_Received;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
