using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.DTO.Requset
{
    public class StatusParam
    {
        [Required]
        public int? ID { get; set; }

        [Required]
        [Range(0,1,ErrorMessage ="0-上架，1-下架，请填写0或1")]
        public int? Status { get; set; }
    }
}
