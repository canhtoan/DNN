

using System;
using System.Reflection;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Framework;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Modules.Dashboard.Components.Portals;

namespace DotNetNuke.Web.DDRMenu.Localisation
{
    public class Generic : ILocalisation
    {
        private bool _haveChecked;
        private object _locApi;
        private MethodInfo _locTab;
        private MethodInfo _locNodes;

        public bool HaveApi()
        {
            if (!_haveChecked)
            {
                var modules = DesktopModuleController.GetDesktopModules(PortalSettings.Current.PortalId);
                foreach (var moduleKeyPair in modules)
                {
                    if (!String.IsNullOrEmpty(moduleKeyPair.Value.BusinessControllerClass))
                    {
                        try
                        {
                            _locApi = Reflection.CreateObject(moduleKeyPair.Value.BusinessControllerClass, moduleKeyPair.Value.BusinessControllerClass);
                            _locTab = _locApi.GetType().GetMethod("LocaliseTab", new[] { typeof(TabInfo), typeof(int) });
                            if (_locTab != null)
                            {
                                if (_locTab.IsStatic)
                                {
                                    _locApi = null;
                                }
                                break;
                            }

                            _locNodes = _locApi.GetType().GetMethod("LocaliseNodes", new[] { typeof(DNNNodeCollection) });
                            if (_locNodes != null)
                            {
                                if (_locNodes.IsStatic)
                                {
                                    _locApi = null;
                                }
                                break;
                            }
                        }
                        // ReSharper disable EmptyGeneralCatchClause
                        catch
                        {
                        }
                        // ReSharper restore EmptyGeneralCatchClause
                    }
                }
                _haveChecked = true;
            }

            return (_locTab != null) || (_locNodes != null);
        }

        public TabInfo LocaliseTab(TabInfo tab, int portalId)
        {
            return (_locTab == null) ? null : (TabInfo)_locTab.Invoke(_locApi, new object[] { tab, portalId });
        }

        public DNNNodeCollection LocaliseNodes(DNNNodeCollection nodes)
        {
            return (_locNodes == null) ? null : (DNNNodeCollection)_locNodes.Invoke(_locApi, new object[] { nodes });
        }
    }
}