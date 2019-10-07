using System.Web;

namespace Feature.CDN
{
    public static class Extensions
    {
        public static bool IsPublicWebsite(this string siteName)
        {
            return siteName == "habitat";
        }

        public static bool IsContextRequestForDynamicData()
        {
            return HttpContext.Current.Request.QueryString["GetDynamicContent"] == "1";
        }
    }
}