namespace Symposium.Feature.CDN
{
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using Sitecore.Data.Items;
    using Sitecore.Mvc.Presentation;
    using Sitecore.StringExtensions;

    using Symposium.Feature.CDN.Extensions;

    public class CDNXmlBasedRenderingParser : XmlBasedRenderingParser
    {
        public override Rendering Parse(XElement node, bool parseChildNodes)
        {
            var rendering = base.Parse(node, parseChildNodes);

            RenderingItem renderingItem = rendering.RenderingItem;
            if (renderingItem == null)
            {
                return rendering;
            }

            // setting dynamic flag for rendering with 'IsAlwaysDynamicallyLoaded' checked
            rendering.SetDynamicallyLoadedFlag(
                renderingItem.InnerItem[Templates.CDNRendering.Fields.IsAlwaysDynamicallyLoaded].Equals("1"));

            // setting dynamic flag for rendering with personalization
            if (HasPersonalizationRules(rendering["PersonlizationRules"]))
            {
                rendering.SetDynamicallyLoadedFlag(true);
            }

            return rendering;
        }


        private static bool HasPersonalizationRules(string personalization)
        {
            if (personalization.IsNullOrEmpty())
            {
                return false;
            }

            return new Regex(Regex.Escape("</rule>")).Matches(personalization).Count > 1;
        }
    }
}