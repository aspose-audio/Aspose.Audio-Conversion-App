using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis.Models.Response
{
    public class DocumentUploadResponse : BaseResponse
    {
        public DocumentUploadModel Data { get; set; } = new DocumentUploadModel();
        public DocumentUploadResponse(string fileRequestId)
        {
            Data.FileRequestId = fileRequestId;
        }


        public class DocumentUploadModel
        {
            public string FileRequestId { get; set; }
        }
    }
}
