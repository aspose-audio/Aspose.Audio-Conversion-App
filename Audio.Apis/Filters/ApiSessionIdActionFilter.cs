using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace AudioVedio.Apis.Filters
{
    public class ApiSessionIdActionFilter : IActionFilter
    {
        ILogger logger;
        public ApiSessionIdActionFilter(ILogger<ApiExceptionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.IsApiRequest())
            {
                var sessionId = context.HttpContext.Request.GetSessionId();
                logger.LogWarning($"Response[sessionId:{sessionId}]{GetRequestUrl(context.HttpContext.Request)}>>{GetJsonResult(context)}");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.IsApiRequest())
            {
                var sessionId = context.HttpContext.Request.GetSessionId();
                sessionId = !string.IsNullOrEmpty(sessionId) ? sessionId : Guid.NewGuid().ToString();
                context.HttpContext.Request.SetSessionId(sessionId);
                logger.LogWarning($"Request [sessionId:{sessionId}]{GetRequestUrl(context.HttpContext.Request)}");

            }
        }

        private string GetRequestUrl(HttpRequest request)
        {
            if (request != null && request.Path != null)
            {
                return request.Path.ToString().ToLower() + request.QueryString;
            }
            return "";
        }

        private string GetJsonResult(ActionExecutedContext context)
        {
            var response = context.Result;
            if (response != null) {
                if (response is ObjectResult)
                {
                    var result = response as ObjectResult;
                    return "JSON<" + JsonConvert.SerializeObject(result.Value) + ">";
                }
                else if (response is JsonResult)
                {
                    var result = response as JsonResult;
                    return "JSON<"+ JsonConvert.SerializeObject(result.Value) + ">";
                }
                else if (response is FileStreamResult)
                {
                    var result = response as FileStreamResult;
                    return "FILE<" + result.FileDownloadName + ">";
                }
            }
            return "NONE";
        }
    }
}
