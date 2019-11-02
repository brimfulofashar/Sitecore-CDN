namespace Symposium.Feature.CDN.Pipelines.Mvc.GetRenderer
{
    using Sitecore;
    using Sitecore.Data.Items;
    using Sitecore.Mvc.Extensions;
    using Sitecore.Mvc.Names;
    using Sitecore.Mvc.Pipelines.Response.GetRenderer;
    using Sitecore.Mvc.Presentation;

    using Symposium.Feature.CDN.Extensions;

    public class GetLoaderViewRenderer : GetRendererProcessor
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
            if (renderingItem != null && renderingItem.InnerItem.TemplateID == TemplateIds.ViewRendering)
            {
                string viewPath = renderingItem.InnerItem[Templates.CDNViewRendering.Fields.CDNDefaultPath];
                if (viewPath.IsWhiteSpaceOrNull())
                {
                    return new ViewRenderer
                    {
                        ViewPath = "/Views/CDN/DefaultLoader.cshtml",
                        Rendering = rendering
                    };
                }

                return new ViewRenderer
                {
                    ViewPath = viewPath,
                    Rendering = rendering
                };
            }

            return null;
        }
    }
}