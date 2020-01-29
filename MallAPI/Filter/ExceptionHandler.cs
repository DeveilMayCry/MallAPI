using MallAPI.DataModel.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Filter
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var response = new Response(Enum.ResultEnum.Fail, context.Exception.Message);
            context.Result = new JsonResult(response);
        }
    }
}
