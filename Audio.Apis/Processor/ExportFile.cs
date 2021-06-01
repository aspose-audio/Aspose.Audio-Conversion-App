using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVedio.Apis.Processor
{
    public class ExportFile
    {
        public string FileName { get; set; }

        public string FullPath { get; set; }

        public Stream FileStream { get; set; }
    }
}
