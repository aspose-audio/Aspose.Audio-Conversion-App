using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AudioVedio.Apis.Api.Models
{
    public class InputDocument
    {
        public int FileType { get; set; } = 0;

        public string FileName { get; set; }

        public string FullPath { get; set; }

        public string FileUrl { get; set; }

        public Stream InputStream { get; set; }
    }
}