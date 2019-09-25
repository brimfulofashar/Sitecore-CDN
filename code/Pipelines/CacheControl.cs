using Sitecore.Mvc.Pipelines.MvcEvents.ActionExecuted;

namespace Feature.CDN.Pipelines
{
    public class CacheControl : ActionExecutedProcessor
    {
        public override void Process(ActionExecutedArgs args)
        {
            args.Context.RequestContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
        }
    }
}