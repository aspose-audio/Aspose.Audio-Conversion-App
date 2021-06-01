
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AudioVedio.Apis.Models.Response
{
    public class DocumentActionResponse : BaseResponse
    {
        public DocumentActionModel Data { get; set; } = new DocumentActionModel();

        public DocumentActionResponse()
        {

        }
        public DocumentActionResponse(int status)
        {
            this.Data.Status = status;
        }
        public DocumentActionResponse(string fileName, string folderName,string downloadLink)
        {
            this.Data.FileName = fileName;
            this.Data.FolderName = folderName;
            this.Data.DownloadLink = downloadLink;
        }
    }

    public class DocumentActionModel {
        public static int STATUS_OK = 0;
        public static int STATUS_ERROR = 1;
        public static int STATUS_NO = 2;

        public int Status { get; set; } = STATUS_OK;
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public string DownloadLink { get; set; }
    }

}