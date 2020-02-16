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

        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public List<Product> GetProducts(int pageSize = 10, int pageNum = 1)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                int start = (pageNum - 1) * pageSize;
                var sql = $"SELECT * FROM MALL.PRODUCT WHERE STATUS = 0 ORDER BY CREATETIME DESC LIMIT @start,@pageSize";
                return conn.Query<Product>(sql, new { start, pageSize }).ToList();
            }
        }

        /// <summary>
        /// 根据id查询产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductById(int id)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"SELECT * FROM MALL.PRODUCT WHERE ID = @id AND STATUS = 0 ";
                return conn.QueryFirstOrDefault<Product>(sql, new { id });
            }
        }

        /// <summary>
        /// 根据名称模糊查询产品
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Product> GetProductsByName(string name)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"SELECT * FROM MALL.PRODUCT WHERE NAME like @name AND STATUS = 0 ";
                return conn.Query<Product>(sql, new { name = $"%{name}%" }).ToList();
            }
        }
    }
}
