using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Model
{
    public class Product : BaseModel
    {
        private IConfiguration _configuration = null;
        public Product(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Product()
        {
        }

        public int ID { get; set; }

        /// <summary>
        /// 品类id
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string MainImage { get; set; }

        public List<Product> GetProducts(int pageSize = 10, int pageNum = 1)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                int start = (pageNum - 1) * pageSize;
                var sql = $"SELECT * FROM MYSQL.PRODUCT ORDER BY CREATETIME DESC LIMIT {start},{pageSize}";
                return conn.Query<Product>(sql).ToList();
            }
        }

    }
}
