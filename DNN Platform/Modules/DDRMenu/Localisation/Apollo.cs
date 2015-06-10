

using System;
using System.Collections.Generic;
using System.Reflection;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Entities.Portals;

namespace DotNetNuke.Web.DDRMenu.Localisation
{
    public class Apollo : ILocalisation
    {
        private bool _haveChecked;
        private MethodInfo _apiMember;

        public bool HaveApi()
        {
            if (!_haveChecked)
            {
                try
                {
                    if (DesktopModuleController.GetDesktopModuleByModuleName("PageLocalization", PortalSettings.Current.PortalId) != null)
                    {
                        var api = Activator.CreateInstance("Apollo.LocalizationApi", "Apollo.DNN_Localization.LocalizeTab").Unwrap();
                        var apiType = api.GetType();
                        _apiMember = apiType.GetMethod("getLocalizedTab", new[] { typeof(TabInfo) });
                    }
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                }
                _haveChecked = true;
            }

            return (_apiMember != null);
        }

        public TabInfo LocaliseTab(TabInfo tab, int portalId)
        {
            return _apiMember.Invoke(null, new object[] { tab }) as TabInfo ?? tab;
        }

        public DNNNodeCollection LocaliseNodes(DNNNodeCollection nodes)
        {
            return null;
        }
    }
}