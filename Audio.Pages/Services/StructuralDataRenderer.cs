using AudioVedio.Pages.Models;
using AudioVedio.Pages.Models.SEO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AudioVedio.Pages.Services
{
    public class StructuralDataRenderer
    {
        public static List<string> GetJson(AppModel appModel,FileFormatInfo fileFormatInfo) {
            List<string> list = new List<string>();
            try
            {
                list.Add(PrepareJsonLdBreadcrumbList(appModel, fileFormatInfo));
                list.Add(PrepareJsonLdSoftware(appModel));
                if(appModel.HowToModel != null)
                {
                    list.Add(PrepareJsonLdHowTo(appModel));
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;

        }

        private static string PrepareJsonLdBreadcrumbList(AppModel appModel, FileFormatInfo fileFormatInfo)
        {
            List<(string Name, string Href)> list = new List<(string Name, string Href)>();
            list.Add(("Aspose Product Family", "https://products.aspose.app/"));
            list.Add(("diagram", "https://products.aspose.app/diagram/family"));
            list.Add(($"{appModel.AppRoute}", "https://products.aspose.app/diagram/" + appModel.AppRoute));

            if (fileFormatInfo!=null&&(!string.IsNullOrEmpty(fileFormatInfo.SourceExtension)))
            {
                var add1 = fileFormatInfo.SourceExtension == null ? "" : fileFormatInfo.SourceExtension.ToUpper();
                var add2 = fileFormatInfo.DestinationExtension == null ? "" : fileFormatInfo.DestinationExtension.ToUpper();

                string name;
                switch (appModel.App)
                {
                    case AudioVedioApps.Conversion:
                        name = string.Format(AppXmlResource.GetResource($"{appModel.AppName}Breadcrumb"), add1, add2);
                        break;
  
                    default:
                        name = "Document";
                        break;
                }
                list.Add((name, "https://products.aspose.app" + fileFormatInfo.PathAndQuery));
            }
            return Breadcrumb.GenerateJson(list.ToArray());
        }

        private static string PrepareJsonLdSoftware(AppModel appModel)
        {
            SoftwareApplication obj = new SoftwareApplication()
            {
                Name = appModel.Title,
                Description = appModel.PageMetaDescription,
                SoftwareRequirements = new URL() { Id = "https://docs.aspose.com/display/diagramnet/System+Requirements" },
                SoftwareHelp = "https://docs.aspose.com/display/diagramnet/Home"
            };
            return JsonConvert.SerializeObject(obj);
        }

        private static string PrepareJsonLdHowTo(AppModel appModel)
        {
            HowTo obj = new HowTo(appModel.HowToModel, appModel.TitleSub, appModel.Title);
            return JsonConvert.SerializeObject(obj);
        }
    }
}