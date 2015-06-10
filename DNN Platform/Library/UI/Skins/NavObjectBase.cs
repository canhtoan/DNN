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
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

using DotNetNuke.Common;
using DotNetNuke.Modules.NavigationProvider;
using DotNetNuke.UI.WebControls;


#endregion
namespace DotNetNuke.UI.Skins
{
    public class NavObjectBase : SkinObjectBase
    {
        #region "Private Members"

        private readonly List<CustomAttribute> _objCustomAttributes = new List<CustomAttribute>();
        private bool _blnPopulateNodesFromClient = true;
        private int _intExpandDepth = -1;
        private int _intStartTabId = -1;
        private NavigationProvider _objControl;
        private string _strCSSBreadCrumbRoot;

        private string _strCSSBreadCrumbSub;
        private string _strCSSBreak;
        private string _strCSSContainerRoot;
        private string _strCSSContainerSub;
        private string _strCSSControl;
        private string _strCSSIcon;
        private string _strCSSIndicateChildRoot;
        private string _strCSSIndicateChildSub;
        private string _strCSSLeftSeparator;
        private string _strCSSLeftSeparatorBreadCrumb;
        private string _strCSSLeftSeparatorSelection;
        private string _strCSSNode;
        private string _strCSSNodeHover;
        private string _strCSSNodeHoverRoot;
        private string _strCSSNodeHoverSub;
        private string _strCSSNodeRoot;
        private string _strCSSNodeSelectedRoot;
        private string _strCSSNodeSelectedSub;
        private string _strCSSRightSeparator;
        private string _strCSSRightSeparatorBreadCrumb;
        private string _strCSSRightSeparatorSelection;
        private string _strCSSSeparator;
        private string _strControlAlignment;
        private string _strControlOrientation;
        private string _strEffectsDuration;
        private string _strEffectsShadowColor;
        private string _strEffectsShadowDirection;
        private string _strEffectsShadowStrength;
        private string _strEffectsStyle;
        private string _strEffectsTransition;
        private string _strForceCrawlerDisplay;
        private string _strForceDownLevel;
        private string _strIndicateChildImageExpandedRoot;
        private string _strIndicateChildImageExpandedSub;
        private string _strIndicateChildImageRoot;
        private string _strIndicateChildImageSub;
        private string _strIndicateChildren;
        private string _strLevel = "";
        private string _strMouseOutHideDelay;
        private string _strMouseOverAction;
        private string _strMouseOverDisplay;
        private string _strNodeLeftHTMLBreadCrumbRoot;
        private string _strNodeLeftHTMLBreadCrumbSub;
        private string _strNodeLeftHTMLRoot;
        private string _strNodeLeftHTMLSub;
        private string _strNodeRightHTMLBreadCrumbRoot;
        private string _strNodeRightHTMLBreadCrumbSub;
        private string _strNodeRightHTMLRoot;
        private string _strNodeRightHTMLSub;
        private string _strPathImage;
        private string _strPathSystemImage;
        private string _strPathSystemScript;
        private string _strProviderName = "";
        private string _strSeparatorHTML;
        private string _strSeparatorLeftHTML;
        private string _strSeparatorLeftHTMLActive;
        private string _strSeparatorLeftHTMLBreadCrumb;
        private string _strSeparatorRightHTML;
        private string _strSeparatorRightHTMLActive;
        private string _strSeparatorRightHTMLBreadCrumb;
        private string _strStyleBackColor;
        private string _strStyleBorderWidth;
        private string _strStyleControlHeight;
        private string _strStyleFontBold;
        private string _strStyleFontNames;
        private string _strStyleFontSize;
        private string _strStyleForeColor;
        private string _strStyleHighlightColor;
        private string _strStyleIconBackColor;
        private string _strStyleIconWidth;
        private string _strStyleNodeHeight;
        private string _strStyleSelectionBorderColor;
        private string _strStyleSelectionColor;
        private string _strStyleSelectionForeColor;
        private string _strToolTip = "";
        private string _strWorkImage;

        #endregion

        #region "Public Properties"
        //JH - 2/5/07 - support for custom attributes
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public List<CustomAttribute> CustomAttributes
        {
            get
            {
                return _objCustomAttributes;
            }
        }

        public bool ShowHiddenTabs { get; set; }

        public string ProviderName
        {
            get
            {
                return _strProviderName;
            }
            set
            {
                _strProviderName = value;
            }
        }

        protected NavigationProvider Control
        {
            get
            {
                return _objControl;
            }
        }

        public string Level
        {
            get
            {
                return _strLevel;
            }
            set
            {
                _strLevel = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return _strToolTip;
            }
            set
            {
                _strToolTip = value;
            }
        }

        public bool PopulateNodesFromClient
        {
            get
            {
                return _blnPopulateNodesFromClient;
            }
            set
            {
                _blnPopulateNodesFromClient = value;
            }
        }

        public int ExpandDepth
        {
            get
            {
                return _intExpandDepth;
            }
            set
            {
                _intExpandDepth = value;
            }
        }

        public int StartTabId
        {
            get
            {
                return _intStartTabId;
            }
            set
            {
                _intStartTabId = value;
            }
        }

        public string PathSystemImage
        {
            get
            {
                if (Control == null)
                {
                    return _strPathSystemImage;
                }
                else
                {
                    return Control.PathSystemImage;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strPathSystemImage = value;
                }
                else
                {
                    Control.PathSystemImage = value;
                }
            }
        }

        public string PathImage
        {
            get
            {
                if (Control == null)
                {
                    return _strPathImage;
                }
                else
                {
                    return Control.PathImage;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strPathImage = value;
                }
                else
                {
                    Control.PathImage = value;
                }
            }
        }

        public string WorkImage
        {
            get
            {
                if (Control == null)
                {
                    return _strWorkImage;
                }
                else
                {
                    return Control.WorkImage;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strWorkImage = value;
                }
                else
                {
                    Control.WorkImage = value;
                }
            }
        }

