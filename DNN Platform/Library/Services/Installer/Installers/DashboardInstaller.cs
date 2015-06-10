#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion
#region Usings
using System;
using System.Xml.XPath;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Dashboard.Components;


#endregion
namespace DotNetNuke.Services.Installer.Installers
{
    public class DashboardInstaller : ComponentInstallerBase
    {
        #region "Private Properties"

        private string _controllerClass;
        private bool _isEnabled;
        private string _key;
        private string _localResources;
        private string _src;
        private DashboardControl _tempDashboardControl;
        private int _viewOrder;

        #endregion

        #region "Public Properties"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of allowable file extensions (in addition to the Host's List)
        /// </summary>
        /// <value>A String</value>
        /// <history>
        /// 	[cnurse]	01/05/2009  created
        /// </history>
        /// -----------------------------------------------------------------------------
        public override string AllowableFiles
        {
            get
            {
                return "ashx, aspx, ascx, vb, cs, resx, css, js, resources, config, vbproj, csproj, sln, htm, html";
            }
        }

        #endregion

        #region "Private Methods"

        private void DeleteDashboard()
        {
            try
            {
                //Attempt to get the Dashboard
                DashboardControl dashboardControl = DashboardController.GetDashboardControlByPackageId(Package.PackageID);
                if (dashboardControl != null)
                {
                    DashboardController.DeleteControl(dashboardControl);
                }
                Log.AddInfo(dashboardControl.DashboardControlKey + " " + Util.AUTHENTICATION_UnRegistered);
            }
            catch (Exception ex)
            {
                Log.AddFailure(ex);
            }
        }

        public override void Commit()
        {
        }

        public override void Install()
        {
            bool bAdd = Null.NullBoolean;
            try
            {
                //Attempt to get the Dashboard
                _tempDashboardControl = DashboardController.GetDashboardControlByKey(_key);
                var dashboardControl = new DashboardControl();

                if (_tempDashboardControl == null)
                {
                    dashboardControl.IsEnabled = true;
                    bAdd = true;
                }
                else
                {
                    dashboardControl.DashboardControlID = _tempDashboardControl.DashboardControlID;
                    dashboardControl.IsEnabled = _tempDashboardControl.IsEnabled;
                }
                dashboardControl.DashboardControlKey = _key;
                dashboardControl.PackageID = Package.PackageID;
                dashboardControl.DashboardControlSrc = _src;
                dashboardControl.DashboardControlLocalResources = _localResources;
                dashboardControl.ControllerClass = _controllerClass;
                dashboardControl.ViewOrder = _viewOrder;
                if (bAdd)
                {
                    //Add new Dashboard
                    DashboardController.AddDashboardControl(dashboardControl);
                }
                else
                {
                    //Update Dashboard
                    DashboardController.UpdateDashboardControl(dashboardControl);
                }
                Completed = true;
                Log.AddInfo(dashboardControl.DashboardControlKey + " " + Util.DASHBOARD_Registered);
            }
            catch (Exception ex)
            {
                Log.AddFailure(ex);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The ReadManifest method reads the manifest file for the Authentication compoent.
        /// </summary>
        /// <history>
        /// 	[cnurse]	07/25/2007  created
        /// </history>
        /// -----------------------------------------------------------------------------
        public override void ReadManifest(XPathNavigator manifestNav)
        {
            //Get the Key
            _key = Util.ReadElement(manifestNav, "dashboardControl/key", Log, Util.DASHBOARD_KeyMissing);

            //Get the Src
            _src = Util.ReadElement(manifestNav, "dashboardControl/src", Log, Util.DASHBOARD_SrcMissing);

            //Get the LocalResources
            _localResources = Util.ReadElement(manifestNav, "dashboardControl/localResources", Log, Util.DASHBOARD_LocalResourcesMissing);

            //Get the ControllerClass
            _controllerClass = Util.ReadElement(manifestNav, "dashboardControl/controllerClass");

            //Get the IsEnabled Flag
            _isEnabled = bool.Parse(Util.ReadElement(manifestNav, "dashboardControl/isEnabled", "true"));

            //Get the ViewOrder
            _viewOrder = int.Parse(Util.ReadElement(manifestNav, "dashboardControl/viewOrder", "-1"));

            if (Log.Valid)
            {
                Log.AddInfo(Util.DASHBOARD_ReadSuccess);
            }
        }

        public override void Rollback()
        {
            //If Temp Dashboard exists then we need to update the DataStore with this 
            if (_tempDashboardControl == null)
            {
                //No Temp Dashboard - Delete newly added system
                DeleteDashboard();
            }
            else
            {
                //Temp Dashboard - Rollback to Temp
                DashboardController.UpdateDashboardControl(_tempDashboardControl);
            }
        }

        public override void UnInstall()
        {
            try
            {
                //Attempt to get the DashboardControl
                DashboardControl dashboardControl = DashboardController.GetDashboardControlByPackageId(Package.PackageID);
                if (dashboardControl != null)
                {
                    DashboardController.DeleteControl(dashboardControl);
                }
                Log.AddInfo(dashboardControl.DashboardControlKey + " " + Util.DASHBOARD_UnRegistered);
            }
            catch (Exception ex)
            {
                Log.AddFailure(ex);
            }
        }

        #endregion
    }
}
