using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVedio.Apis.Processor
{
    public class ZipProcessor
    {
        public static ExportFile Save(ProcessedDocuments document)
        {
            return Save(new List<ProcessedDocuments> { document });
        }

        public static ExportFile Save(List<ProcessedDocuments> files)
        {
            var zipName = "documents.zip";
            if (files.Count == 1)
            {
                if (files.First().Documents.Count == 1)
                {
                    var doc = files.First().Documents.First();
                    return new ExportFile() { FileName = doc.Key, FileStream = doc.Value };
                }
                zipName = files.First().FolderName + ".zip";
            }
            var ms = new MemoryStream();
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Update, true))
            {
                foreach (var file in files)
                {
                    //var documentEntry = archive.CreateEntry(file.FolderName);
                    foreach (var item in file.Documents)
                    {
                        var itemEntry = archive.CreateEntry(item.Key);
                        using (var itemStream = itemEntry.Open())
                        {
                            using (item.Value)
                            {
                                item.Value.Position = 0;
                                item.Value.CopyTo(itemStream);
                            }
                        }
                    }
                }
            }

            ms.Position = 0;

            return new ExportFile()
            {
                FileStream = ms,
                FileName = zipName,
            };
        }
    }
}
