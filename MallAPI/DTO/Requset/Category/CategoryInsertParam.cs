using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.DTO.Requset.Category
{
    public class CategoryInsertParam
    {
        [Range(0,long.MaxValue)]
        public long? ParentId { get; set; }

        [Required]
        [MinLength(1,ErrorMessage ="名称最小长度为1")]
        public string Name { get; set; }

        [Range(0,1)]
        public int? Status { get; set; }
    }
}
