using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;

namespace DotNetNuke.Web.DDRMenu.DNNCommon
{
    public class DNNContext : IDisposable
    {
        public static DNNContext Current { get { return (DNNContext)HttpContext.Current.Items[DataName]; } private set { HttpContext.Current.Items[DataName] = value; } }

        private readonly DNNContext _savedContext;

        public Control HostControl { get; private set; }

        private Page _Page;
        public Page Page { get { return _Page ?? (_Page = HostControl.Page); } }

        private PortalSettings _PortalSettings;
        public PortalSettings PortalSettings { get { return _PortalSettings ?? (_PortalSettings = PortalController.Instance.GetCurrentPortalSettings()); } }

        private TabInfo _ActiveTab;
        public TabInfo ActiveTab { get { return _ActiveTab ?? (_ActiveTab = PortalSettings.ActiveTab); } }

        private string _SkinPath;
        public string SkinPath { get { return _SkinPath ?? (_SkinPath = ActiveTab.SkinPath); } }

        private static string s_moduleName;
        public static string ModuleName { get { return s_moduleName ?? (s_moduleName = GetModuleNameFromAssembly()); } }

        private static string s_moduleFolder;
        public static string ModuleFolder
        {
            get
            {
                return s_moduleFolder ??
                       (s_moduleFolder =
                        String.Format(
                            "~/DesktopModules/{0}/", DesktopModuleController.GetDesktopModuleByModuleName(ModuleName, PortalSettings.Current.PortalId).FolderName));
            }
        }

        private static string s_dataName;
        private static string DataName { get { return s_dataName ?? (s_dataName = "DDRMenu.DNNContext." + ModuleName); } }

        public DNNContext(Control hostControl)
        {
            HostControl = hostControl;

            _savedContext = Current;
            Current = this;
        }

        public string ResolveUrl(string relativeUrl)
        {
            return HostControl.ResolveUrl(relativeUrl);
        }

        private static string GetModuleNameFromAssembly()
        {
            var moduleFullName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            // ReSharper disable PossibleNullReferenceException
            return moduleFullName.Substring(moduleFullName.LastIndexOf('.') + 1);
            // ReSharper restore PossibleNullReferenceException
        }

        public void Dispose()
        {
            Current = _savedContext;
        }
    }
}