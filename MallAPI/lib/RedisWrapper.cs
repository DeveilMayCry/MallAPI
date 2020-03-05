using Microsoft.Extensions.Configuration;
using Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.lib
{
    public class RedisWrapper : Redis.Redis
    {
        private IConfiguration _configuration;
        public override string RedisConnectStr => _configuration["Redis:ConnectString"];


        public RedisWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}
