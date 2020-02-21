using Microsoft.Extensions.Configuration;
using Rabbitmq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.MessageMQ
{
    public class MessagePublisher : Publisher
    {
        private IConfiguration _configuration;
        public MessagePublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override string HostName => _configuration["RabbitMQ:HostName"];

        public override string UserName => _configuration["RabbitMQ:UserName"];

        public override string Password => _configuration["RabbitMQ:Password"];

        public override string Exchange => _configuration["RabbitMQ:Exchange"];

        public override string RouteKey => _configuration["RabbitMQ:RouteKey"];

    }
}
