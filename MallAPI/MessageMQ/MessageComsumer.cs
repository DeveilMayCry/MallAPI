using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rabbitmq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MallAPI.MessageMQ
{
    public class MessageComsumer : Comsumer
    {
        private IConfiguration _configuration;

        public MessageComsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override string HostName => _configuration["RabbitMQ:HostName"];

        public override string UserName => _configuration["RabbitMQ:UserName"];

        public override string Password => _configuration["RabbitMQ:Password"];

        public override string Exchange => _configuration["RabbitMQ:Exchange"];

        public override string BindKey => _configuration["RabbitMQ:RouteKey"];

        public override string QueueName => _configuration["RabbitMQ:QueueName"];


        protected override void CustomComsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body);
            (string fileName,byte[] data) tuple = JsonConvert.DeserializeObject<(string,byte[])>(body);
            var filePath = Path.Combine(_configuration["StoredFilesPath"],
                    tuple.fileName);
            using (FileStream stream = File.Create(filePath))
            {
                stream.Write(tuple.data, 0, tuple.data.Length);
            }
            EventingBasicConsumer comsumer = sender as EventingBasicConsumer;
            comsumer.Model.BasicAck(e.DeliveryTag, false);
        }
    }
}
