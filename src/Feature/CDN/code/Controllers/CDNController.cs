namespace Symposium.Feature.CDN.Controllers
{
    using System.Web.Mvc;

    using Sitecore.Mvc.Controllers;

    public class CDNController : SitecoreController
    {
        public ActionResult DefaultLoader()
        {
            return this.View("DefaultLoader");
        }
    }
}