using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MallAPI.Authorization;
using MallAPI.DTO.Requset;
using MallAPI.DTO.Response;
using MallAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rabbitmq;

namespace MallAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private Product _product;
        private IConfiguration _configuration;
        private Publisher _publisher;
        public ProductController(Product product, IConfiguration configuration, Publisher publisher)
        {
            _product = product;
            _configuration = configuration;
            _publisher = publisher;
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
        [HttpGet("GetProductById")]
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
        [HttpGet("GetProductByName")]
        public Response GetProductByName(string name)
        {
            var products = _product.GetProductsByName(name);
            if (products.Any())
            {
                return new Response(products);
            }
            return new Response(Enum.ResultEnum.Fail, "未找到商品");
        }

        /// <summary>
        /// 更新商品图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [PermissionAuthorize("2")]
        [HttpPut("upload/{id}")]
        public Response UploadImage(int id, [Required]IFormFile file)
        {
            //todo 引入mq处理文件保存等操作
            if (file.Length == 0)
            {
                return new Response(Enum.ResultEnum.Fail, "文件大小不能为0");
            }
           
            _product.UpdateImage(id, file);
            return new Response("上传成功");
        }

        /// <summary>
        /// 商品上下架
        /// </summary>
        /// <param name="param">Status：0-上架，1-下架</param>
        /// <returns></returns>
        [PermissionAuthorize("3")]
        [HttpPut("SetSaleStatus")]
        public Response SetSaleStatus(StatusParam param)
        {
            _product.SetSaleStatus(param);
            return new Response("操作成功");
        }
    }
}