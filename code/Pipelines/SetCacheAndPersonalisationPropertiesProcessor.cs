using System.Web;
using Sitecore;
using Sitecore.Mvc.Analytics.Pipelines.Response.CustomizeRendering;

namespace Feature.CDN.Pipelines
{
    public class SetCacheAndPersonalisationPropertiesProcessor : CustomizeRenderingProcessor
    {
        public override void Process(CustomizeRenderingArgs args)
        {
            if (Context.Item != null
                && Context.PageMode.IsNormal
                && Context.Site.Name.IsPublicWebsite()
                && args.Rendering.Item != null)
            {
                HttpContext.Current.Items[$"RenderingIsPersonalised"] = args.IsCustomized;
                HttpContext.Current.Items[$"RenderingIsCached"] = args.Rendering.Item.Fields["Cacheable"] != null && args.Rendering.Item.Fields["Cacheable"].Value == "1";
            }
        }
    }
}