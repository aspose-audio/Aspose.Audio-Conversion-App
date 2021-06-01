using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AudioVedio.Apis.Models.Response;

namespace AudioVedio.Apis.Services
{
    public class AmazonS3CloudStorage : IStorage
    {
        public string ReportRoot()
        {
            return StorageManager.ROOT_REPORT;
        }
        public string ExportRoot()
        {
            return StorageManager.ROOT_EXPORT;
        }

        public string SourceRoot()
        {
            return StorageManager.ROOT_SOURCE;
        }
      

        public async Task<Stream> DownloadFileAsync(string filePath)
        {
            var md5Stream = await AmazonS3StorageUtils.Download(filePath);
            using (md5Stream) {
                MemoryStream memory = new MemoryStream();
                await md5Stream.CopyToAsync(memory);
                memory.Position = 0;
                return memory;
            }
        }

        public async Task<string> UploadFileAsync(string filePath, Stream fileStream)
        {
            await AmazonS3StorageUtils.Upload(filePath, fileStream);
            return AmazonS3StorageUtils.GetPreSignedURL(filePath);
        }

        public async Task UploadFileListAsync(Dictionary<string, Stream> documents)
        {
            await AmazonS3StorageUtils.UploadList(documents);
        }

        public async Task CopyFolderAsync(string sourceFolder, string targetFolder)
        {
            await AmazonS3StorageUtils.CopyFolder(sourceFolder, targetFolder);
        }


    }
}