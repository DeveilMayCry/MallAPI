using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Model
{
    public class BaseModel
    {

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime DeleteTime { get; set; }

        /// <summary>
        /// 0-正常，1-不可用
        /// </summary>
        public int Status { get; set; }
    }
}
