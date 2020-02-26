using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Model
{
    public class CartProduct
    {
        private IConfiguration _configuration;
        private readonly string _tableName = "MALL.productCart";

        public long ID { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 勾选数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool ProductSelect { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal ProductTotalPrice { get { return Quantity * Price; } }

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

        public CartProduct(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CartProduct()
        {

        }

        /// <summary>
        /// 根据userid查询购物车中的产品
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetCartProducts(long userId)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"SELECT * from {_tableName} c INNER JOIN mall.product p ON c.productId = p.id WHERE c.userId= @userId and c.`status`=0 and p.`status`=0";
                var result = conn.Query<CartProduct>(sql, new { userId });
                return CaculateProducts(result.ToList());
            }
        }

        /// <summary>
        /// 计算购物车内总价格及是否都勾选了
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private object CaculateProducts(List<CartProduct> products)
        {
            bool selectedAll = products.All(p => p.ProductSelect);
            decimal cartTotalPrice = products.Select(p => p.ProductTotalPrice).Sum();
            return new { cartProductVoList = products, selectedAll, cartTotalPrice };
        }


        /// <summary>
        /// 增加购物车内商品数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns>返回购物车最新列表</returns>
        public object IncresrProduct(long userId, long productId)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"UPDATE {_tableName} SET quantity = quantity+1 WHERE userId = @userId AND productId = @productId AND STATUS=0";
                var result = conn.Execute(sql, new { userId, productId });
                if (result == 0)
                {
                    throw new Exception("购物车下无对应产品，操作失败");
                }
                return GetCartProducts(userId);
            }
        }

        /// <summary>
        /// 购物车新增商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public object InsertProductToCart(long userId, long productId, bool select)
        {
            //存在的话则数量+1
            if (ExistProduct(userId, productId))
            {
                return IncresrProduct(userId, productId);
            }

            //新增进购物车
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var initQuantity = 1;
                var sql = $"insert into {_tableName} (`userid`,`productId`,`quantity`,`productselect`) value ({userId},{productId},{initQuantity},{select})";
                var result = conn.Execute(sql);
                if (result == 0)
                {
                    throw new Exception("操作失败");
                }

                return GetCartProducts(userId);
            }
        }

        /// <summary>
        /// 用户购物车是否存在商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private bool ExistProduct(long userId, long productId)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"select 1 from {_tableName} where userId=@userId and productId = @productId and status=0";
                var result = conn.Query(sql, new { userId, productId });
                return result.Any();
            }
        }

        /// <summary>
        /// 移除购物车内某件商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public object RemoveProductFromCart(long userId,long productId)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"UPDATE {_tableName} SET status = 1 WHERE userId = @userId AND productId = @productId AND STATUS=0";
                var result = conn.Execute(sql, new { userId, productId });
                if (result == 0)
                {
                    throw new Exception("购物车下无对应产品，操作失败");
                }
                return GetCartProducts(userId);
            }
        }

        /// <summary>
        /// 全选或全不选
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productselect"></param>
        /// <returns></returns>
        public object SelectOrNegative(long userId,bool productselect)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"UPDATE {_tableName} SET productselect = @productselect WHERE userId = @userId and STATUS=0";
                var result = conn.Execute(sql, new { userId, productselect });
                if (result == 0)
                {
                    throw new Exception("购物车下无对应产品，操作失败");
                }
                return GetCartProducts(userId);
            }
        }

        /// <summary>
        /// 查询购物车内商品总数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetProductCount(long userId)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"SELECT * from {_tableName} c INNER JOIN mall.product p ON c.productId = p.id WHERE c.userId= @userId and c.`status`=0 and p.`status`=0";
                var result = conn.Query<CartProduct>(sql, new { userId });
                return result.Sum(p => p.Quantity);
            }
              
        }


    }
}
