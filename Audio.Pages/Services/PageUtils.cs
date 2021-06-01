using AudioVedio.Pages.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AudioVedio.Pages.Services
{
    public static class PageUtils
    {
        private static readonly Dictionary<string, string[]> SupportedFormats = new Dictionary<string, string[]> {
            {"vsd", new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm","pptx","docx"}},
            {"vsdx",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm","pptx","docx"}},
            {"vsx", new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm","pptx","docx"}},
            {"vtx", new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vdx","vssx","vstx","vsdm","vssm","vstm","pptx","docx"}},
            {"vdx", new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vssx","vstx","vsdm","vssm","vstm","pptx","docx"}},
            {"vssx",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vstx","vsdm","vssm","vstm","pptx","docx"}},
            {"vstx",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vsdm","vssm","vstm","pptx","docx"}},
            {"vsdm",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vstx","vssm","vstm","pptx","docx"}},
            {"vssm",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vstx","vsdm","vstm","pptx","docx"}},
            {"vstm",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","pptx","docx"}},
            {"visio",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","pptx","docx"}},
            {"diagram",new[] {"pdf","html", "jpg", "png", "svg", "bmp", "tiff", "xps","gif","emf","vsdx","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","pptx","docx"}},
        };

        private static readonly Dictionary<string, string[]> SupportedMergerFormats = new Dictionary<string, string[]>
        {
            {"vsd", new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vsdx",new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vsx", new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vtx", new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vdx", new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vssx",new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vstx",new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vsdm",new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vssm",new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"vstm",new[]   { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"visio",new[]  { "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
            {"diagram",new[]{ "vsdx","pdf","docx","pptx","xps","vsx","vtx","vdx","vssx","vstx","vsdm","vssm","vstm"}},
        };

        private static Dictionary<string, string> SuffixMap = new Dictionary<string, string>() {
            { "image","jpg"},
            { "word","docx"},
            { "doc","docx"},
            { "ppt","pptx"},
            { "visio","vsdx"},
        };

        public static FileFormatInfo GetFileFormatInfo(HttpRequest request)
        {
            FileFormatInfo fileFormatInfo = null;
            var routeValues = request.RouteValues;
            if (routeValues.ContainsKey("fileformat") && !string.IsNullOrEmpty(routeValues["fileformat"].ToString()))
            {
                fileFormatInfo = new FileFormatInfo();
                fileFormatInfo.PathAndQuery = request.QueryString.ToString().ToLower();
                var fileformat = routeValues["fileformat"].ToString();
                if (fileformat.Contains("-"))
                {
                    var values = fileformat.Split('-');
                    fileFormatInfo.SourceExtension = values.First().ToLower();
                    fileFormatInfo.DestinationExtension = values.Last().ToLower();
                }
                else
                {
                    fileFormatInfo.SourceExtension = fileformat.ToLower();
                }

                if (fileFormatInfo.SourceExtension != null && SuffixMap.ContainsKey(fileFormatInfo.SourceExtension))
                {
                    fileFormatInfo.RealSourceExtension = SuffixMap[fileFormatInfo.SourceExtension];
                }
                else
                {
                    fileFormatInfo.RealSourceExtension = fileFormatInfo.SourceExtension;
                }

                if (fileFormatInfo.DestinationExtension != null && SuffixMap.ContainsKey(fileFormatInfo.DestinationExtension))
                {
                    fileFormatInfo.RealDestinationExtension = SuffixMap[fileFormatInfo.DestinationExtension];
                }
                else
                {
                    fileFormatInfo.RealDestinationExtension = fileFormatInfo.DestinationExtension;
                }

            }


            return fileFormatInfo;
        }

        public static bool isSupportedMergerExtensions(string extFrom, string extTo)
        {
            if (string.IsNullOrEmpty(extFrom))
            {
                return true;
            }

            var _extTo = string.IsNullOrEmpty(extTo) ? "" : extTo.ToLower();

            var isValExist = SupportedMergerFormats.TryGetValue(extFrom.ToLower(), out var exToArray);
            if (!isValExist)
                return false;
            if (isValExist && string.IsNullOrEmpty(_extTo))
                return true;

            return exToArray.Contains(_extTo);
        }

        public static string ToRouteUrl(this string url) {
            return url.ToLower();
        }
    }
}
