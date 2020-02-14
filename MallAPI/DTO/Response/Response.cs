using MallAPI.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.DTO.Response
{
    public class Response
    {
        public Response(object data) : this(ResultEnum.Success, data, default(string))
        {
        }

        public Response(ResultEnum status, string errorMessage) : this(status, default(string), errorMessage)
        {

        }

        public Response(ResultEnum status, object data, string errorMessage)
        {
            Status = status;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public ResultEnum Status { get; set; }

        public object Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