        public string PathSystemScript
        {
            get
            {
                if (Control == null)
                {
                    return _strPathSystemScript;
                }
                else
                {
                    return Control.PathSystemScript;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strPathSystemScript = value;
                }
                else
                {
                    Control.PathSystemScript = value;
                }
            }
        }

        public string ControlOrientation
        {
            get
            {
                string retValue = "";
                if (Control == null)
                {
                    retValue = _strControlOrientation;
                }
                else
                {
                    switch (Control.ControlOrientation)
                    {
                        case NavigationProvider.Orientation.Horizontal:
                            retValue = "Horizontal";
                            break;
                        case NavigationProvider.Orientation.Vertical:
                            retValue = "Vertical";
                            break;
                    }
                }
                return retValue;
            }
            set
            {
                if (Control == null)
                {
                    _strControlOrientation = value;
                }
                else
                {
                    switch (value.ToLower())
                    {
                        case "horizontal":
                            Control.ControlOrientation = NavigationProvider.Orientation.Horizontal;
                            break;
                        case "vertical":
                            Control.ControlOrientation = NavigationProvider.Orientation.Vertical;
                            break;
                    }
                }
            }
        }

        public string ControlAlignment
        {
            get
            {
                string retValue = "";
                if (Control == null)
                {
                    retValue = _strControlAlignment;
                }
                else
                {
                    switch (Control.ControlAlignment)
                    {
                        case NavigationProvider.Alignment.Left:
                            retValue = "Left";
                            break;
                        case NavigationProvider.Alignment.Right:
                            retValue = "Right";
                            break;
                        case NavigationProvider.Alignment.Center:
                            retValue = "Center";
                            break;
                        case NavigationProvider.Alignment.Justify:
                            retValue = "Justify";
                            break;
                    }
                }
                return retValue;
            }
            set
            {
                if (Control == null)
                {
                    _strControlAlignment = value;
                }
                else
                {
                    switch (value.ToLower())
                    {
                        case "left":
                            Control.ControlAlignment = NavigationProvider.Alignment.Left;
                            break;
                        case "right":
                            Control.ControlAlignment = NavigationProvider.Alignment.Right;
                            break;
                        case "center":
                            Control.ControlAlignment = NavigationProvider.Alignment.Center;
                            break;
                        case "justify":
                            Control.ControlAlignment = NavigationProvider.Alignment.Justify;
                            break;
                    }
                }
            }
        }

        public string ForceCrawlerDisplay
        {
            get
            {
                if (Control == null)
                {
                    return _strForceCrawlerDisplay;
                }
                else
                {
                    return Control.ForceCrawlerDisplay;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strForceCrawlerDisplay = value;
                }
                else
                {
                    Control.ForceCrawlerDisplay = value;
                }
            }
        }

        public string ForceDownLevel
        {
            get
            {
                if (Control == null)
                {
                    return _strForceDownLevel;
                }
                else
                {
                    return Control.ForceDownLevel;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strForceDownLevel = value;
                }
                else
                {
                    Control.ForceDownLevel = value;
                }
            }
        }

        public string MouseOutHideDelay
        {
            get
            {
                if (Control == null)
                {
                    return _strMouseOutHideDelay;
                }
                else
                {
                    return Control.MouseOutHideDelay.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strMouseOutHideDelay = value;
                }
                else
                {
                    Control.MouseOutHideDelay = Convert.ToDecimal(value);
                }
            }
        }

        public string MouseOverDisplay
        {
            get
            {
                string retValue = "";
                if (Control == null)
                {
                    retValue = _strMouseOverDisplay;
                }
                else
                {
                    switch (Control.MouseOverDisplay)
                    {
                        case NavigationProvider.HoverDisplay.Highlight:
                            retValue = "Highlight";
                            break;
                        case NavigationProvider.HoverDisplay.None:
                            retValue = "None";
                            break;
                        case NavigationProvider.HoverDisplay.Outset:
                            retValue = "Outset";
                            break;
                    }
                }
                return retValue;
            }
            set
            {
                if (Control == null)
                {
                    _strMouseOverDisplay = value;
                }
                else
                {
                    switch (value.ToLower())
                    {
                        case "highlight":
                            Control.MouseOverDisplay = NavigationProvider.HoverDisplay.Highlight;
                            break;
                        case "outset":
                            Control.MouseOverDisplay = NavigationProvider.HoverDisplay.Outset;
                            break;
                        case "none":
                            Control.MouseOverDisplay = NavigationProvider.HoverDisplay.None;
                            break;
                    }
                }
            }
        }

        public string MouseOverAction
        {
            get
            {
                string retValue = "";
                if (Control == null)
                {
                    retValue = _strMouseOverAction;
                }
                else
                {
                    switch (Control.MouseOverAction)
                    {
                        case NavigationProvider.HoverAction.Expand:
                            retValue = "True";
                            break;
                        case NavigationProvider.HoverAction.None:
                            retValue = "False";
                            break;
                    }
                }
                return retValue;
            }
            set
            {
                if (Control == null)
                {
                    _strMouseOverAction = value;
                }
                else
                {
                    if (Convert.ToBoolean(GetValue(value, "True")))
                    {
                        Control.MouseOverAction = NavigationProvider.HoverAction.Expand;
                    }
                    else
                    {
                        Control.MouseOverAction = NavigationProvider.HoverAction.None;
                    }
                }
            }
        }

        public string IndicateChildren
        {
            get
            {
                if (Control == null)
                {
                    return _strIndicateChildren;
                }
                else
                {
                    return Control.IndicateChildren.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strIndicateChildren = value;
                }
                else
                {
                    Control.IndicateChildren = Convert.ToBoolean(value);
                }
            }
        }

