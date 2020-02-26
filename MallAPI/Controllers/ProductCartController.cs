using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MallAPI.Authorization;
using MallAPI.DTO.Response;
using MallAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MallAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCartController : ControllerBase
    {
        private CartProduct _productCart;

        public ProductCartController(CartProduct productCart)
        {
            _productCart = productCart;
        }

        /// <summary>
        /// 查询购物车内的产品
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize("8")]
        [HttpGet("GetCartProducts")]
        public Response GetCartProducts()
        {
            var userId = User.FindFirst("userId").Value;
            var result = _productCart.GetCartProducts(long.Parse(userId));
            return new Response(result);
        }

        /// <summary>
        /// 查询购物车内商品总数量
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize("8")]
        [HttpGet("GetProductCount")]
        public Response GetProductCount()
        {
            var userId = User.FindFirst("userId").Value;
            var result = _productCart.GetProductCount(long.Parse(userId));
            return new Response(result);
        }

        /// <summary>
        /// 增加购物车产品数量，每次加1
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [PermissionAuthorize("9")]
        [HttpPost("IncresrProduct")]
        public Response IncresrProduct([Required]long? productId)
        {
            var userId = User.FindFirst("userId").Value;
            var result = _productCart.IncresrProduct(long.Parse(userId), productId.Value);
            return new Response(result);
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        [PermissionAuthorize("9")]
        [HttpPost("InsertProductToCart")]
        public Response InsertProductToCart([Required]long? productId, [Required]bool? select)
        {
            var userId = User.FindFirst("userId").Value;
            var result = _productCart.InsertProductToCart(long.Parse(userId), productId.Value, select.Value);
            return new Response(result);
        }

        /// <summary>
        /// 移除购物车内某件商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [PermissionAuthorize("9")]
        [HttpDelete("RemoveProductFromCart")]
        public Response RemoveProductFromCart([Required]long? productId)
        {
            var userId = User.FindFirst("userId").Value;
            var result = _productCart.RemoveProductFromCart(long.Parse(userId), productId.Value);
            return new Response(result);
        }

        /// <summary>
        /// 全选或全不选
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [PermissionAuthorize("9")]
        [HttpGet("SelectOrNegative")]
        public Response SelectOrNegative([Required]bool? select)
        {
            var userId = User.FindFirst("userId").Value;
            var result = _productCart.SelectOrNegative(long.Parse(userId), select.Value);
            return new Response(result);
        }
    }
}