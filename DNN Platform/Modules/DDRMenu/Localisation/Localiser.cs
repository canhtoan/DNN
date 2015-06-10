

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.WebControls;
using EaloTabInfo = effority.Ealo.Specialized.TabInfo;

namespace DotNetNuke.Web.DDRMenu.Localisation
{
    public class Localiser
    {
        private readonly int _portalId;
        private static bool s_apiChecked;
        private static ILocalisation s_localisationApi;
        private static ILocalisation LocalisationApi
        {
            get
            {
                if (!s_apiChecked)
                {
                    foreach (var api in new ILocalisation[] { new Generic(), new Ealo(), new Apollo() }) //new Adequation() 
                    {
                        if (api.HaveApi())
                        {
                            s_localisationApi = api;
                            break;
                        }
                    }
                    s_apiChecked = true;
                }
                return s_localisationApi;
            }
        }

        public static DNNNodeCollection LocaliseDNNNodeCollection(DNNNodeCollection nodes)
        {
            return (LocalisationApi == null) ? nodes : (LocalisationApi.LocaliseNodes(nodes) ?? nodes);
        }

        public Localiser(int portalId)
        {
            _portalId = portalId;
        }

        public void LocaliseNode(MenuNode node)
        {
            var tab = (node.TabId > 0) ? TabController.Instance.GetTab(node.TabId, Null.NullInteger, false) : null;
            if (tab != null)
            {
                var localised = LocaliseTab(tab);
                tab = localised ?? tab;

                if (localised != null)
                {
                    node.TabId = tab.TabID;
                    node.Text = tab.TabName;
                    node.Enabled = !tab.DisableLink;
                    if (!tab.IsVisible)
                    {
                        node.TabId = -1;
                    }
                }

                node.Title = tab.Title;
                node.Description = tab.Description;
                node.Keywords = tab.KeyWords;
            }
            else
            {
                node.TabId = -1;
            }

            node.Children.ForEach(LocaliseNode);
        }

        private TabInfo LocaliseTab(TabInfo tab)
        {
            return (LocalisationApi == null) ? null : LocalisationApi.LocaliseTab(tab, _portalId);
        }
    }
}