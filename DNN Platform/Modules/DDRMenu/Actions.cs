

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Modules.NavigationProvider;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI;
using DotNetNuke.UI.Containers;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.DDRMenu.DNNCommon;
using DotNetNuke.Web.DDRMenu.TemplateEngine;

namespace DotNetNuke.Web.DDRMenu
{
    public class Actions : ActionBase
    {
        public string PathSystemScript { get; set; }

        public string MenuStyle { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ClientOption> ClientOptions { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<TemplateArgument> TemplateArguments { get; set; }

        private DDRMenuNavigationProvider _navProvider;
        private Dictionary<int, ModuleAction> _actions;

        protected override void OnInit(EventArgs e)
        {
            using (new DNNContext(this))
            {
                base.OnInit(e);

                _navProvider = (DDRMenuNavigationProvider)NavigationProvider.Instance("DDRMenuNavigationProvider");
                _navProvider.ControlID = "ctl" + ID;
                _navProvider.MenuStyle = MenuStyle;
                _navProvider.Initialize();

                Controls.Add(_navProvider.NavigationControl);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            using (new DNNContext(this))
            {
                base.OnLoad(e);

                SetMenuDefaults();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            using (new DNNContext(this))
            {
                base.OnPreRender(e);

                try
                {
                    _navProvider.TemplateArguments = TemplateArguments;
                    BindMenu(Navigation.GetActionNodes(ActionRoot, this, -1));
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        private void BindMenu(DNNNodeCollection objNodes)
        {
            Visible = DisplayControl(objNodes);
            if (!Visible)
            {
                return;
            }

            _navProvider.ClearNodes();
            foreach (DNNNode node in objNodes)
            {
                ProcessNode(node);
            }
            _navProvider.Bind(objNodes, false);
        }

        private void ActionClick(NavigationEventArgs args)
        {
            using (new DNNContext(this))
            {
                try
                {
                    ProcessAction(args.ID);
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        private void AddActionIDs(ModuleAction action)
        {
            if (!_actions.ContainsKey(action.ID))
            {
                _actions.Add(action.ID, action);
            }
            if (action.HasChildren())
            {
                foreach (ModuleAction a in action.Actions)
                {
                    AddActionIDs(a);
                }
            }
        }

        private ModuleAction FindAction(int id)
        {
            if (_actions == null)
            {
                _actions = new Dictionary<int, ModuleAction>();
                AddActionIDs(ActionRoot);
            }

            ModuleAction result;
            return _actions.TryGetValue(id, out result) ? result : null;
        }

        private void ProcessNode(DNNNode dnnNode)
        {
            if (!dnnNode.IsBreak)
            {
                var action = FindAction(Convert.ToInt32(dnnNode.Key));
                if (action != null)
                {
                    dnnNode.set_CustomAttribute("CommandName", action.CommandName);
                    dnnNode.set_CustomAttribute("CommandArgument", action.CommandArgument);
                }
            }

            if (!String.IsNullOrEmpty(dnnNode.JSFunction))
            {
                dnnNode.JSFunction = string.Format(
                    "if({0}){{{1}}};",
                    dnnNode.JSFunction,
                    Page.ClientScript.GetPostBackEventReference(_navProvider.NavigationControl, dnnNode.ID));
            }

            foreach (DNNNode node in dnnNode.DNNNodes)
            {
                ProcessNode(node);
            }
        }

        private void SetMenuDefaults()
        {
            try
            {
                _navProvider.StyleIconWidth = 15M;
                _navProvider.MouseOutHideDelay = 500M;
                _navProvider.MouseOverAction = NavigationProvider.HoverAction.Expand;
                _navProvider.MouseOverDisplay = NavigationProvider.HoverDisplay.None;
                _navProvider.CSSControl = "ModuleTitle_MenuBar";
                _navProvider.CSSContainerRoot = "ModuleTitle_MenuContainer";
                _navProvider.CSSNode = "ModuleTitle_MenuItem";
                _navProvider.CSSIcon = "ModuleTitle_MenuIcon";
                _navProvider.CSSContainerSub = "ModuleTitle_SubMenu";
                _navProvider.CSSBreak = "ModuleTitle_MenuBreak";
                _navProvider.CSSNodeHover = "ModuleTitle_MenuItemSel";
                _navProvider.CSSIndicateChildSub = "ModuleTitle_MenuArrow";
                _navProvider.CSSIndicateChildRoot = "ModuleTitle_RootMenuArrow";
                if (String.IsNullOrEmpty(_navProvider.PathSystemScript))
                {
                    _navProvider.PathSystemScript = Globals.ApplicationPath + "/Controls/SolpartMenu/";
                }
                _navProvider.PathImage = Globals.ApplicationPath + "/Images/";
                _navProvider.PathSystemImage = Globals.ApplicationPath + "/Images/";
                _navProvider.IndicateChildImageSub = "action_right.gif";
                _navProvider.IndicateChildren = true;
                _navProvider.StyleRoot = "background-color: Transparent; font-size: 1pt;";
                _navProvider.NodeClick += ActionClick;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}