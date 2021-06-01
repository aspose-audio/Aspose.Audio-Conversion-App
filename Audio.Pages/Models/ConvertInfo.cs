namespace AudioVedio.Pages.Models
{
    public class FileFormatInfo
    {
        public string SourceExtension { get; set; }
        public string DestinationExtension { get; set; }

        public string RealSourceExtension { get; set; }
        public string RealDestinationExtension { get; set; }

        public string PathAndQuery { get; set; }
    }
}