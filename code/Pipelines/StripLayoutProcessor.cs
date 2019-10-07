using Feature.CDN.Filters;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;

namespace Feature.CDN.Pipelines
{
    public class StripLayoutProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item != null
                && Context.PageMode.IsNormal
                && Context.Site.Name.IsPublicWebsite()
                && Extensions.IsContextRequestForDynamicData())
            {
                var filter = new RemoveStaticMarkupFilter(
                    args.HttpContext.Response.Filter,
                    args.HttpContext.Response.ContentEncoding);

                args.HttpContext.Response.Filter = filter;
            }
        }
    }
}