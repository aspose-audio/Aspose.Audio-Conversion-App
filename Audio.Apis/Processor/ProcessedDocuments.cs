using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVedio.Apis.Processor
{
    public class ProcessedDocuments
    {
        public string FolderName { get; set; }

        public Dictionary<string, Stream> Documents { get; set; }
    }
}
