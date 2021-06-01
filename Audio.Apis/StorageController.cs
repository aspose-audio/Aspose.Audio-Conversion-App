using AudioVedio.Apis.Models.Response;
using AudioVedio.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;

namespace AudioVedio.Apis
{
    public class StorageController : BaseApiController
    {
        public StorageController(ILogger<StorageController> logger,IStorage storage) : base(logger, storage)
        {
        }

        public async Task<FileStreamResult> Download(string file, string folder)
        {
            var stream = await storage.DownloadFileAsync(StorageManager.GetExportPath(storage,file, folder));
            return DownloadFile(file, stream);
        }

        public async Task<UploadFileResponse> Upload()
        {
            var sessionId = GetRequestSession();
            var documents = Request.GetFiles();

            if (documents.Count() != 1)
            {
                throw new AppException("The file count must be 1");
            }
            var file = documents.Single();
            await storage.UploadFileAsync(StorageManager.GetSourcePath(storage,file.FileName, sessionId), file.InputStream);
            return new UploadFileResponse()
            {
                Code = 200,
                Data = new UploadResult(file.FileName, sessionId),
                SessionId = sessionId,
            };
        }

        private FileStreamResult DownloadFile(string file, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            MediaTypeHeaderValue header = new MediaTypeHeaderValue(new StringSegment("application/octet-stream"));
            var fileStream = new FileStreamResult(stream, header);
            fileStream.FileDownloadName = file;
            return fileStream;
        }

    }
}
