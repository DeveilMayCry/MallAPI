using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.DTO.Requset
{
    public class ProductUpdateParam
    {

        [Required]
        [Range(1,int.MaxValue)]
        public int? ID { get; set; }


        /// <summary>
        /// 品类id
        /// </summary>
        [Range(0,int.MaxValue)]
        public int? CategoryID { get; set; }


        /// <summary>
        /// 商品名
        /// </summary>
        [MinLength(1)]
        public string Name { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string MainImage { get; set; }

        /// <summary>
        /// 详情图片
        /// </summary>
        public string SubImages { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int? Stock { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Range(0,double.MaxValue)]
        public decimal? Price { get; set; }
    }
}
