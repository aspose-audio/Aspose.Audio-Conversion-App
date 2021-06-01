namespace AudioVedio.Pages.Services
{
    public class PageConfig
    {
        public static string PageUrl;
        public static string ApiUrl;
        static PageConfig() {
            PageUrl = "/audio/";
            ApiUrl = "/audio/{0}/api/";
        }
    }
}
