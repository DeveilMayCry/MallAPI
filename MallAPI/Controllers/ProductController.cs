using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MallAPI.DataModel.Requset;
using MallAPI.DataModel.Response;
using MallAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MallAPI.Controllers
{
    
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
        public Response Get([FromQuery]PageParams pageParams)
        {
            var products = _product.GetProducts(pageParams.PageSize,pageParams.PageNum);
            var result = new Response(products);
            return result;
        }
    }
}