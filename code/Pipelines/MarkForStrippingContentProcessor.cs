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
                || Context.Site.Name != "website")
                return;

            if (args.IsCustomized)
            {
                var getDynamicContent = Context.Request.QueryString["GetDynamicContent"] == "1";

                HttpContext.Current.Items["MarkedRenderingId"] = new Tuple<string, bool>(args.Rendering.UniqueId.ToString(), getDynamicContent);
            }
        }
    }
}