using Sitecore;

namespace Feature.CDN
{
    public static class Extensions
    {
        public static bool IsPublicWebsite(this string siteName)
        {
            return siteName == "habitat" || siteName == "website";
        }

        public static bool IsContextRequestForCustomization()
        {
            return Context.Request.QueryString["GetDynamicContent"] == "1";
        }
    }
}