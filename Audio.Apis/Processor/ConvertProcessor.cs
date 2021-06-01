using AudioVedio.Apis.Api.Models;
using AudioVedio.Apis.Services;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using FFmpeg.NET.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioVedio.Apis.Processor
{
    public class ConvertProcessor
    {
        public static async Task<ExportFile> Convert(ConversionOptions options, List<InputDocument> documents, string sessionId)
        {
            try
            {
                if (documents == null || documents.Count() == 0 || documents.Count > 1)
                {
                    throw new AppException("The number of documents must be equal 1");
                }

                AudioFormat audioFormat = options.AudioFormat;
                if(audioFormat == AudioFormat.Default)
                {
                    throw new AppException("Please specify a audio format.");
                }
                InputDocument inputDocument = documents[0];
                var inputFile = inputDocument.FullPath;
                var outputFileName = Path.GetFileNameWithoutExtension(inputFile) + "." + audioFormat.ToString();
                var outputFileDirect = LocalStorage.GetTempExportFilePath(outputFileName, sessionId);
                if(!Directory.Exists(outputFileDirect))
                {
                    Directory.CreateDirectory(outputFileDirect);
                }
                var outputFile = Path.Combine(outputFileDirect, outputFileName);
                var ffmpeg = new Engine(AppConfig.FFmpegPath);
                ffmpeg.Progress += OnProgress;
                ffmpeg.Data += OnData;
                ffmpeg.Error += OnError;
                ffmpeg.Complete += OnComplete;
                var output = await ffmpeg.ConvertAsync(new MediaFile(inputFile), new MediaFile(outputFile), options);
                return new ExportFile() { FileName = outputFileName, FullPath = outputFile, FileStream = File.OpenRead(outputFile) };
            }
            catch (Exception e) {
                throw e;
            }
        }

        private static void OnProgress(object sender, ConversionProgressEventArgs e)
        {
            Console.WriteLine("[{0} => {1}]", e.Input.FileInfo.Name, e.Output?.FileInfo.Name);
            Console.WriteLine("Bitrate: {0}", e.Bitrate);
            Console.WriteLine("Fps: {0}", e.Fps);
            Console.WriteLine("Frame: {0}", e.Frame);
            Console.WriteLine("ProcessedDuration: {0}", e.ProcessedDuration);
            Console.WriteLine("Size: {0} kb", e.SizeKb);
            Console.WriteLine("TotalDuration: {0}\n", e.TotalDuration);
        }

        private static void OnData(object sender, ConversionDataEventArgs e)
            => Console.WriteLine("[{0} => {1}]: {2}", e.Input.FileInfo.Name, e.Output?.FileInfo.Name, e.Data);

        private static void OnComplete(object sender, ConversionCompleteEventArgs e)
            => Console.WriteLine("Completed conversion from {0} to {1}", e.Input.FileInfo.FullName, e.Output?.FileInfo.FullName);

        private static void OnError(object sender, ConversionErrorEventArgs e)
            => Console.WriteLine("[{0} => {1}]: Error: {2}\n{3}", e.Input.FileInfo.Name, e.Output?.FileInfo.Name, e.Exception.ExitCode, e.Exception.InnerException);
    }
}
