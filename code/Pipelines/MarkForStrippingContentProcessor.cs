using System;
using System.Web;
using Sitecore;
using Sitecore.Mvc.Analytics.Pipelines.Response.CustomizeRendering;

namespace Feature.CDN.Pipelines
{
    public class MarkForStrippingContentProcessor : CustomizeRenderingProcessor
    {
        public override void Process(CustomizeRenderingArgs args)
        {
            if (Context.Item == null
                || !Context.PageMode.IsNormal
                || Context.Site.Name != "website"
                || string.IsNullOrEmpty(args.Rendering.RenderingItemPath))
                return;

            var renderingItem = Context.Data.Database.GetItem(args.Rendering.RenderingItemPath);
            if (renderingItem != null)
            {
                if (args.IsCustomized || (renderingItem.Fields["Is Dynamic Rendering"] != null && renderingItem.Fields["Is Dynamic Rendering"].Value == "1"))
                {
                    var getDynamicContent = Context.Request.QueryString["GetDynamicContent"] == "1";

                    HttpContext.Current.Items["MarkedRenderingId"] =
                        new Tuple<string, bool>(args.Rendering.UniqueId.ToString(), getDynamicContent);
                }
            }
        }
    }
}