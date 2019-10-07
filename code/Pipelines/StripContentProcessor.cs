using System;
using System.IO;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Feature.CDN.Pipelines
{
    public class StripContentProcessor : RenderRenderingProcessor
    {
        private const string SpinnerHtml = "<div class=\"loader\"></div>";

        public override void Process(RenderRenderingArgs args)
        {
            if (Context.Item != null
                && Context.PageMode.IsNormal
                && Context.Site.Name.IsPublicWebsite()
                && args.Rendering != null
                && args.Rendering.RenderingType != "Layout"
                && HttpContext.Current.Items[$"RenderingIsPersonalised"] != null)
            {

                var existingHtmlString = args.Writer.ToString().Trim();
                if (!string.IsNullOrEmpty(existingHtmlString))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(existingHtmlString);
                    {
                        var renderingIsPersonalised = System.Convert.ToBoolean(HttpContext.Current.Items[$"RenderingIsPersonalised"]);
                        HtmlNode rootNode = htmlDoc.DocumentNode.FirstChild;

                        rootNode.Attributes.Add("data-renderingId", args.Rendering.UniqueId.ToString());
                        rootNode.Attributes.Add("data-renderingIsPersonalised", renderingIsPersonalised ? "1" : "0");

                        if (!Extensions.IsContextRequestForDynamicData())
                        {
                            if (HttpContext.Current.Items["RenderingPlaceholderKey"] != null && HttpContext.Current.Items["RenderingPlaceholderKey"].ToString().Contains(args.Rendering.Placeholder))
                            {
                                ((StringWriter) args.Writer).GetStringBuilder().Clear();
                                args.Writer.Write(existingHtmlString);

                                return;
                            }

                            if (renderingIsPersonalised)
                            {
                                rootNode.InnerHtml = SpinnerHtml;
                            }

                            HttpContext.Current.Items["RenderingPlaceholderKey"] += args.Rendering.Placeholder + "|";


                            ((StringWriter) args.Writer).GetStringBuilder().Clear();
                            args.Writer.Write(rootNode.OuterHtml);
                        }
                        else if (Extensions.IsContextRequestForDynamicData() && (HttpContext.Current.Request.Headers["DynamicRenderingIds"] != null && HttpContext.Current.Request.Headers["DynamicRenderingIds"].Contains(args.Rendering.UniqueId.ToString())))
                        {
                            ((StringWriter)args.Writer).GetStringBuilder().Clear();
                            args.Writer.Write(rootNode.OuterHtml);
                        }
                        else
                        {
                            args.AbortPipeline();
                        }
                    }
                }
            }
        }
    }
}