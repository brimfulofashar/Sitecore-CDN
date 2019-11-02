namespace Symposium.Feature.CDN.Pipelines.Mvc.GetRenderer
{
    using Sitecore;
    using Sitecore.Data.Items;
    using Sitecore.Mvc.Extensions;
    using Sitecore.Mvc.Names;
    using Sitecore.Mvc.Pipelines.Response.GetRenderer;
    using Sitecore.Mvc.Presentation;

    using Symposium.Feature.CDN.Extensions;

    public class GetLoaderControllerRenderer : GetRendererProcessor
    {
        public override void Process(GetRendererArgs args)
        {
            if (args.Result != null)
            {
                return;
            }

            if (Context.PageMode.IsNormal
                && !RequestExtensions.IsContextRequestForDynamicData()
                && args.Rendering.IsAlwaysDynamicallyLoaded())
            {
                args.Result = this.GetRenderer(args.Rendering, args);
            }
        }

        private Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
        {
            RenderingItem renderingItem = rendering.RenderingItem;
            if (renderingItem != null && renderingItem.InnerItem.TemplateID == TemplateIds.ControllerRendering)
            {
                string controller = renderingItem.InnerItem[Templates.CDNControllerRendering.Fields.CDNDefaultController];
                string action = renderingItem.InnerItem[Templates.CDNControllerRendering.Fields.CDNDefaultControllerAction];
                if (controller.IsWhiteSpaceOrNull() || action.IsWhiteSpaceOrNull())
                {
                    return new ControllerRenderer
                    {
                        ControllerName = "CDN",
                        ActionName = "DefaultLoader"
                    };
                }

                return new ControllerRenderer
                {
                    ControllerName = controller,
                    ActionName = action
                };
            }

            return null;
        }
    }
}