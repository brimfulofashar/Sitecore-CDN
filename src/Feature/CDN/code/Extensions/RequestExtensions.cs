namespace Symposium.Feature.CDN.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Sitecore.StringExtensions;

    using Symposium.Feature.CDN.Models;

    public static class RequestExtensions
    {
        public static bool IsContextRequestForDynamicData()
        {
            return HttpContext.Current.Request.QueryString["GetDynamicContent"] == "1";
        }

        public static IEnumerable<RenderingInfo> GetDynamicRenderingsInfo()
        {
            var dynamicRenderingsInfoHeader = HttpContext.Current.Request.Headers["DynamicRenderingIds"];
            if (!dynamicRenderingsInfoHeader.IsNullOrEmpty())
            {
                return dynamicRenderingsInfoHeader.Split('|')
                    .Select(dynamicRenderingInfo => dynamicRenderingInfo.Split(':'))
                    .Select(idPlaceholder => new RenderingInfo
                    {
                        RenderingId = idPlaceholder[0],
                        Placeholder = idPlaceholder[1]
                    });
            }

            return Enumerable.Empty<RenderingInfo>();
        }
    }
}