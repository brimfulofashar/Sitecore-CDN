namespace Symposium.Feature.CDN.Pipelines.Mvc.ActionExecuted
{
    using System;

    using Sitecore.Mvc.Pipelines.MvcEvents.ActionExecuted;

    using Symposium.Feature.CDN.Extensions;

    public class CDNCacheControl : ActionExecutedProcessor
    {
        public override void Process(ActionExecutedArgs args)
        {
            if (RequestExtensions.IsContextRequestForDynamicData())
            {
                // no cache for dynamic data
                args.Context.RequestContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            }
            else
            {
                // all pages are cached for 1 minute
                args.Context.RequestContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                args.Context.RequestContext.HttpContext.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(1));
            }
        }
    }
}