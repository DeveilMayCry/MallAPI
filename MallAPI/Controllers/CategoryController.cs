using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;
        public CategoryController(Category category, IConfiguration configuration, IMapper mapper)
        {
            _category = category;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// 根据id获取品类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorize(Enum.PermissionNameEnum.CategoryQuery)]
        [HttpGet("{id:int}")]
        public Response GetCategories(int id)
        {
            _category.ID = id;
            var result = _category.GetCategories();
            return new Response(result);
        }

        /// <summary>
        /// 设置品类名称
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PermissionAuthorize(Enum.PermissionNameEnum.CategoryModify)]
        [HttpPatch]
        public Response SetCategoryName(CategoryUpdateParam param)
        {
            _mapper.Map(param, _category);
            _category.SetCategoryName();
            return new Response("操作成功");
        }

        /// <summary>
        /// 创建新品类
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PermissionAuthorize(Enum.PermissionNameEnum.CategoryAdd)]
        [HttpPost]
        public Response CreateCategoryName(CategoryInsertParam param)
        {
            _mapper.Map(param, _category);
            _category.InsertCategory();
            return new Response("操作成功");
        }

        /// <summary>
        /// 递归获取子品类节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorize(Enum.PermissionNameEnum.CategoryQuery)]
        [HttpGet("Recursive/{id:long}")]
        public Response GetCaregoryRecursive(long id)
        {
            _category.ID = id;
            var result = _category.GetCaregoryRecursive();
            return new Response(result);
        }
    }
}