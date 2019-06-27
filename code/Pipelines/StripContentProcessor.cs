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
        private const string SpinnerHtml = "<div class='lds-spinner'><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>";

        public override void Process(RenderRenderingArgs args)
        {
            if (Context.Item != null
                && Context.PageMode.IsNormal
                && Context.Site.Name.IsPublicWebsite()
                && args.Rendering != null
                && args.Rendering.RenderingType != "Layout"
                && HttpContext.Current.Items[$"MarkedRenderingId{args.Rendering.UniqueId}"] != null)
            {
                var existingHtmlString = args.Writer.ToString().Trim();
                if (!string.IsNullOrEmpty(existingHtmlString))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(existingHtmlString);
                    htmlDoc.DocumentNode.FirstChild.Attributes.Add("data-rid", args.Rendering.UniqueId.ToString());

                    string output;

                    if (!Extensions.IsContextRequestForCustomization())
                    {
                        output = htmlDoc.DocumentNode.FirstChild.OuterHtml.Replace(
                            htmlDoc.DocumentNode.FirstChild.InnerHtml, SpinnerHtml);
                        htmlDoc.LoadHtml(output);

                        htmlDoc.DocumentNode.FirstChild.Attributes.Add("data-rs", "0");
                    }
                    else
                    {
                        htmlDoc.DocumentNode.FirstChild.Attributes.Add("data-rs", "1");
                    }

                    output = htmlDoc.DocumentNode.FirstChild.OuterHtml;
                    ((StringWriter)args.Writer).GetStringBuilder().Clear();
                    args.Writer.Write(output);
                }
            }
        }
    }
}