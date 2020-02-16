using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MallAPI.Authorization;
using MallAPI.DTO.Requset;
using MallAPI.DTO.Response;
using MallAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MallAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private Product _product = null;
        public ProductController(Product product)
        {
            _product = product;
        }

        /// <summary>
        /// 产品列表
        /// </summary>
        [HttpGet]
        [PermissionAuthorize("1")]
        public Response Get([FromQuery]PageParams pageParams)
        {
            var products = _product.GetProducts(pageParams.PageSize, pageParams.PageNum);
            var result = new Response(products);
            return result;
        }

        /// <summary>
        /// 根据id查询产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public Response GetProductById(int id)
        {
            var product = _product.GetProductById(id);
            if (product != null)
            {
                return new Response(product);
            }
            return new Response(Enum.ResultEnum.Fail, "未找到商品");
        }

        /// <summary>
        /// 根据名称查询产品
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("name/{name}")]
        public Response GetProductByName(string name)
        {
            var products = _product.GetProductsByName(name);
            if (products.Any())
            {
                return new Response(products);
            }
            return new Response(Enum.ResultEnum.Fail, "未找到商品");
        }
    }
}