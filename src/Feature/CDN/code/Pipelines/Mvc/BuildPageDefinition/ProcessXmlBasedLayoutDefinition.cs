namespace Symposium.Feature.CDN.Pipelines.Mvc.BuildPageDefinition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Sitecore.Mvc.Extensions;
    using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
    using Sitecore.Mvc.Presentation;

    using Symposium.Feature.CDN.Extensions;
    using Symposium.Feature.CDN.Models;

    public class ProcessXmlBasedLayoutDefinition : Sitecore.Mvc.Pipelines.Response.BuildPageDefinition.ProcessXmlBasedLayoutDefinition
    {
        protected override IEnumerable<Rendering> GetRenderings(
          XElement layoutDefinition,
          BuildPageDefinitionArgs args)
        {
            // replaced parser with custom one
            XmlBasedRenderingParser parser = new CDNXmlBasedRenderingParser();
            foreach (XElement element1 in layoutDefinition.Elements("d"))
            {
                XElement deviceNode = element1;
                Guid deviceId = deviceNode.GetAttributeValueOrEmpty("id").ToGuid();

                // replace layout with CDN layout
                Guid layoutId = RequestExtensions.IsContextRequestForDynamicData()
                                    ? Guid.Parse(Constants.Layout.DefaultCDNLayoutId)
                                    : deviceNode.GetAttributeValueOrEmpty("l").ToGuid();

                IEnumerable<RenderingInfo> dynamicRenderingsInfo = RequestExtensions.GetDynamicRenderingsInfo();

                yield return this.GetRendering(deviceNode, deviceId, layoutId, "Layout", parser);
                foreach (XElement element2 in deviceNode.Elements("r"))
                {
                    // replacing placeholders to match CDN layout placeholder keys
                    var rendering = this.GetRendering(element2, deviceId, layoutId, element2.Name.LocalName, parser);

                    if (RequestExtensions.IsContextRequestForDynamicData())
                    {
                        var idMatchRendering = dynamicRenderingsInfo
                            .FirstOrDefault(r => r.RenderingId.ToGuid().Equals(rendering.UniqueId));

                        if (idMatchRendering != null)
                        {
                            rendering["Placeholder"] = Constants.Layout.DefaultCDNPlaceholder;
                        }
                        else
                        {
                            var placeholderMatchRendering = dynamicRenderingsInfo
                                .FirstOrDefault(
                                    r =>
                                        rendering.Placeholder.StartsWith(r.Placeholder)
                                        && !rendering.Placeholder.Equals(r.Placeholder));

                            if (placeholderMatchRendering != null)
                            {
                                rendering["Placeholder"] = rendering["Placeholder"]
                                    .Replace(
                                        placeholderMatchRendering.Placeholder.TrimStart('/'),
                                        Constants.Layout.DefaultCDNPlaceholder);
                            }
                        }
                    }

                    yield return rendering;
                }
            }
        }
    }
}