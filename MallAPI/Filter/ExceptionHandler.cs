using MallAPI.DataModel.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Filter
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        private ILogger _logger = null;

        public ExceptionHandlerAttribute(ILogger<ExceptionHandlerAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"{context.Exception.Message}:{context.Exception.StackTrace}");
            var response = new Response(Enum.ResultEnum.Fail, context.Exception.Message);
            context.Result = new JsonResult(response);
        }
    }
}
