using System.Collections.Generic;

namespace AudioVedio.Pages.Models
{
    public class OtherAppsModel
    {
        public string OtherAppsTitle { get; set; }
        public IEnumerable<AnotherApp> OtherApps { get; set; }
    }
}