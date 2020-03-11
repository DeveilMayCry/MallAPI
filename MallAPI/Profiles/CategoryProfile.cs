using AutoMapper;
using MallAPI.DTO.Requset.Category;
using MallAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Profiles
{
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryUpdateParam, Category>();
            CreateMap<CategoryInsertParam, Category>();
        }
    }
}
