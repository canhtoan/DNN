

using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.WebControls;
using effority.Ealo.Specialized;
using EaloTabInfo = effority.Ealo.Specialized.TabInfo;
using TabInfo = DotNetNuke.Entities.Tabs.TabInfo;
using DotNetNuke.Entities.Portals;

namespace DotNetNuke.Web.DDRMenu.Localisation
{
    public class Ealo : ILocalisation
    {
        private bool _haveChecked;
        private bool _found;

        public bool HaveApi()
        {
            if (!_haveChecked)
            {
                _found = (DesktopModuleController.GetDesktopModuleByModuleName("effority.Ealo.Tabs", PortalSettings.Current.PortalId) != null);
                _haveChecked = true;
            }

            return _found;
        }

        public TabInfo LocaliseTab(TabInfo tab, int portalId)
        {
            return EaloWorker.LocaliseTab(tab, portalId);
        }

        public DNNNodeCollection LocaliseNodes(DNNNodeCollection nodes)
        {
            return null;
        }

        // Separate class only instantiated if Ealo is available.
        private static class EaloWorker
        {
            private static readonly Dictionary<string, Dictionary<int, EaloTabInfo>> s_ealoTabLookup =
                new Dictionary<string, Dictionary<int, EaloTabInfo>>();

            public static TabInfo LocaliseTab(TabInfo tab, int portalId)
            {
                var culture = DNNAbstract.GetCurrentCulture();
                Dictionary<int, EaloTabInfo> ealoTabs;
                if (!s_ealoTabLookup.TryGetValue(culture, out ealoTabs))
                {
                    ealoTabs = Tabs.GetAllTabsAsDictionary(culture, true);
                    lock (s_ealoTabLookup)
                    {
                        if (!s_ealoTabLookup.ContainsKey(culture))
                        {
                            s_ealoTabLookup.Add(culture, ealoTabs);
                        }
                    }
                }

                EaloTabInfo ealoTab;
                if (ealoTabs.TryGetValue(tab.TabID, out ealoTab))
                {
                    if (ealoTab.EaloTabName != null)
                    {
                        tab.TabName = ealoTab.EaloTabName.StringTextOrFallBack;
                    }
                    if (ealoTab.EaloTitle != null)
                    {
                        tab.Title = ealoTab.EaloTitle.StringTextOrFallBack;
                    }
                }
                return tab;
            }
        }
    }
}