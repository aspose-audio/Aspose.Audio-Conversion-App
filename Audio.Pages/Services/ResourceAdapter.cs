namespace AudioVedio.Pages.Services
{
    public class ResourceAdapter
    {
        public string this[string key]
        {
            get
            {
                return AppXmlResource.GetResource(key);
            }
        }

    }
}