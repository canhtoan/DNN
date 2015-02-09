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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.ExtensionPoints;
using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Modules.DigitalAssets.Components.Controllers;
using DotNetNuke.Modules.DigitalAssets.Components.Controllers.Models;
using DotNetNuke.Modules.DigitalAssets.Components.ExtensionPoint;
using DotNetNuke.Modules.DigitalAssets.Services;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.UI;

namespace DotNetNuke.Modules.DigitalAssets
{
    public partial class FileProperties : PortalModuleBase
    {
        private readonly IDigitalAssetsController _controller = (new Factory()).DigitalAssetsController;

        private IFileInfo _file;
        private IFolderInfo _folder;
        private ItemViewModel _fileItem;
        private Control _previewPanelControl;
        private Control _fileFieldsControl;
        private IEnumerable<PropertiesTabContentControl> _tabContentControls;

        protected string DialogTitle
        {
            get
            {
                return _fileItem.ItemName;
            }
        }

        protected bool CanManageFolder
        {
            get
            {
                return UserInfo.IsSuperUser || FolderPermissionController.CanManageFolder((FolderInfo)_folder);
            }
        }

        protected string ActiveTab
        {
            get
            {
                return Request.QueryString["activeTab"];
            }
        }


        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);

                JavaScript.RequestRegistration(CommonJs.DnnPlugins);

                var fileId = Convert.ToInt32(Request.Params["FileId"]);
                _file = FileManager.Instance.GetFile(fileId, true);
                _fileItem = _controller.GetFile(fileId);
                _folder = FolderManager.Instance.GetFolder(_file.FolderId);

                SaveButton.Click += OnSaveClick;
                CancelButton.Click += OnCancelClick;

                if (FolderPermissionController.CanViewFolder((FolderInfo)_folder))
                {
                    var mef = new ExtensionPointManager();
                    var preViewPanelExtension = mef.GetUserControlExtensionPointFirstByPriority("DigitalAssets", "PreviewInfoPanelExtensionPoint");
                    _previewPanelControl = Page.LoadControl(preViewPanelExtension.UserControlSrc);
                    PreviewPanelContainer.Controls.Add(_previewPanelControl);

                    var fileFieldsExtension = mef.GetUserControlExtensionPointFirstByPriority("DigitalAssets", "FileFieldsControlExtensionPoint");
                    _fileFieldsControl = Page.LoadControl(fileFieldsExtension.UserControlSrc);
                    _fileFieldsControl.ID = _fileFieldsControl.GetType().BaseType.Name;
                    FileFieldsContainer.Controls.Add(_fileFieldsControl);

                    PrepareFilePreviewInfoControl();
                    PrepareFileFieldsControl();

                    // Tab Extension Point
                    var tabContentControlsInstances = new List<PropertiesTabContentControl>();
                    foreach (var extension in mef.GetEditPageTabExtensionPoints("DigitalAssets", "FilePropertiesTab"))
                    {
                        if (FolderPermissionController.HasFolderPermission(_folder.FolderPermissions, extension.Permission))
                        {
                            var liElement = new HtmlGenericControl("li") { InnerHtml = "<a href=\"#" + extension.EditPageTabId + "\">" + extension.Text + "</a>", };
                            liElement.Attributes.Add("class", extension.CssClass);
                            liElement.Attributes.Add("id", extension.EditPageTabId + "_tab");
                            Tabs.Controls.Add(liElement);

                            var container = new PanelTabExtensionControl { PanelId = extension.EditPageTabId };
                            var control = (PortalModuleBase)Page.LoadControl(extension.UserControlSrc);
                            control.ID = Path.GetFileNameWithoutExtension(extension.UserControlSrc);
                            control.ModuleConfiguration = ModuleConfiguration;
                            var contentControl = control as PropertiesTabContentControl;
                            if (contentControl != null)
                            {
                                contentControl.OnItemUpdated += OnItemUpdated;
                                tabContentControlsInstances.Add(contentControl);
                            }
                            container.Controls.Add(control);
                            TabsPanel.Controls.Add(container);
                        }
                    }
                    _tabContentControls = tabContentControlsInstances.ToList();
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SetPropertiesAvailability(CanManageFolder);
                }

                if (!FolderPermissionController.CanViewFolder((FolderInfo)_folder))
                {
                    SaveButton.Visible = false;
                    SetPropertiesVisibility(false);
                    UI.Skins.Skin.AddModuleMessage(this, LocalizeString("UserCannotReadFileError"), ModuleMessage.ModuleMessageType.RedError);
                }
                else
                {
                    SetFilePreviewInfo();
                    SaveButton.Visible = FolderPermissionController.CanViewFolder((FolderInfo)_folder) && FolderPermissionController.CanManageFolder((FolderInfo)_folder);
                }
            }
            catch (DotNetNukeException dnnex)
            {
                UI.Skins.Skin.AddModuleMessage(this, dnnex.Message, ModuleMessage.ModuleMessageType.RedError);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void OnItemUpdated()
        {
            SetFilePreviewInfo();
            foreach (var propertiesTabContentControl in _tabContentControls)
            {
                propertiesTabContentControl.DataBindItem();
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                SaveFileProperties();
                Page.CloseClientDialog(true);
            }
            catch (ThreadAbortException) { }
            catch (DotNetNukeException dnnex)
            {
                UI.Skins.Skin.AddModuleMessage(this, dnnex.Message, ModuleMessage.ModuleMessageType.RedError);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                UI.Skins.Skin.AddModuleMessage(this, ex.Message, ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            Page.CloseClientDialog(false);
        }

        private void SaveFileProperties()
        {
            _file = (IFileInfo)((FileFieldsControl)_fileFieldsControl).SaveProperties();
        }

        private void SetPropertiesVisibility(bool visibility)
        {
            ((FileFieldsControl)_fileFieldsControl).SetPropertiesVisibility(visibility);
        }

        private void SetPropertiesAvailability(bool availability)
        {
            ((FileFieldsControl)_fileFieldsControl).SetPropertiesAvailability(availability);
        }

        private void SetFilePreviewInfo()
        {
            var previewPanelInstance = (PreviewPanelControl)_previewPanelControl;
            previewPanelInstance.SetPreviewInfo(_controller.GetFilePreviewInfo(_file, _fileItem));
        }

        private void PrepareFilePreviewInfoControl()
        {
            var previewPanelInstance = (PreviewPanelControl)_previewPanelControl;
            previewPanelInstance.SetController(_controller);
            previewPanelInstance.SetModuleConfiguration(ModuleConfiguration);
        }

        private void PrepareFileFieldsControl()
        {
            var fileFieldsIntance = (FileFieldsControl)_fileFieldsControl;
            fileFieldsIntance.SetController(_controller);
            fileFieldsIntance.SetItemViewModel(_fileItem);
            fileFieldsIntance.SetFileInfo(_file);
            fileFieldsIntance.SetModuleConfiguration(ModuleConfiguration);
        }
    }
}