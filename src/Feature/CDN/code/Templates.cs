namespace Symposium.Feature.CDN
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct CDNRendering
        {
            public static readonly ID ID = new ID("{D9CE8FCB-DF32-48EA-A5F1-6E4A825B835E}");

            public struct Fields
            {
                public static readonly ID IsAlwaysDynamicallyLoaded = new ID("{EC89BFE3-57E5-4390-9240-23BFD8510EFD}");
            }
        }

        public struct CDNControllerRendering
        {
            public static readonly ID ID = new ID("{FEB8B77E-4919-4B64-BA97-EFE44AC26199}");

            public struct Fields
            {
                public static readonly ID CDNDefaultController = new ID("{F147A2EB-76E4-453E-932D-19813AB6F7FD}");
                public static readonly ID CDNDefaultControllerAction = new ID("{432F13C8-93DD-4F34-89DF-D8F4C11DEAA4}");
            }
        }

        public struct CDNViewRendering
        {
            public static readonly ID ID = new ID("{AF094D81-9CF1-40F2-8A93-821BC96B9316}");

            public struct Fields
            {
                public static readonly ID CDNDefaultPath = new ID("{17E69600-A842-4BF1-AA87-4CF7B560BBF1}");
            }
        }
    }
}