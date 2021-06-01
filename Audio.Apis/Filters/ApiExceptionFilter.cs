using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace AudioVedio.Apis.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        ILogger logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.HttpContext.Request.IsApiRequest())
            {
                var result = new ObjectResult(new
                {
                    SessionId = context.HttpContext.Request.GetSessionId(),
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = context.Exception.Message,
                    Action = context.HttpContext.Request.Path.Value + context.HttpContext.Request.QueryString,
                });
                context.Result = result;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
   
            logger.LogError(context.Exception, context.Exception.ToMessage(context.HttpContext.Request.GetSessionId()));
        }
    }
}
