using System.Web;
using Sitecore;
using Sitecore.Mvc.Analytics.Pipelines.Response.CustomizeRendering;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Feature.CDN.Pipelines
{
    public class AbortRenderingProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (Context.Item != null
                && Context.PageMode.IsNormal
                && Context.Site.Name.IsPublicWebsite()
                && args.Rendering.Item != null
                && HttpContext.Current.Items[$"RenderingIsCached"] != null)
            {
                var renderingIsCached = System.Convert.ToBoolean(HttpContext.Current.Items[$"RenderingIsCached"]);

                if (renderingIsCached && Extensions.IsContextRequestForDynamicData())
                {
                    args.AbortPipeline();
                }
            }
        }
    }
}