

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Web.DDRMenu.DNNCommon;

namespace DotNetNuke.Web.DDRMenu
{
    internal class DDRMenuControl : WebControl, IPostBackEventHandler
    {
        public override bool EnableViewState { get { return false; } set { } }

        internal MenuNode RootNode { get; set; }
        internal Boolean SkipLocalisation { get; set; }
        internal Settings MenuSettings { get; set; }

        public delegate void MenuClickEventHandler(string id);

        public event MenuClickEventHandler NodeClick;

        private MenuBase _menu;

        protected override void OnPreRender(EventArgs e)
        {
            using (new DNNContext(this))
            {
                base.OnPreRender(e);

                MenuSettings.MenuStyle = MenuSettings.MenuStyle ?? "DNNMenu";
                _menu = MenuBase.Instantiate(MenuSettings.MenuStyle);
                _menu.RootNode = RootNode ?? new MenuNode();
                _menu.SkipLocalisation = SkipLocalisation;
                _menu.ApplySettings(MenuSettings);

                _menu.PreRender();
            }
        }

        protected override void Render(HtmlTextWriter htmlWriter)
        {
            using (new DNNContext(this))
                _menu.Render(htmlWriter);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            using (new DNNContext(this))
            {
                if (NodeClick != null)
                {
                    NodeClick(eventArgument);
                }
            }
        }
    }
}