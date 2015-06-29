namespace DotNetNuke.Services.Settings
{
    using DotNetNuke.Entities.Portals;

    public class PortalScopedSettings : StringBasedSettings
    {
        public PortalScopedSettings(int portalId)
            : base(
                name => PortalController.GetPortalSetting(name, portalId, ""),
                (name, value) => PortalController.UpdatePortalSetting(portalId, name, value, true)
                )
        { }
    }
}