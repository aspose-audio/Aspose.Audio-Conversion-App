using System.Collections.Generic;

namespace AudioVedio.Pages.Models
{
    public class HowToModel
    {
        public string Title { get; set; }
        public List<HowToItem> List { get; set; }
    }

    public class HowToItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string UrlPath { get; set; }
    }
}