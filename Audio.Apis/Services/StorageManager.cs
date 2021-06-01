using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AudioVedio.Apis.Services
{
    public class StorageManager
    {
        public static readonly string ROOT_SOURCE = "source";
        public static readonly string ROOT_EXPORT = "export";
        public static readonly string ROOT_REPORT = "report";

        public static string GetSourcePath(IStorage storage,string sessionId)
        {
            return Path.Combine(storage.SourceRoot(), sessionId);
        }

        public static string GetSourcePath(IStorage storage, string fileName,string sessionId)
        {
            return Path.Combine(storage.SourceRoot(), sessionId, fileName);
        }

        public static string GetExportPath(IStorage storage, string fileName,string sessionId)
        {
            return Path.Combine(storage.ExportRoot(), sessionId, "documents", Path.GetFileNameWithoutExtension(fileName), fileName);
        }

        public static string GetReportPath(IStorage storage, string sessionId)
        {
            return Path.Combine(storage.ReportRoot(), sessionId);
        }
    }
}