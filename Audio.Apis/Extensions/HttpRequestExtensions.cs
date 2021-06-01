using AudioVedio.Apis.Api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AudioVedio.Apis
{
    public static class HttpRequestExtensions
    {
        public static readonly string SessionIdKey = "AppSessionId";

        internal static string GetSessionId(this HttpRequest Request)
        {
            if (Request.Headers.ContainsKey(SessionIdKey)) {
                return Request.Headers[SessionIdKey];
            }
            return null;
        }

        public static void SetSessionId(this HttpRequest Request,string sessionId)
        {
            Request.Headers[SessionIdKey] = sessionId;
        }

        internal static bool IsApiRequest(this HttpRequest Request) {
            if (Request.Path.HasValue) {
                if (Request.Path.Value.Contains("/api/")) {
                    return true;
                }
            }

            return false;
        }

        public static List<InputDocument> GetFiles(this HttpRequest request)
        {
            var files = request.Form.Files;
            List<InputDocument> requestFiles = new List<InputDocument>();
            foreach (var file in files)
            {
                requestFiles.Add(new InputDocument()
                {
                    FileName = file.FileName,
                    InputStream = file.OpenReadStream()
                });
            }

            return requestFiles;
        }
        
    }
}