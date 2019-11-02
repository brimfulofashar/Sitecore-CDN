namespace Symposium.Feature.CDN.Extensions
{
    using Sitecore.Mvc.Presentation;

    public static class RenderingExtensions
    {
        private const string IsAlwaysDynamicallyPropertyName = "IsAlwaysDynamicallyLoaded";

        public static bool IsAlwaysDynamicallyLoaded(this Rendering rendering)
        {
            return rendering[IsAlwaysDynamicallyPropertyName] != null && rendering[IsAlwaysDynamicallyPropertyName].Equals("1");
        }

        public static void SetDynamicallyLoadedFlag(this Rendering rendering, bool isAlwaysDynamicallyLoaded)
        {
            rendering[IsAlwaysDynamicallyPropertyName] = isAlwaysDynamicallyLoaded ? "1" : "0";
        }
    }
}