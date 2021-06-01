using AudioVedio.Apis.Models.Response;
using AudioVedio.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using System;
using Microsoft.Extensions.Logging;
using System.Web;

namespace AudioVedio.Apis
{
    public class CommonController : BaseApiController
    {
        public CommonController(ILogger<CommonController> logger, IStorage storage) : base(logger, storage)
        {

        }

        [HttpPost]
        public async Task<ReportResult> Error([FromForm] ReportModel model)
        {
            try
            {
                var report = await Task.Run(() => ReportService.Submit(model));
                return report;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToMessage(Request.GetSessionId()));
                return new ReportResult()
                {
                    StatusCode = 500,
                    Status = ex.Message
                };
            }
        }

        [HttpPost]
        public JsonResult SendFile(string url, string email)
        {
            EmailService.SendFile(url, email,logger);
            return new JsonResult(new { Message = "Your file(s) has been converted successfully, please check your email to download converted file(s)" });
        }


        [HttpPost]
        public JsonResult SendFeedback(string text, string appname, int score = 0)
        {
            if (text.Length > 1000)
            {
                throw new AppException("Feedback text message should be less than 1000 symbols");
            }

            var subject = $"aspose.app feedback {appname}";
            var body = score == 0
                ? text
                : $"Score: {score}{Environment.NewLine}{text}";

            EmailService.SendFeedback(subject, body, logger);

            return new JsonResult(new { Message = "Your feedback is sent successfully" });

        }
    }
}
