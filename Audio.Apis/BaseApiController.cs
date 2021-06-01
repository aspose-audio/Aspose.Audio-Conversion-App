using AudioVedio.Apis.Api.Models;
using AudioVedio.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace AudioVedio.Apis
{
    public class BaseApiController: ControllerBase
    {
        protected readonly ILogger logger;
        protected readonly IStorage storage;
        public BaseApiController(ILogger logger, IStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        protected string GetRequestSession()
        {
            var sessionId = Request.GetSessionId();
            if (sessionId != null)
            {
                return sessionId;
            }
            else
            {
                return "NEW-" + Guid.NewGuid().ToString();
            }
        }

        protected string GetPostData()
        {
            var sr = new StreamReader(Request.Body);
            var stream = sr.ReadToEnd();
            return stream;
        }

        protected void DisposeDocuments(List<InputDocument> documents)
        {
            foreach (var document in documents)
            {
                if (document.InputStream != null)
                {
                    using (document.InputStream) { 
                    }
                }
            }
        }

        protected void DisposeStream(Stream stream)
        {
            if (stream != null)
            {
                using (stream) {
                }
            }
        }
    }
}
