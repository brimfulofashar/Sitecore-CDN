using System;
using System.IO;
using System.Web;
using HtmlAgilityPack;
using Sitecore;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Feature.CDN.Pipelines
{
    public class StripContentProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (Context.Item == null
                || !Context.PageMode.IsNormal
                || Context.Site.Name != "website"
                || args.Rendering == null
                || args.Rendering.RenderingType == "Layout"
                || HttpContext.Current.Items["MarkedRenderingId"] == null)
                return;

            var markedPageRenderingId = HttpContext.Current.Items["MarkedRenderingId"] as Tuple<string, bool>;
            var renderingIsMarked = markedPageRenderingId != null && markedPageRenderingId.Item1 == args.Rendering.UniqueId.ToString();

            var existingHtmlString = args.Writer.ToString().Trim();
            if (!string.IsNullOrEmpty(existingHtmlString))
            {
                
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(existingHtmlString);
                htmlDoc.DocumentNode.FirstChild.Attributes.Add("data-rid", args.Rendering.UniqueId.ToString());

                string output;

                if (renderingIsMarked)
                {
                    if (!markedPageRenderingId.Item2)
                    {
                        output =
                            htmlDoc.DocumentNode.FirstChild.OuterHtml.Replace(htmlDoc.DocumentNode.FirstChild.InnerHtml,
                                "");
                        htmlDoc.LoadHtml(output);


                        htmlDoc.DocumentNode.FirstChild.Attributes.Add("data-rs", "0");
                    }
                    else
                    {
                        htmlDoc.DocumentNode.FirstChild.Attributes.Add("data-rs", "1");
                    }
                }

                output = htmlDoc.DocumentNode.FirstChild.OuterHtml;
                ((StringWriter)args.Writer).GetStringBuilder().Clear();
                args.Writer.Write(output);
            }
        }
    }
}