using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AudioVedio.Apis.Services
{
    public class LocalStorage : IStorage
    {
        public static string GetTempSrcFilePath(string fileName, string sessionId)
        {
            return Path.Combine(AppConfig.TempRootDirectory, StorageManager.ROOT_SOURCE, sessionId);
        }
        public static string GetTempExportFilePath(string fileName, string sessionId)
        {
            return Path.Combine(AppConfig.TempRootDirectory, StorageManager.ROOT_EXPORT, sessionId);
        }

        public string ReportRoot()
        {
            return Path.Combine(AppConfig.TempRootDirectory,StorageManager.ROOT_REPORT);
        }
        public string SourceRoot()
        {
            return Path.Combine(AppConfig.TempRootDirectory, StorageManager.ROOT_SOURCE);
        }

        public string ExportRoot()
        {
            return Path.Combine(AppConfig.TempRootDirectory, StorageManager.ROOT_EXPORT);
        }

        public async Task<Stream> DownloadFileAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                return DownloadFile(filePath);
            });
        }

        public async Task UploadFileListAsync(Dictionary<string, Stream> documents)
        {
            await Task.Run(() =>
            {
                UploadFileList(documents);
            });
        }

        public async Task<string> UploadFileAsync(string filePath, Stream fileStream)
        {
            return await Task.Run(() =>
            {
                return UploadFile(filePath, fileStream);
            });
        }

        private MemoryStream DownloadFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MemoryStream stream = new MemoryStream();
                fs.CopyTo(stream);
                stream.Position = 0;
                return stream;
            }
        }
        private string UploadFile(string filePath, Stream stream)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Position = 0;
                stream.CopyTo(fs);
            }
            return filePath;
        }

        private void UploadFileList(Dictionary<string,Stream> documents)
        {
            foreach (var document in documents)
            {
                UploadFile(document.Key, document.Value);
            }
        }

        public void CopyFolder(string sourceFolder, string targetFolder)
        {
    
            if (Directory.Exists(sourceFolder))
            {
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
                foreach (string file in Directory.GetFiles(sourceFolder))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    fileInfo.CopyTo(Path.Combine(targetFolder, fileInfo.Name));
                }
            }
            
        }

        public async Task CopyFolderAsync(string sourceFolder, string targetFolder)
        {
            await Task.Run(() =>
            {
                CopyFolder(sourceFolder, targetFolder);
            });
        }
    }
}