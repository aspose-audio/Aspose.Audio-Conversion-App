using AudioVedio.Apis;
using AudioVedio.Apis.Api.Models;
using AudioVedio.Apis.Models.App;
using AudioVedio.Apis.Models.Response;
using AudioVedio.Apis.Processor;
using AudioVedio.Apis.Services;
using FFmpeg.NET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Audio.Conversion
{
    [Route("audio/conversion/api/conversion")]
    [ApiController]
    public class ConversionApiController : BaseApiController
    {
        public ConversionApiController(ILogger<ConversionApiController> logger, IStorage storage) : base(logger, storage)
        {
        }

        [HttpPost]
        public DocumentUploadResponse Index()
        {
            var sessionId = GetRequestSession();

            List<InputDocument> documents = null;
            var fileType = Request.Form["fileType"].First();
            if (fileType.Equals("0"))
            {
                documents = Request.GetFiles();
            }
            else
            {
                var fileUrl = Request.Form["fileUrl"].First();
                documents = new List<InputDocument>();
                documents.Add(new InputDocument()
                {
                    FileType = 1,
                    FileUrl = fileUrl,
                    FileName = ConstantsService.GenerateUrlFilename()
                }); ;
            }

            if (documents == null || documents.Count() == 0 || documents.Count > 1)
            {
                throw new AppException("The number of documents must be equal 1");
            }

            var convertOptionStr = Request.Form["convertOption"].First();

            ConversionOptions options = null;
            if (convertOptionStr != null)
            {
                ConvertParam optionPara = JsonConvert.DeserializeObject<ConvertParam>(convertOptionStr);
                options = optionPara.ToConversionOptions();
            }

            if(options == null)
            {
                throw new Exception("Invalid audio setting");
            }


            //await storage.UploadFileListAsync(documents.ToDictionary(x => StorageManager.GetSourcePath(storage, x.FileName, sessionId), x => x.InputStream));

            foreach(InputDocument document in documents)
            {
                string fileDirect = LocalStorage.GetTempSrcFilePath(document.FileName, sessionId);
                if (!Directory.Exists(fileDirect))
                {
                    Directory.CreateDirectory(fileDirect);
                }
                string filePath = Path.Combine(fileDirect, document.FileName);
                document.FullPath = filePath;

                if (document.FileType == 1)
                {
                    try
                    {
                        DownloadUrl(document.FileUrl, filePath);
                    }
                    catch(Exception)
                    {
                        throw new Exception("Can't download this url.");
                    }
                }
                else
                {
                    document.InputStream.Seek(0, SeekOrigin.Begin);
                    using (FileStream fsWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        document.InputStream.CopyTo(fsWrite);
                    }
                }
            }
            string requestId = sessionId + documents[0].FileName + Guid.NewGuid().ToString();
            _ = Task.Factory.StartNew(async () =>
              {
                  var response = await HandleConversion(options, documents, sessionId);
                  Cache.Insert<DocumentActionResponse>(requestId, response, 60 * 60 * 24);
                  Console.WriteLine("Complete send result to redis:" + requestId);
              });

            return new DocumentUploadResponse(requestId);
        }

        private void DownloadUrl(string url, string filePath)
        {
            var _webRequest = (HttpWebRequest)WebRequest.Create(url);
            _webRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)_webRequest.GetResponse();
            Stream dataStream = response.GetResponseStream();
            FileStream fstr = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            CopyStream(dataStream, fstr);
            response.Close();
            fstr.Close();
        }
        internal static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0) break;
                output.Write(buffer, 0, read);
            }
        }

        private async Task<DocumentActionResponse> HandleConversion(ConversionOptions options, List<InputDocument> documents, string sessionId)
        {
            DocumentActionResponse response = null;
            try
            {
                var exportFile = await ConvertProcessor.Convert(options, documents, sessionId);
                var downloadLink = await storage.UploadFileAsync(StorageManager.GetExportPath(storage, exportFile.FileName, sessionId), exportFile.FileStream);
                Console.WriteLine("Complete upload file to S3:" + downloadLink);
                DisposeDocuments(documents);
                DisposeStream(exportFile.FileStream);
                if (System.IO.File.Exists(exportFile.FullPath))
                {
                    System.IO.File.Delete(exportFile.FullPath);
                    string directoryName = Path.GetDirectoryName(exportFile.FullPath);
                    Directory.Delete(directoryName);
                }

                foreach(InputDocument document in documents)
                {
                    if (System.IO.File.Exists(document.FullPath))
                    {
                        System.IO.File.Delete(document.FullPath);
                        string directoryName = Path.GetDirectoryName(document.FullPath);
                        Directory.Delete(directoryName);
                    }

                }

                response = new DocumentActionResponse(exportFile.FileName, sessionId, downloadLink);
                return response;
            }
            catch(Exception)
            {
                response = new DocumentActionResponse(DocumentActionModel.STATUS_ERROR);
                return response;
            }

        }

        [HttpGet]
        [Route("HandleStatus")]
        public DocumentActionResponse HandleStatus(string fileRequestId)
        {
            DocumentActionResponse response = null;
            try
            {
                response = Cache.Get<DocumentActionResponse>(fileRequestId);
            }
            catch (Exception ex)
            {
                response = new DocumentActionResponse(DocumentActionModel.STATUS_NO);
                return response;
            }
            if(response == null)
            {
                response = new DocumentActionResponse(DocumentActionModel.STATUS_NO);
            }
            return response;
        }


        [HttpGet]
        public string Get()
        {
            return "Api Started!";
        }
    }
}
