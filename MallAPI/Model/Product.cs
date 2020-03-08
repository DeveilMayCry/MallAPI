using Dapper;
using MallAPI.DTO.Product.Requset;
using MallAPI.DTO.Requset;
using MallAPI.DTO.Requset.Product;
using MallAPI.lib;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Rabbitmq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Model
{
    public class Product : BaseModel
    {
        private IConfiguration _configuration;
        private Publisher _publisher;
        private Category _category;
        private readonly string _productTableName = "MALL.PRODUCT";
        public Product(IConfiguration configuration, Publisher publisher,Category category)
        {
            _configuration = configuration;
            _publisher = publisher;
            _category = category;
        }

        public Product()
        {
        }

        public int ID { get; set; }

        /// <summary>
        /// 品类id
        /// </summary>
        public long CategoryID { get; set; }

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
        /// 详情图片
        /// </summary>
        public string SubImages { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }

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
                var sql = $"SELECT * FROM {_productTableName} WHERE STATUS = 0 ORDER BY CREATETIME DESC LIMIT @start,@pageSize";
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
                var sql = $"SELECT * FROM {_productTableName} WHERE ID = @id AND STATUS = 0 ";
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
                var sql = $"SELECT * FROM {_productTableName} WHERE NAME like @name AND STATUS = 0 ";
                return conn.Query<Product>(sql, new { name = $"%{name}%" }).ToList();
            }
        }

        /// <summary>
        /// 更新商品图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        public void UpdateImage(int id, IFormFile file)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                using (var fileStream = file.OpenReadStream())
                {
                    var bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, bytes.Length);
                    _publisher.Publish((file.FileName, bytes));
                }
                var sql = $"UPDATE {_productTableName} SET MAINIMAGE = @fileName WHERE ID = @id";
                int result = conn.Execute(sql, new { id, fileName = file.FileName });
                if (result == 0)
                {
                    throw new Exception("商品不存在");
                }
            }
        }

        /// <summary>
        /// 上下架商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void SetSaleStatus(StatusParam param)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"UPDATE {_productTableName} SET STATUS =@status WHERE ID= @id ";
                var result = conn.Execute(sql, new { status = param.Status, id = param.ID });
                if (result == 0)
                {
                    throw new Exception("商品不存在");
                }
            }
        }

        /// <summary>
        /// 更新商品部分信息
        /// </summary>
        /// <param name="param"></param>
        public void UpdateProduct(long id,ProductUpdateParam param)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                if (param.CategoryID.HasValue && !_category.ExistCategory(param.CategoryID.Value))
                {
                    throw new Exception("CategoryID不存在");
                }

                var result = conn.Update(_productTableName, id, param);
                if (result == 0)
                {
                    throw new Exception("商品不存在");
                }
            }
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="param"></param>
        public void InsertProduct(ProductInsertParam param)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                if (!_category.ExistCategory(param.CategoryID.Value))
                {
                    throw new Exception("CategoryID不存在");
                }

                var result = conn.Insert("MALL.PRODUCT", param);
                if (result == 0)
                {
                    throw new Exception("商品不存在");
                }
            }
        }
    }
}
