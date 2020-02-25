using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.DTO.Requset.Category
{
    public class CategoryUpdateParam
    {
        [Required]
        [Range(1,long.MaxValue)]
        public long? ID { get; set; }

        [Required]
        [MinLength(1,ErrorMessage ="不得少于1个字")]
        public string Name { get; set; }
    }
}
