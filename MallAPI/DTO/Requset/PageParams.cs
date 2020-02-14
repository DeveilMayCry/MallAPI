using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.DTO.Requset
{
    public class PageParams
    {
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public int PageNum { get; set; } = 1;
    }
}
