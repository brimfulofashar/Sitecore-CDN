using Sitecore.Mvc.Pipelines.MvcEvents.ActionExecuted;
using System;

namespace Feature.CDN.Pipelines
{
    public class CacheControl : ActionExecutedProcessor
    {
        public override void Process(ActionExecutedArgs args)
        {
            if (Extensions.IsContextRequestForDynamicData())
            {
                args.Context.RequestContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            }
            else
            {
                args.Context.RequestContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                args.Context.RequestContext.HttpContext.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(1));
            }
        }
    }
}