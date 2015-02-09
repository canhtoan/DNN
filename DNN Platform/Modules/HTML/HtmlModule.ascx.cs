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
using System.Web.UI;

using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.WebControls;


#endregion
namespace DotNetNuke.Modules.Html
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    ///   The HtmlModule Class provides the UI for displaying the Html
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class HtmlModule : PortalModuleBase, IActionable
    {
        private bool _editorEnabled;
        private int _workflowID;

        #region "Private Methods"

        #endregion

        #region "Event Handlers"

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   Page_Init runs when the control is initialized
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            lblContent.UpdateLabel += lblContent_UpdateLabel;
            _editorEnabled = PortalSettings.InlineEditorEnabled;
            try
            {
                _workflowID = new HtmlTextController().GetWorkflow(ModuleId, TabId, PortalId).Value;

                //Add an Action Event Handler to the Skin
                AddActionHandler(ModuleAction_Click);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                var objHTML = new HtmlTextController();

                // edit in place
                if (_editorEnabled && IsEditable && PortalSettings.UserMode == PortalSettings.Mode.Edit)
                {
                    _editorEnabled = true;
                }
                else
                {
                    _editorEnabled = false;
                }

                // get content
                HtmlTextInfo htmlTextInfo = null;
                string contentString = "";

                htmlTextInfo = objHTML.GetTopHtmlText(ModuleId, !IsEditable, _workflowID);

                if ((htmlTextInfo != null))
                {
                    //don't decode yet (this is done in FormatHtmlText)
                    contentString = htmlTextInfo.Content;
                }
                else
                {
                    // get default content from resource file
                    if (!IsPostBack)
                    {
                        if (PortalSettings.UserMode == PortalSettings.Mode.Edit)
                        {
                            if (_editorEnabled)
                            {
                                contentString = Localization.GetString("AddContentFromToolBar.Text", LocalResourceFile);
                            }
                            else
                            {
                                contentString = Localization.GetString("AddContentFromActionMenu.Text", LocalResourceFile);
                            }
                        }
                        else
                        {
                            // hide the module if no content and in view mode
                            ContainerControl.Visible = false;
                        }
                    }
                }

                // token replace
                if (_editorEnabled && Settings["HtmlText_ReplaceTokens"] != null)
                {
                    _editorEnabled = !Convert.ToBoolean(Settings["HtmlText_ReplaceTokens"]);
                }

                // localize toolbar
                if (!IsPostBack)
                {
                    if (_editorEnabled)
                    {
                        foreach (DNNToolBarButton button in editorDnnToobar.Buttons)
                        {
                            button.ToolTip = Localization.GetString(button.ToolTip + ".ToolTip", LocalResourceFile);
                        }
                    }
                    else
                    {
                        editorDnnToobar.Visible = false;
                    }
                }

                lblContent.EditEnabled = _editorEnabled;

                // add content to module
                lblContent.Controls.Add(new LiteralControl(HtmlTextController.FormatHtmlText(ModuleId, contentString, Settings)));

                //set normalCheckBox on the content wrapper to prevent form decoration if its disabled.
                if (Settings.ContainsKey("HtmlText_UseDecorate") && Settings["HtmlText_UseDecorate"].ToString() == "0")
                {
                    lblContent.CssClass = string.Format("{0} normalCheckBox", lblContent.CssClass);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   lblContent_UpdateLabel allows for inline editing of content
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        private void lblContent_UpdateLabel(object source, DNNLabelEditEventArgs e)
        {
            try
            {
                // verify security 
                if ((!new PortalSecurity().InputFilter(e.Text, PortalSecurity.FilterFlag.NoScripting).Equals(e.Text)))
                {
                    throw new SecurityException();
                }
                else if (_editorEnabled && IsEditable && PortalSettings.UserMode == PortalSettings.Mode.Edit)
                {
                    // get content
                    var objHTML = new HtmlTextController();
                    var objWorkflow = new WorkflowStateController();
                    HtmlTextInfo objContent = objHTML.GetTopHtmlText(ModuleId, false, _workflowID);
                    if (objContent == null)
                    {
                        objContent = new HtmlTextInfo();
                        objContent.ItemID = -1;
                    }

                    // set content attributes
                    objContent.ModuleID = ModuleId;
                    objContent.Content = Server.HtmlEncode(e.Text);
                    objContent.WorkflowID = _workflowID;
                    objContent.StateID = objWorkflow.GetFirstWorkflowStateID(_workflowID);

                    // save the content
                    objHTML.UpdateHtmlText(objContent, objHTML.GetMaximumVersionHistory(PortalId));
                }
                else
                {
                    throw new SecurityException();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   ModuleAction_Click handles all ModuleAction events raised from the action menu
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        private void ModuleAction_Click(object sender, ActionEventArgs e)
        {
            try
            {
                if (e.Action.CommandArgument == "publish")
                {
                    // verify security 
                    if (IsEditable && PortalSettings.UserMode == PortalSettings.Mode.Edit)
                    {
                        // get content
                        var objHTML = new HtmlTextController();
                        HtmlTextInfo objContent = objHTML.GetTopHtmlText(ModuleId, false, _workflowID);

                        var objWorkflow = new WorkflowStateController();
                        if (objContent.StateID == objWorkflow.GetFirstWorkflowStateID(_workflowID))
                        {
                            // publish content
                            objContent.StateID = objWorkflow.GetNextWorkflowStateID(objContent.WorkflowID, objContent.StateID);

                            // save the content
                            objHTML.UpdateHtmlText(objContent, objHTML.GetMaximumVersionHistory(PortalId));

                            // refresh page
                            Response.Redirect(Globals.NavigateURL(), true);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region "Optional Interfaces"

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   ModuleActions is an interface property that returns the module actions collection for the module
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public ModuleActionCollection ModuleActions
        {
            get
            {
                // add the Edit Text action
                var Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(),
                            Localization.GetString(ModuleActionType.AddContent, LocalResourceFile),
                            ModuleActionType.AddContent,
                            "",
                            "",
                            EditUrl(),
                            false,
                            SecurityAccessLevel.Edit,
                            true,
                            false);

                // get the content
                var objHTML = new HtmlTextController();
                var objWorkflow = new WorkflowStateController();
                _workflowID = objHTML.GetWorkflow(ModuleId, TabId, PortalId).Value;

                HtmlTextInfo objContent = objHTML.GetTopHtmlText(ModuleId, false, _workflowID);
                if ((objContent != null))
                {
                    // if content is in the first state
                    if (objContent.StateID == objWorkflow.GetFirstWorkflowStateID(_workflowID))
                    {
                        // if not direct publish workflow
                        if (objWorkflow.GetWorkflowStates(_workflowID).Count > 1)
                        {
                            // add publish action
                            Actions.Add(GetNextActionID(),
                                        Localization.GetString("PublishContent.Action", LocalResourceFile),
                                        ModuleActionType.AddContent,
                                        "publish",
                                        "grant.gif",
                                        "",
                                        true,
                                        SecurityAccessLevel.Edit,
                                        true,
                                        false);
                        }
                    }
                    else
                    {
                        // if the content is not in the last state of the workflow then review is required
                        if (objContent.StateID != objWorkflow.GetLastWorkflowStateID(_workflowID))
                        {
                            // if the user has permissions to review the content
                            if (WorkflowStatePermissionController.HasWorkflowStatePermission(WorkflowStatePermissionController.GetWorkflowStatePermissions(objContent.StateID), "REVIEW"))
                            {
                                // add approve and reject actions
                                Actions.Add(GetNextActionID(),
                                            Localization.GetString("ApproveContent.Action", LocalResourceFile),
                                            ModuleActionType.AddContent,
                                            "",
                                            "grant.gif",
                                            EditUrl("action", "approve", "Review"),
                                            false,
                                            SecurityAccessLevel.Edit,
                                            true,
                                            false);
                                Actions.Add(GetNextActionID(),
                                            Localization.GetString("RejectContent.Action", LocalResourceFile),
                                            ModuleActionType.AddContent,
                                            "",
                                            "deny.gif",
                                            EditUrl("action", "reject", "Review"),
                                            false,
                                            SecurityAccessLevel.Edit,
                                            true,
                                            false);
                            }
                        }
                    }
                }

                // add mywork to action menu
                Actions.Add(GetNextActionID(),
                            Localization.GetString("MyWork.Action", LocalResourceFile),
                            "MyWork.Action",
                            "",
                            "view.gif",
                            EditUrl("MyWork"),
                            false,
                            SecurityAccessLevel.Edit,
                            true,
                            false);

                return Actions;
            }
        }

        #endregion
    }
}