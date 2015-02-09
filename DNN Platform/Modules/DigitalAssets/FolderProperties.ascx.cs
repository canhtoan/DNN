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
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.ExtensionPoints;
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
    public partial class FolderProperties : PortalModuleBase
    {
        private static readonly DigitalAssetsSettingsRepository s_settingsRepository = new DigitalAssetsSettingsRepository();

        private readonly IDigitalAssetsController _controller = (new Factory()).DigitalAssetsController;
        private FolderViewModel _folderViewModel;
        private bool _isRootFolder;
        private Control _folderFieldsControl;

        protected IFolderInfo Folder { get; private set; }

        protected bool CanManageFolder
        {
            get
            {
                return UserInfo.IsSuperUser || FolderPermissionController.CanManageFolder((FolderInfo)Folder);
            }
        }

        protected bool HasFullControl { get; private set; }

        protected string DialogTitle
        {
            get
            {
                return string.Format(LocalizeString("DialogTitle"), _folderViewModel.FolderName);
            }
        }

        protected bool IsHostPortal
        {
            get
            {
                return IsHostMenu || _controller.GetCurrentPortalId(ModuleId) == Null.NullInteger;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);

                JavaScript.RequestRegistration(CommonJs.DnnPlugins);

                var folderId = Convert.ToInt32(Request.Params["FolderId"]);
                Folder = FolderManager.Instance.GetFolder(folderId);
                HasFullControl = UserInfo.IsSuperUser || FolderPermissionController.HasFolderPermission(Folder.FolderPermissions, "FULLCONTROL");

                FolderViewModel rootFolder;
                switch (s_settingsRepository.GetMode(ModuleId))
                {
                    case DigitalAssestsMode.Group:
                        var groupId = Convert.ToInt32(Request.Params["GroupId"]);
                        rootFolder = _controller.GetGroupFolder(groupId, PortalSettings);
                        if (rootFolder == null)
                        {
                            throw new Exception("Invalid group folder");
                        }
                        break;

                    case DigitalAssestsMode.User:
                        rootFolder = _controller.GetUserFolder(PortalSettings.UserInfo);
                        break;

                    default:
                        rootFolder = _controller.GetRootFolder(ModuleId);
                        break;
                }

                _isRootFolder = rootFolder.FolderID == folderId;
                _folderViewModel = _isRootFolder ? rootFolder : _controller.GetFolder(folderId);

                // Setup controls
                CancelButton.Click += OnCancelClick;
                SaveButton.Click += OnSaveClick;
                PrepareFolderPreviewInfo();
                cmdCopyPerm.Click += cmdCopyPerm_Click;

                var mef = new ExtensionPointManager();
                var folderFieldsExtension = mef.GetUserControlExtensionPointFirstByPriority("DigitalAssets", "FolderFieldsControlExtensionPoint");
                if (folderFieldsExtension != null)
                {
                    _folderFieldsControl = Page.LoadControl(folderFieldsExtension.UserControlSrc);
                    _folderFieldsControl.ID = _folderFieldsControl.GetType().BaseType.Name;
                    FolderDynamicFieldsContainer.Controls.Add(_folderFieldsControl);
                    var fieldsControl = _folderFieldsControl as IFieldsControl;
                    if (fieldsControl != null)
                    {
                        fieldsControl.SetController(_controller);
                        fieldsControl.SetItemViewModel(new ItemViewModel
                        {
                            ItemID = _folderViewModel.FolderID,
                            IsFolder = true,
                            PortalID = _folderViewModel.PortalID,
                            ItemName = _folderViewModel.FolderName
                        });
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    return;
                }

                SaveFolderProperties();

                SavePermissions();
                Page.CloseClientDialog(true);
            }
            catch (ThreadAbortException)
            {
            }
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

        private void SaveFolderProperties()
        {
            if (!CanManageFolder)
            {
                throw new DotNetNukeException(LocalizeString("UserCannotEditFolderError"));
            }

            if (!_isRootFolder)
            {
                _controller.RenameFolder(_folderViewModel.FolderID, FolderNameInput.Text);
            }

            var fieldsControl = _folderFieldsControl as IFieldsControl;
            if (fieldsControl != null)
            {
                Folder = (IFolderInfo)fieldsControl.SaveProperties();
            }
        }

        private void SavePermissions()
        {
            if (!CanManageFolder)
            {
                throw new DotNetNukeException(LocalizeString("UserCannotChangePermissionsError"));
            }

            Folder = FolderManager.Instance.GetFolder(Folder.FolderID);
            Folder.FolderPermissions.Clear();
            Folder.FolderPermissions.AddRange(PermissionsGrid.Permissions);
            FolderPermissionController.SaveFolderPermissions(Folder);
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            Page.CloseClientDialog(false);
        }

        private void cmdCopyPerm_Click(object sender, EventArgs e)
        {
            try
            {
                FolderPermissionController.CopyPermissionsToSubfolders(Folder, PermissionsGrid.Permissions);
                UI.Skins.Skin.AddModuleMessage(this, LocalizeString("PermissionsCopied"), ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            catch (Exception ex)
            {
                UI.Skins.Skin.AddModuleMessage(this, LocalizeString("PermissionCopyError"), ModuleMessage.ModuleMessageType.RedError);
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SetupPermissionGrid();
                    PrepareFolderProperties();
                    SetPropertiesAvailability(FolderPermissionController.CanManageFolder((FolderInfo)Folder));
                }

                if (!FolderPermissionController.CanViewFolder((FolderInfo)Folder))
                {
                    SaveButton.Visible = false;
                    SetPropertiesVisibility(false);
                    UI.Skins.Skin.AddModuleMessage(this, LocalizeString("UserCannotReadFolderError"), ModuleMessage.ModuleMessageType.RedError);
                }
                else
                {
                    SaveButton.Visible = FolderPermissionController.CanViewFolder((FolderInfo)Folder) && FolderPermissionController.CanManageFolder((FolderInfo)Folder);
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

        private void SetPropertiesAvailability(bool availability)
        {
            FolderNameInput.Enabled = (!_isRootFolder) && availability;
            var fieldsControl = _folderFieldsControl as IFieldsControl;
            if (fieldsControl != null)
            {
                fieldsControl.SetPropertiesAvailability(availability);
            }
        }

        private void SetPropertiesVisibility(bool visibility)
        {
            FolderNameInput.Visible = visibility;
            FolderTypeLiteral.Visible = visibility;
            FolderInfoPreviewPanel.Visible = visibility;
            var fieldsControl = _folderFieldsControl as IFieldsControl;
            if (fieldsControl != null)
            {
                fieldsControl.SetPropertiesVisibility(visibility);
            }
        }

        private void PrepareFolderProperties()
        {
            FolderNameInput.Text = _folderViewModel.FolderName;
            FolderTypeLiteral.Text = FolderMappingController.Instance.GetFolderMapping(_folderViewModel.FolderMappingID).MappingName;

            FolderNameInvalidCharactersValidator.ValidationExpression = "^([^" + Regex.Escape(_controller.GetInvalidChars()) + "]+)$";
            FolderNameInvalidCharactersValidator.ErrorMessage = _controller.GetInvalidCharsErrorText();

            var fieldsControl = _folderFieldsControl as IFieldsControl;
            if (fieldsControl != null)
            {
                fieldsControl.PrepareProperties();
            }
        }

        private void PrepareFolderPreviewInfo()
        {
            var folderPreviewPanel = (PreviewPanelControl)FolderInfoPreviewPanel;
            if (folderPreviewPanel != null)
            {
                folderPreviewPanel.SetPreviewInfo(_controller.GetFolderPreviewInfo(Folder));
            }
        }

        private void SetupPermissionGrid()
        {
            PermissionsGrid.FolderPath = Folder.FolderPath;
            PermissionsGrid.Visible = HasFullControl && !IsHostPortal;
        }
    }
}