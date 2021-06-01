
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioVedio.Apis.Models.Response
{
    public class UploadFileResponse : BaseResponse
    {
        public UploadResult Data { get; set; }
    }

    public class UploadResult
    {
        public UploadResult(string fileName, string folderName)
        {
            this.FileName = fileName;
            this.FolderName = folderName;
        }

        public string FileName { get; set; }
        public string FolderName { get; set; }
    }
}