namespace Symposium.Feature.CDN.Pipelines.Mvc.RenderRendering
{
    using Sitecore;
    using Sitecore.Mvc.Pipelines.Response.RenderRendering;

    using Symposium.Feature.CDN.Extensions;

    public class AddClosingRenderingIdTag : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (RequestExtensions.IsContextRequestForDynamicData()
                && Context.PageMode.IsNormal
                && args.Rendering != null
                && args.Rendering.RenderingType != "Layout"
                && args.Rendering.Placeholder.Equals(CDN.Constants.Layout.DefaultCDNPlaceholder))
            {
                args.Writer.Write("</div>");
            }
        }
    }
}