        public string IndicateChildImageRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strIndicateChildImageRoot;
                }
                else
                {
                    return Control.IndicateChildImageRoot;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strIndicateChildImageRoot = value;
                }
                else
                {
                    Control.IndicateChildImageRoot = value;
                }
            }
        }

        public string IndicateChildImageSub
        {
            get
            {
                if (Control == null)
                {
                    return _strIndicateChildImageSub;
                }
                else
                {
                    return Control.IndicateChildImageSub;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strIndicateChildImageSub = value;
                }
                else
                {
                    Control.IndicateChildImageSub = value;
                }
            }
        }

        public string IndicateChildImageExpandedRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strIndicateChildImageExpandedRoot;
                }
                else
                {
                    return Control.IndicateChildImageExpandedRoot;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strIndicateChildImageExpandedRoot = value;
                }
                else
                {
                    Control.IndicateChildImageExpandedRoot = value;
                }
            }
        }

        public string IndicateChildImageExpandedSub
        {
            get
            {
                if (Control == null)
                {
                    return _strIndicateChildImageExpandedSub;
                }
                else
                {
                    return Control.IndicateChildImageExpandedSub;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strIndicateChildImageExpandedSub = value;
                }
                else
                {
                    Control.IndicateChildImageExpandedSub = value;
                }
            }
        }

        public string NodeLeftHTMLRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeLeftHTMLRoot;
                }
                else
                {
                    return Control.NodeLeftHTMLRoot;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeLeftHTMLRoot = value;
                }
                else
                {
                    Control.NodeLeftHTMLRoot = value;
                }
            }
        }

        public string NodeRightHTMLRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeRightHTMLRoot;
                }
                else
                {
                    return Control.NodeRightHTMLRoot;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeRightHTMLRoot = value;
                }
                else
                {
                    Control.NodeRightHTMLRoot = value;
                }
            }
        }

        public string NodeLeftHTMLSub
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeLeftHTMLSub;
                }
                else
                {
                    return Control.NodeLeftHTMLSub;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeLeftHTMLSub = value;
                }
                else
                {
                    Control.NodeLeftHTMLSub = value;
                }
            }
        }

        public string NodeRightHTMLSub
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeRightHTMLSub;
                }
                else
                {
                    return Control.NodeRightHTMLSub;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeRightHTMLSub = value;
                }
                else
                {
                    Control.NodeRightHTMLSub = value;
                }
            }
        }

        public string NodeLeftHTMLBreadCrumbRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeLeftHTMLBreadCrumbRoot;
                }
                else
                {
                    return Control.NodeLeftHTMLBreadCrumbRoot;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeLeftHTMLBreadCrumbRoot = value;
                }
                else
                {
                    Control.NodeLeftHTMLBreadCrumbRoot = value;
                }
            }
        }

        public string NodeLeftHTMLBreadCrumbSub
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeLeftHTMLBreadCrumbSub;
                }
                else
                {
                    return Control.NodeLeftHTMLBreadCrumbSub;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeLeftHTMLBreadCrumbSub = value;
                }
                else
                {
                    Control.NodeLeftHTMLBreadCrumbSub = value;
                }
            }
        }

        public string NodeRightHTMLBreadCrumbRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeRightHTMLBreadCrumbRoot;
                }
                else
                {
                    return Control.NodeRightHTMLBreadCrumbRoot;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeRightHTMLBreadCrumbRoot = value;
                }
                else
                {
                    Control.NodeRightHTMLBreadCrumbRoot = value;
                }
            }
        }

        public string NodeRightHTMLBreadCrumbSub
        {
            get
            {
                if (Control == null)
                {
                    return _strNodeRightHTMLBreadCrumbSub;
                }
                else
                {
                    return Control.NodeRightHTMLBreadCrumbSub;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strNodeRightHTMLBreadCrumbSub = value;
                }
                else
                {
                    Control.NodeRightHTMLBreadCrumbSub = value;
                }
            }
        }

        public string SeparatorHTML
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorHTML;
                }
                else
                {
                    return Control.SeparatorHTML;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorHTML = value;
                }
                else
                {
                    Control.SeparatorHTML = value;
                }
            }
        }

        public string SeparatorLeftHTML
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorLeftHTML;
                }
                else
                {
                    return Control.SeparatorLeftHTML;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorLeftHTML = value;
                }
                else
                {
                    Control.SeparatorLeftHTML = value;
                }
            }
        }

        public string SeparatorRightHTML
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorRightHTML;
                }
                else
                {
                    return Control.SeparatorRightHTML;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorRightHTML = value;
                }
                else
                {
                    Control.SeparatorRightHTML = value;
                }
            }
        }

        public string SeparatorLeftHTMLActive
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorLeftHTMLActive;
                }
                else
                {
                    return Control.SeparatorLeftHTMLActive;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorLeftHTMLActive = value;
                }
                else
                {
                    Control.SeparatorLeftHTMLActive = value;
                }
            }
        }

        public string SeparatorRightHTMLActive
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorRightHTMLActive;
                }
                else
                {
                    return Control.SeparatorRightHTMLActive;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorRightHTMLActive = value;
                }
                else
                {
                    Control.SeparatorRightHTMLActive = value;
                }
            }
        }

        public string SeparatorLeftHTMLBreadCrumb
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorLeftHTMLBreadCrumb;
                }
                else
                {
                    return Control.SeparatorLeftHTMLBreadCrumb;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorLeftHTMLBreadCrumb = value;
                }
                else
                {
                    Control.SeparatorLeftHTMLBreadCrumb = value;
                }
            }
        }

        public string SeparatorRightHTMLBreadCrumb
        {
            get
            {
                if (Control == null)
                {
                    return _strSeparatorRightHTMLBreadCrumb;
                }
                else
                {
                    return Control.SeparatorRightHTMLBreadCrumb;
                }
            }
            set
            {
                value = GetPath(value);
                if (Control == null)
                {
                    _strSeparatorRightHTMLBreadCrumb = value;
                }
                else
                {
                    Control.SeparatorRightHTMLBreadCrumb = value;
                }
            }
        }

        public string CSSControl
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSControl;
                }
                else
                {
                    return Control.CSSControl;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSControl = value;
                }
                else
                {
                    Control.CSSControl = value;
                }
            }
        }

        public string CSSContainerRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSContainerRoot;
                }
                else
                {
                    return Control.CSSContainerRoot;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSContainerRoot = value;
                }
                else
                {
                    Control.CSSContainerRoot = value;
                }
            }
        }

        public string CSSNode
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNode;
                }
                else
                {
                    return Control.CSSNode;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNode = value;
                }
                else
                {
                    Control.CSSNode = value;
                }
            }
        }

        public string CSSIcon
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSIcon;
                }
                else
                {
                    return Control.CSSIcon;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSIcon = value;
                }
                else
                {
                    Control.CSSIcon = value;
                }
            }
        }

        public string CSSContainerSub
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSContainerSub;
                }
                else
                {
                    return Control.CSSContainerSub;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSContainerSub = value;
                }
                else
                {
                    Control.CSSContainerSub = value;
                }
            }
        }

        public string CSSNodeHover
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNodeHover;
                }
                else
                {
                    return Control.CSSNodeHover;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNodeHover = value;
                }
                else
                {
                    Control.CSSNodeHover = value;
                }
            }
        }

        public string CSSBreak
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSBreak;
                }
                else
                {
                    return Control.CSSBreak;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSBreak = value;
                }
                else
                {
                    Control.CSSBreak = value;
                }
            }
        }

        public string CSSIndicateChildSub
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSIndicateChildSub;
                }
                else
                {
                    return Control.CSSIndicateChildSub;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSIndicateChildSub = value;
                }
                else
                {
                    Control.CSSIndicateChildSub = value;
                }
            }
        }

        public string CSSIndicateChildRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSIndicateChildRoot;
                }
                else
                {
                    return Control.CSSIndicateChildRoot;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSIndicateChildRoot = value;
                }
                else
                {
                    Control.CSSIndicateChildRoot = value;
                }
            }
        }

        public string CSSBreadCrumbRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSBreadCrumbRoot;
                }
                else
                {
                    return Control.CSSBreadCrumbRoot;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSBreadCrumbRoot = value;
                }
                else
                {
                    Control.CSSBreadCrumbRoot = value;
                }
            }
        }

        public string CSSBreadCrumbSub
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSBreadCrumbSub;
                }
                else
                {
                    return Control.CSSBreadCrumbSub;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSBreadCrumbSub = value;
                }
                else
                {
                    Control.CSSBreadCrumbSub = value;
                }
            }
        }

        public string CSSNodeRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNodeRoot;
                }
                else
                {
                    return Control.CSSNodeRoot;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNodeRoot = value;
                }
                else
                {
                    Control.CSSNodeRoot = value;
                }
            }
        }

        public string CSSNodeSelectedRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNodeSelectedRoot;
                }
                else
                {
                    return Control.CSSNodeSelectedRoot;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNodeSelectedRoot = value;
                }
                else
                {
                    Control.CSSNodeSelectedRoot = value;
                }
            }
        }

        public string CSSNodeSelectedSub
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNodeSelectedSub;
                }
                else
                {
                    return Control.CSSNodeSelectedSub;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNodeSelectedSub = value;
                }
                else
                {
                    Control.CSSNodeSelectedSub = value;
                }
            }
        }

        public string CSSNodeHoverRoot
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNodeHoverRoot;
                }
                else
                {
                    return Control.CSSNodeHoverRoot;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNodeHoverRoot = value;
                }
                else
                {
                    Control.CSSNodeHoverRoot = value;
                }
            }
        }

        public string CSSNodeHoverSub
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSNodeHoverSub;
                }
                else
                {
                    return Control.CSSNodeHoverSub;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSNodeHoverSub = value;
                }
                else
                {
                    Control.CSSNodeHoverSub = value;
                }
            }
        }

        public string CSSSeparator
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSSeparator;
                }
                else
                {
                    return Control.CSSSeparator;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSSeparator = value;
                }
                else
                {
                    Control.CSSSeparator = value;
                }
            }
        }

        public string CSSLeftSeparator
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSLeftSeparator;
                }
                else
                {
                    return Control.CSSLeftSeparator;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSLeftSeparator = value;
                }
                else
                {
                    Control.CSSLeftSeparator = value;
                }
            }
        }

        public string CSSRightSeparator
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSRightSeparator;
                }
                else
                {
                    return Control.CSSRightSeparator;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSRightSeparator = value;
                }
                else
                {
                    Control.CSSRightSeparator = value;
                }
            }
        }

        public string CSSLeftSeparatorSelection
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSLeftSeparatorSelection;
                }
                else
                {
                    return Control.CSSLeftSeparatorSelection;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSLeftSeparatorSelection = value;
                }
                else
                {
                    Control.CSSLeftSeparatorSelection = value;
                }
            }
        }

        public string CSSRightSeparatorSelection
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSRightSeparatorSelection;
                }
                else
                {
                    return Control.CSSRightSeparatorSelection;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSRightSeparatorSelection = value;
                }
                else
                {
                    Control.CSSRightSeparatorSelection = value;
                }
            }
        }

        public string CSSLeftSeparatorBreadCrumb
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSLeftSeparatorBreadCrumb;
                }
                else
                {
                    return Control.CSSLeftSeparatorBreadCrumb;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSLeftSeparatorBreadCrumb = value;
                }
                else
                {
                    Control.CSSLeftSeparatorBreadCrumb = value;
                }
            }
        }

        public string CSSRightSeparatorBreadCrumb
        {
            get
            {
                if (Control == null)
                {
                    return _strCSSRightSeparatorBreadCrumb;
                }
                else
                {
                    return Control.CSSRightSeparatorBreadCrumb;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strCSSRightSeparatorBreadCrumb = value;
                }
                else
                {
                    Control.CSSRightSeparatorBreadCrumb = value;
                }
            }
        }

        public string StyleBackColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleBackColor;
                }
                else
                {
                    return Control.StyleBackColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleBackColor = value;
                }
                else
                {
                    Control.StyleBackColor = value;
                }
            }
        }

        public string StyleForeColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleForeColor;
                }
                else
                {
                    return Control.StyleForeColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleForeColor = value;
                }
                else
                {
                    Control.StyleForeColor = value;
                }
            }
        }

        public string StyleHighlightColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleHighlightColor;
                }
                else
                {
                    return Control.StyleHighlightColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleHighlightColor = value;
                }
                else
                {
                    Control.StyleHighlightColor = value;
                }
            }
        }

        public string StyleIconBackColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleIconBackColor;
                }
                else
                {
                    return Control.StyleIconBackColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleIconBackColor = value;
                }
                else
                {
                    Control.StyleIconBackColor = value;
                }
            }
        }

        public string StyleSelectionBorderColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleSelectionBorderColor;
                }
                else
                {
                    return Control.StyleSelectionBorderColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleSelectionBorderColor = value;
                }
                else
                {
                    Control.StyleSelectionBorderColor = value;
                }
            }
        }

        public string StyleSelectionColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleSelectionColor;
                }
                else
                {
                    return Control.StyleSelectionColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleSelectionColor = value;
                }
                else
                {
                    Control.StyleSelectionColor = value;
                }
            }
        }

        public string StyleSelectionForeColor
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleSelectionForeColor;
                }
                else
                {
                    return Control.StyleSelectionForeColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleSelectionForeColor = value;
                }
                else
                {
                    Control.StyleSelectionForeColor = value;
                }
            }
        }

        public string StyleControlHeight
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleControlHeight;
                }
                else
                {
                    return Control.StyleControlHeight.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleControlHeight = value;
                }
                else
                {
                    Control.StyleControlHeight = Convert.ToDecimal(value);
                }
            }
        }

        public string StyleBorderWidth
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleBorderWidth;
                }
                else
                {
                    return Control.StyleBorderWidth.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleBorderWidth = value;
                }
                else
                {
                    Control.StyleBorderWidth = Convert.ToDecimal(value);
                }
            }
        }

        public string StyleNodeHeight
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleNodeHeight;
                }
                else
                {
                    return Control.StyleNodeHeight.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleNodeHeight = value;
                }
                else
                {
                    Control.StyleNodeHeight = Convert.ToDecimal(value);
                }
            }
        }

        public string StyleIconWidth
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleIconWidth;
                }
                else
                {
                    return Control.StyleIconWidth.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleIconWidth = value;
                }
                else
                {
                    Control.StyleIconWidth = Convert.ToDecimal(value);
                }
            }
        }

        public string StyleFontNames
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleFontNames;
                }
                else
                {
                    return Control.StyleFontNames;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleFontNames = value;
                }
                else
                {
                    Control.StyleFontNames = value;
                }
            }
        }

        public string StyleFontSize
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleFontSize;
                }
                else
                {
                    return Control.StyleFontSize.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleFontSize = value;
                }
                else
                {
                    Control.StyleFontSize = Convert.ToDecimal(value);
                }
            }
        }

        public string StyleFontBold
        {
            get
            {
                if (Control == null)
                {
                    return _strStyleFontBold;
                }
                else
                {
                    return Control.StyleFontBold;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strStyleFontBold = value;
                }
                else
                {
                    Control.StyleFontBold = value;
                }
            }
        }

        public string EffectsShadowColor
        {
            get
            {
                if (Control == null)
                {
                    return _strEffectsShadowColor;
                }
                else
                {
                    return Control.EffectsShadowColor;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strEffectsShadowColor = value;
                }
                else
                {
                    Control.EffectsShadowColor = value;
                }
            }
        }

        public string EffectsStyle
        {
            get
            {
                if (Control == null)
                {
                    return _strEffectsStyle;
                }
                else
                {
                    return Control.EffectsStyle;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strEffectsStyle = value;
                }
                else
                {
                    Control.EffectsStyle = value;
                }
            }
        }

        public string EffectsShadowStrength
        {
            get
            {
                if (Control == null)
                {
                    return _strEffectsShadowStrength;
                }
                else
                {
                    return Control.EffectsShadowStrength.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strEffectsShadowStrength = value;
                }
                else
                {
                    Control.EffectsShadowStrength = Convert.ToInt32(value);
                }
            }
        }

        public string EffectsTransition
        {
            get
            {
                if (Control == null)
                {
                    return _strEffectsTransition;
                }
                else
                {
                    return Control.EffectsTransition;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strEffectsTransition = value;
                }
                else
                {
                    Control.EffectsTransition = value;
                }
            }
        }

        public string EffectsDuration
        {
            get
            {
                if (Control == null)
                {
                    return _strEffectsDuration;
                }
                else
                {
                    return Control.EffectsDuration.ToString();
                }
            }
            set
            {
                if (Control == null)
                {
                    _strEffectsDuration = value;
                }
                else
                {
                    Control.EffectsDuration = Convert.ToDouble(value);
                }
            }
        }

        public string EffectsShadowDirection
        {
            get
            {
                if (Control == null)
                {
                    return _strEffectsShadowDirection;
                }
                else
                {
                    return Control.EffectsShadowDirection;
                }
            }
            set
            {
                if (Control == null)
                {
                    _strEffectsShadowDirection = value;
                }
                else
                {
                    Control.EffectsShadowDirection = value;
                }
            }
        }

        #endregion

        #region "Public Methods"

        public DNNNodeCollection GetNavigationNodes(DNNNode objNode)
        {
            int intRootParent = PortalSettings.ActiveTab.TabID;
            DNNNodeCollection objNodes = null;
            Navigation.ToolTipSource eToolTips;
            int intNavNodeOptions = 0;
            int intDepth = ExpandDepth;
            switch (Level.ToLower())
            {
                case "child":
                    break;
                case "parent":
                    intNavNodeOptions = (int)Navigation.NavNodeOptions.IncludeParent + (int)Navigation.NavNodeOptions.IncludeSelf;
                    break;
                case "same":
                    intNavNodeOptions = (int)Navigation.NavNodeOptions.IncludeSiblings + (int)Navigation.NavNodeOptions.IncludeSelf;
                    break;
                default:
                    intRootParent = -1;
                    intNavNodeOptions = (int)Navigation.NavNodeOptions.IncludeSiblings + (int)Navigation.NavNodeOptions.IncludeSelf;
                    break;
            }

            if (ShowHiddenTabs) intNavNodeOptions += (int)Navigation.NavNodeOptions.IncludeHiddenNodes;

            switch (ToolTip.ToLower())
            {
                case "name":
                    eToolTips = Navigation.ToolTipSource.TabName;
                    break;
                case "title":
                    eToolTips = Navigation.ToolTipSource.Title;
                    break;
                case "description":
                    eToolTips = Navigation.ToolTipSource.Description;
                    break;
                default:
                    eToolTips = Navigation.ToolTipSource.None;
                    break;
            }
            if (PopulateNodesFromClient && Control.SupportsPopulateOnDemand)
            {
                intNavNodeOptions += (int)Navigation.NavNodeOptions.MarkPendingNodes;
            }
            if (PopulateNodesFromClient && Control.SupportsPopulateOnDemand == false)
            {
                ExpandDepth = -1;
            }
            if (StartTabId != -1)
            {
                intRootParent = StartTabId;
            }
            if (objNode != null)
            {
                intRootParent = Convert.ToInt32(objNode.ID);
                intNavNodeOptions = (int)Navigation.NavNodeOptions.MarkPendingNodes;
                objNodes = Navigation.GetNavigationNodes(objNode, eToolTips, intRootParent, intDepth, intNavNodeOptions);
            }
            else
            {
                objNodes = Navigation.GetNavigationNodes(Control.ClientID, eToolTips, intRootParent, intDepth, intNavNodeOptions);
            }
            return objNodes;
        }

        #endregion

        #region "Protected Methods"

        protected string GetValue(string strVal, string strDefault)
        {
            if (String.IsNullOrEmpty(strVal))
            {
                return strDefault;
            }
            else
            {
                return strVal;
            }
        }

        protected void InitializeNavControl(Control objParent, string strDefaultProvider)
        {
            if (String.IsNullOrEmpty(ProviderName))
            {
                ProviderName = strDefaultProvider;
            }
            _objControl = NavigationProvider.Instance(ProviderName);
            Control.ControlID = "ctl" + ID;
            Control.Initialize();
            AssignControlProperties();
            objParent.Controls.Add(Control.NavigationControl);
        }

        #endregion

        #region "Private Methods"

        private void AssignControlProperties()
        {
            if (!String.IsNullOrEmpty(_strPathSystemImage))
            {
                Control.PathSystemImage = _strPathSystemImage;
            }
            if (!String.IsNullOrEmpty(_strPathImage))
            {
                Control.PathImage = _strPathImage;
            }
            if (!String.IsNullOrEmpty(_strPathSystemScript))
            {
                Control.PathSystemScript = _strPathSystemScript;
            }
            if (!String.IsNullOrEmpty(_strWorkImage))
            {
                Control.WorkImage = _strWorkImage;
            }
            if (!String.IsNullOrEmpty(_strControlOrientation))
            {
                switch (_strControlOrientation.ToLower())
                {
                    case "horizontal":
                        Control.ControlOrientation = NavigationProvider.Orientation.Horizontal;
                        break;
                    case "vertical":
                        Control.ControlOrientation = NavigationProvider.Orientation.Vertical;
                        break;
                }
            }
            if (!String.IsNullOrEmpty(_strControlAlignment))
            {
                switch (_strControlAlignment.ToLower())
                {
                    case "left":
                        Control.ControlAlignment = NavigationProvider.Alignment.Left;
                        break;
                    case "right":
                        Control.ControlAlignment = NavigationProvider.Alignment.Right;
                        break;
                    case "center":
                        Control.ControlAlignment = NavigationProvider.Alignment.Center;
                        break;
                    case "justify":
                        Control.ControlAlignment = NavigationProvider.Alignment.Justify;
                        break;
                }
            }
            Control.ForceCrawlerDisplay = GetValue(_strForceCrawlerDisplay, "False");
            Control.ForceDownLevel = GetValue(_strForceDownLevel, "False");
            if (!String.IsNullOrEmpty(_strMouseOutHideDelay))
            {
                Control.MouseOutHideDelay = Convert.ToDecimal(_strMouseOutHideDelay);
            }
            if (!String.IsNullOrEmpty(_strMouseOverDisplay))
            {
                switch (_strMouseOverDisplay.ToLower())
                {
                    case "highlight":
                        Control.MouseOverDisplay = NavigationProvider.HoverDisplay.Highlight;
                        break;
                    case "outset":
                        Control.MouseOverDisplay = NavigationProvider.HoverDisplay.Outset;
                        break;
                    case "none":
                        Control.MouseOverDisplay = NavigationProvider.HoverDisplay.None;
                        break;
                }
            }
            if (Convert.ToBoolean(GetValue(_strMouseOverAction, "True")))
            {
                Control.MouseOverAction = NavigationProvider.HoverAction.Expand;
            }
            else
            {
                Control.MouseOverAction = NavigationProvider.HoverAction.None;
            }
            Control.IndicateChildren = Convert.ToBoolean(GetValue(_strIndicateChildren, "True"));
            if (!String.IsNullOrEmpty(_strIndicateChildImageRoot))
            {
                Control.IndicateChildImageRoot = _strIndicateChildImageRoot;
            }
            if (!String.IsNullOrEmpty(_strIndicateChildImageSub))
            {
                Control.IndicateChildImageSub = _strIndicateChildImageSub;
            }
            if (!String.IsNullOrEmpty(_strIndicateChildImageExpandedRoot))
            {
                Control.IndicateChildImageExpandedRoot = _strIndicateChildImageExpandedRoot;
            }
            if (!String.IsNullOrEmpty(_strIndicateChildImageExpandedSub))
            {
                Control.IndicateChildImageExpandedSub = _strIndicateChildImageExpandedSub;
            }
            if (!String.IsNullOrEmpty(_strNodeLeftHTMLRoot))
            {
                Control.NodeLeftHTMLRoot = _strNodeLeftHTMLRoot;
            }
            if (!String.IsNullOrEmpty(_strNodeRightHTMLRoot))
            {
                Control.NodeRightHTMLRoot = _strNodeRightHTMLRoot;
            }
            if (!String.IsNullOrEmpty(_strNodeLeftHTMLSub))
            {
                Control.NodeLeftHTMLSub = _strNodeLeftHTMLSub;
            }
            if (!String.IsNullOrEmpty(_strNodeRightHTMLSub))
            {
                Control.NodeRightHTMLSub = _strNodeRightHTMLSub;
            }
            if (!String.IsNullOrEmpty(_strNodeLeftHTMLBreadCrumbRoot))
            {
                Control.NodeLeftHTMLBreadCrumbRoot = _strNodeLeftHTMLBreadCrumbRoot;
            }
            if (!String.IsNullOrEmpty(_strNodeLeftHTMLBreadCrumbSub))
            {
                Control.NodeLeftHTMLBreadCrumbSub = _strNodeLeftHTMLBreadCrumbSub;
            }
            if (!String.IsNullOrEmpty(_strNodeRightHTMLBreadCrumbRoot))
            {
                Control.NodeRightHTMLBreadCrumbRoot = _strNodeRightHTMLBreadCrumbRoot;
            }
            if (!String.IsNullOrEmpty(_strNodeRightHTMLBreadCrumbSub))
            {
                Control.NodeRightHTMLBreadCrumbSub = _strNodeRightHTMLBreadCrumbSub;
            }
            if (!String.IsNullOrEmpty(_strSeparatorHTML))
            {
                Control.SeparatorHTML = _strSeparatorHTML;
            }
            if (!String.IsNullOrEmpty(_strSeparatorLeftHTML))
            {
                Control.SeparatorLeftHTML = _strSeparatorLeftHTML;
            }
            if (!String.IsNullOrEmpty(_strSeparatorRightHTML))
            {
                Control.SeparatorRightHTML = _strSeparatorRightHTML;
            }
            if (!String.IsNullOrEmpty(_strSeparatorLeftHTMLActive))
            {
                Control.SeparatorLeftHTMLActive = _strSeparatorLeftHTMLActive;
            }
            if (!String.IsNullOrEmpty(_strSeparatorRightHTMLActive))
            {
                Control.SeparatorRightHTMLActive = _strSeparatorRightHTMLActive;
            }
            if (!String.IsNullOrEmpty(_strSeparatorLeftHTMLBreadCrumb))
            {
                Control.SeparatorLeftHTMLBreadCrumb = _strSeparatorLeftHTMLBreadCrumb;
            }
            if (!String.IsNullOrEmpty(_strSeparatorRightHTMLBreadCrumb))
            {
                Control.SeparatorRightHTMLBreadCrumb = _strSeparatorRightHTMLBreadCrumb;
            }
            if (!String.IsNullOrEmpty(_strCSSControl))
            {
                Control.CSSControl = _strCSSControl;
            }
            if (!String.IsNullOrEmpty(_strCSSContainerRoot))
            {
                Control.CSSContainerRoot = _strCSSContainerRoot;
            }
            if (!String.IsNullOrEmpty(_strCSSNode))
            {
                Control.CSSNode = _strCSSNode;
            }
            if (!String.IsNullOrEmpty(_strCSSIcon))
            {
                Control.CSSIcon = _strCSSIcon;
            }
            if (!String.IsNullOrEmpty(_strCSSContainerSub))
            {
                Control.CSSContainerSub = _strCSSContainerSub;
            }
            if (!String.IsNullOrEmpty(_strCSSNodeHover))
            {
                Control.CSSNodeHover = _strCSSNodeHover;
            }
            if (!String.IsNullOrEmpty(_strCSSBreak))
            {
                Control.CSSBreak = _strCSSBreak;
            }
            if (!String.IsNullOrEmpty(_strCSSIndicateChildSub))
            {
                Control.CSSIndicateChildSub = _strCSSIndicateChildSub;
            }
            if (!String.IsNullOrEmpty(_strCSSIndicateChildRoot))
            {
                Control.CSSIndicateChildRoot = _strCSSIndicateChildRoot;
            }
            if (!String.IsNullOrEmpty(_strCSSBreadCrumbRoot))
            {
                Control.CSSBreadCrumbRoot = _strCSSBreadCrumbRoot;
            }
            if (!String.IsNullOrEmpty(_strCSSBreadCrumbSub))
            {
                Control.CSSBreadCrumbSub = _strCSSBreadCrumbSub;
            }
            if (!String.IsNullOrEmpty(_strCSSNodeRoot))
            {
                Control.CSSNodeRoot = _strCSSNodeRoot;
            }
            if (!String.IsNullOrEmpty(_strCSSNodeSelectedRoot))
            {
                Control.CSSNodeSelectedRoot = _strCSSNodeSelectedRoot;
            }
            if (!String.IsNullOrEmpty(_strCSSNodeSelectedSub))
            {
                Control.CSSNodeSelectedSub = _strCSSNodeSelectedSub;
            }
            if (!String.IsNullOrEmpty(_strCSSNodeHoverRoot))
            {
                Control.CSSNodeHoverRoot = _strCSSNodeHoverRoot;
            }
            if (!String.IsNullOrEmpty(_strCSSNodeHoverSub))
            {
                Control.CSSNodeHoverSub = _strCSSNodeHoverSub;
            }
            if (!String.IsNullOrEmpty(_strCSSSeparator))
            {
                Control.CSSSeparator = _strCSSSeparator;
            }
            if (!String.IsNullOrEmpty(_strCSSLeftSeparator))
            {
                Control.CSSLeftSeparator = _strCSSLeftSeparator;
            }
            if (!String.IsNullOrEmpty(_strCSSRightSeparator))
            {
                Control.CSSRightSeparator = _strCSSRightSeparator;
            }
            if (!String.IsNullOrEmpty(_strCSSLeftSeparatorSelection))
            {
                Control.CSSLeftSeparatorSelection = _strCSSLeftSeparatorSelection;
            }
            if (!String.IsNullOrEmpty(_strCSSRightSeparatorSelection))
            {
                Control.CSSRightSeparatorSelection = _strCSSRightSeparatorSelection;
            }
            if (!String.IsNullOrEmpty(_strCSSLeftSeparatorBreadCrumb))
            {
                Control.CSSLeftSeparatorBreadCrumb = _strCSSLeftSeparatorBreadCrumb;
            }
            if (!String.IsNullOrEmpty(_strCSSRightSeparatorBreadCrumb))
            {
                Control.CSSRightSeparatorBreadCrumb = _strCSSRightSeparatorBreadCrumb;
            }
            if (!String.IsNullOrEmpty(_strStyleBackColor))
            {
                Control.StyleBackColor = _strStyleBackColor;
            }
            if (!String.IsNullOrEmpty(_strStyleForeColor))
            {
                Control.StyleForeColor = _strStyleForeColor;
            }
            if (!String.IsNullOrEmpty(_strStyleHighlightColor))
            {
                Control.StyleHighlightColor = _strStyleHighlightColor;
            }
            if (!String.IsNullOrEmpty(_strStyleIconBackColor))
            {
                Control.StyleIconBackColor = _strStyleIconBackColor;
            }
            if (!String.IsNullOrEmpty(_strStyleSelectionBorderColor))
            {
                Control.StyleSelectionBorderColor = _strStyleSelectionBorderColor;
            }
            if (!String.IsNullOrEmpty(_strStyleSelectionColor))
            {
                Control.StyleSelectionColor = _strStyleSelectionColor;
            }
            if (!String.IsNullOrEmpty(_strStyleSelectionForeColor))
            {
                Control.StyleSelectionForeColor = _strStyleSelectionForeColor;
            }
            if (!String.IsNullOrEmpty(_strStyleControlHeight))
            {
                Control.StyleControlHeight = Convert.ToDecimal(_strStyleControlHeight);
            }
            if (!String.IsNullOrEmpty(_strStyleBorderWidth))
            {
                Control.StyleBorderWidth = Convert.ToDecimal(_strStyleBorderWidth);
            }
            if (!String.IsNullOrEmpty(_strStyleNodeHeight))
            {
                Control.StyleNodeHeight = Convert.ToDecimal(_strStyleNodeHeight);
            }
            if (!String.IsNullOrEmpty(_strStyleIconWidth))
            {
                Control.StyleIconWidth = Convert.ToDecimal(_strStyleIconWidth);
            }
            if (!String.IsNullOrEmpty(_strStyleFontNames))
            {
                Control.StyleFontNames = _strStyleFontNames;
            }
            if (!String.IsNullOrEmpty(_strStyleFontSize))
            {
                Control.StyleFontSize = Convert.ToDecimal(_strStyleFontSize);
            }
            if (!String.IsNullOrEmpty(_strStyleFontBold))
            {
                Control.StyleFontBold = _strStyleFontBold;
            }
            if (!String.IsNullOrEmpty(_strEffectsShadowColor))
            {
                Control.EffectsShadowColor = _strEffectsShadowColor;
            }
            if (!String.IsNullOrEmpty(_strEffectsStyle))
            {
                Control.EffectsStyle = _strEffectsStyle;
            }
            if (!String.IsNullOrEmpty(_strEffectsShadowStrength))
            {
                Control.EffectsShadowStrength = Convert.ToInt32(_strEffectsShadowStrength);
            }
            if (!String.IsNullOrEmpty(_strEffectsTransition))
            {
                Control.EffectsTransition = _strEffectsTransition;
            }
            if (!String.IsNullOrEmpty(_strEffectsDuration))
            {
                Control.EffectsDuration = Convert.ToDouble(_strEffectsDuration);
            }
            if (!String.IsNullOrEmpty(_strEffectsShadowDirection))
            {
                Control.EffectsShadowDirection = _strEffectsShadowDirection;
            }
            Control.CustomAttributes = CustomAttributes;
        }

        protected void Bind(DNNNodeCollection objNodes)
        {
            Control.Bind(objNodes);
        }

        private string GetPath(string strPath)
        {
            if (strPath.IndexOf("[SKINPATH]") > -1)
            {
                return strPath.Replace("[SKINPATH]", PortalSettings.ActiveTab.SkinPath);
            }
            else if (strPath.IndexOf("[APPIMAGEPATH]") > -1)
            {
                return strPath.Replace("[APPIMAGEPATH]", Globals.ApplicationPath + "/images/");
            }
            else if (strPath.IndexOf("[HOMEDIRECTORY]") > -1)
            {
                return strPath.Replace("[HOMEDIRECTORY]", PortalSettings.HomeDirectory);
            }
            else
            {
                if (strPath.StartsWith("~"))
                {
                    return ResolveUrl(strPath);
                }
            }
            return strPath;
        }

        #endregion
    }

    public class CustomAttribute
    {
        public string Name;
        public string Value;
    }
}