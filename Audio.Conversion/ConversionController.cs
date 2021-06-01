using AudioVedio.Pages.Services;
using Microsoft.AspNetCore.Mvc;

namespace Audio.Conversion
{
    public class ConversionController : Controller
    {
        public IActionResult Index()
        {
            var fileFormatInfo = PageUtils.GetFileFormatInfo(Request);
            var renderer = new AppRenderer();
            var model = renderer.GetAppModel(Startup.application, fileFormatInfo);
            ViewBag.AppName = model.AppName.ToRouteUrl();
            return View(model);
        }
    }
}
