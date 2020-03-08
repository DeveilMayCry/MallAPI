using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MallAPI.Authorization;
using MallAPI.DTO.Requset.Category;
using MallAPI.DTO.Response;
using MallAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MallAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {

        private Category _category;
        private IConfiguration _configuration;
        public CategoryController(Category category, IConfiguration configuration)
        {
            _category = category;
            _configuration = configuration;
        }

        /// <summary>
        /// 根据id获取品类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorize("5")]
        [HttpGet("{id:int}")]
        public Response GetCategories(int id)
        {
            var result= _category.GetCategories(id);
            return new Response(result);
        }

        /// <summary>
        /// 设置品类名称
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PermissionAuthorize("6")]
        [HttpPatch("SetCategoryName")]
        public Response SetCategoryName(CategoryUpdateParam param)
        {
            _category.SetCategoryName(param);
            return new Response("操作成功");
        }

        /// <summary>
        /// 创建新品类
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PermissionAuthorize("7")]
        [HttpPost("CreateCategoryName")]
        public Response CreateCategoryName(CategoryInsertParam param)
        {
            _category.InsertCategory(param);
            return new Response("操作成功");
        }

        /// <summary>
        /// 递归获取子品类节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorize("5")]
        [HttpGet("Recursive/{id:long}")]
        public Response GetCaregoryRecursive(long id)
        {
            var result =  _category.GetCaregoryRecursive(id);
            return new Response(result);
        }
    }
}