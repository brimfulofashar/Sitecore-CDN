using System.Web;
using Sitecore;
using Sitecore.Mvc.Analytics.Pipelines.Response.CustomizeRendering;

namespace Feature.CDN.Pipelines
{
    public class MarkForStrippingContentProcessor : CustomizeRenderingProcessor
    {
        public override void Process(CustomizeRenderingArgs args)
        {
            if (Context.Item != null
                && Context.PageMode.IsNormal
                && Context.Site.Name.IsPublicWebsite()
                && args.Rendering.Item != null)
            {
                // todo: clarify with Ash?
                //if (args.IsCustomized || (args.Rendering.Item.Fields["Cacheable"] != null && args.Rendering.Item.Fields["Cacheable"].Value != "1"))
                if (args.IsCustomized)
                {
                    // todo: improve
                    HttpContext.Current.Items[$"MarkedRenderingId{args.Rendering.UniqueId}"] = args.Rendering.UniqueId;
                }
            }
        }
    }
}