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
using System.Collections;
using System.Reflection;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Profile;
using DotNetNuke.Entities.Users;

#endregion
namespace DotNetNuke.UI.WebControls
{
    /// -----------------------------------------------------------------------------
    /// Project:    DotNetNuke
    /// Namespace:  DotNetNuke.UI.WebControls
    /// Class:      CollectionEditorInfoFactory
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The CollectionEditorInfoAdapter control provides an Adapter for Collection Onjects
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [cnurse]	05/08/2006	created
    /// </history>
    /// -----------------------------------------------------------------------------
    public class CollectionEditorInfoAdapter : IEditorInfoAdapter
    {
        private readonly object _dataSource;
        private readonly Hashtable _fieldNames;
        private readonly string _name;

        public CollectionEditorInfoAdapter(object dataSource, string name, string fieldName, Hashtable fieldNames)
        {
            _dataSource = dataSource;
            _fieldNames = fieldNames;
            _name = name;
        }

        #region IEditorInfoAdapter Members

        public EditorInfo CreateEditControl()
        {
            return GetEditorInfo();
        }

        public bool UpdateValue(PropertyEditorEventArgs e)
        {
            string NameDataField = Convert.ToString(_fieldNames["Name"]);
            string ValueDataField = Convert.ToString(_fieldNames["Value"]);
            PropertyInfo objProperty;
            string PropertyName = "";
            bool changed = e.Changed;
            string name = e.Name;
            object oldValue = e.OldValue;
            object newValue = e.Value;
            object stringValue = e.StringValue;
            bool _IsDirty = Null.NullBoolean;

            //Get the Name Property
            objProperty = _dataSource.GetType().GetProperty(NameDataField);
            if (objProperty != null)
            {
                PropertyName = Convert.ToString(objProperty.GetValue(_dataSource, null));
                //Do we have the item in the IEnumerable Collection being changed
                PropertyName = PropertyName.Replace(" ", "_");
                if (PropertyName == name)
                {
                    //Get the Value Property
                    objProperty = _dataSource.GetType().GetProperty(ValueDataField);

                    //Set the Value property to the new value
                    if ((!(ReferenceEquals(newValue, oldValue))) || changed)
                    {
                        if (objProperty.PropertyType.FullName == "System.String")
                        {
                            objProperty.SetValue(_dataSource, stringValue, null);
                        }
                        else
                        {
                            objProperty.SetValue(_dataSource, newValue, null);
                        }
                        _IsDirty = true;
                    }
                }
            }
            return _IsDirty;
        }

        public bool UpdateVisibility(PropertyEditorEventArgs e)
        {
            string nameDataField = Convert.ToString(_fieldNames["Name"]);
            string dataField = Convert.ToString(_fieldNames["ProfileVisibility"]);
            string name = e.Name;
            object newValue = e.Value;
            bool dirty = Null.NullBoolean;

            //Get the Name Property
            PropertyInfo property = _dataSource.GetType().GetProperty(nameDataField);
            if (property != null)
            {
                string propertyName = Convert.ToString(property.GetValue(_dataSource, null));
                //Do we have the item in the IEnumerable Collection being changed
                propertyName = propertyName.Replace(" ", "_");
                if (propertyName == name)
                {
                    //Get the Value Property
                    property = _dataSource.GetType().GetProperty(dataField);
                    //Set the Value property to the new value
                    property.SetValue(_dataSource, newValue, null);
                    dirty = true;
                }
            }
            return dirty;
        }

        #endregion

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetEditorInfo builds an EditorInfo object for a propoerty
        /// </summary>
        /// <history>
        /// 	[cnurse]	05/05/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        private EditorInfo GetEditorInfo()
        {
            string CategoryDataField = Convert.ToString(_fieldNames["Category"]);
            string EditorDataField = Convert.ToString(_fieldNames["Editor"]);
            string NameDataField = Convert.ToString(_fieldNames["Name"]);
            string RequiredDataField = Convert.ToString(_fieldNames["Required"]);
            string TypeDataField = Convert.ToString(_fieldNames["Type"]);
            string ValidationExpressionDataField = Convert.ToString(_fieldNames["ValidationExpression"]);
            string ValueDataField = Convert.ToString(_fieldNames["Value"]);
            string VisibilityDataField = Convert.ToString(_fieldNames["ProfileVisibility"]);
            string MaxLengthDataField = Convert.ToString(_fieldNames["Length"]);

            var editInfo = new EditorInfo();
            PropertyInfo property;

            //Get the Name of the property
            editInfo.Name = string.Empty;
            if (!String.IsNullOrEmpty(NameDataField))
            {
                property = _dataSource.GetType().GetProperty(NameDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.Name = Convert.ToString(property.GetValue(_dataSource, null));
                }
            }

            //Get the Category of the property
            editInfo.Category = string.Empty;

            //Get Category Field
            if (!String.IsNullOrEmpty(CategoryDataField))
            {
                property = _dataSource.GetType().GetProperty(CategoryDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.Category = Convert.ToString(property.GetValue(_dataSource, null));
                }
            }

            //Get Value Field
            editInfo.Value = string.Empty;
            if (!String.IsNullOrEmpty(ValueDataField))
            {
                property = _dataSource.GetType().GetProperty(ValueDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.Value = Convert.ToString(property.GetValue(_dataSource, null));
                }
            }

            //Get the type of the property
            editInfo.Type = "System.String";
            if (!String.IsNullOrEmpty(TypeDataField))
            {
                property = _dataSource.GetType().GetProperty(TypeDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.Type = Convert.ToString(property.GetValue(_dataSource, null));
                }
            }

            //Get Editor Field
            editInfo.Editor = "DotNetNuke.UI.WebControls.TextEditControl, DotNetNuke";
            if (!String.IsNullOrEmpty(EditorDataField))
            {
                property = _dataSource.GetType().GetProperty(EditorDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.Editor = EditorInfo.GetEditor(Convert.ToInt32(property.GetValue(_dataSource, null)));
                }
            }

            //Get LabelMode Field
            editInfo.LabelMode = LabelMode.Left;

            //Get Required Field
            editInfo.Required = false;
            if (!String.IsNullOrEmpty(RequiredDataField))
            {
                property = _dataSource.GetType().GetProperty(RequiredDataField);
                if (!((property == null) || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.Required = Convert.ToBoolean(property.GetValue(_dataSource, null));
                }
            }

            //Set ResourceKey Field
            editInfo.ResourceKey = editInfo.Name;
            editInfo.ResourceKey = string.Format("{0}_{1}", _name, editInfo.Name);

            //Set Style
            editInfo.ControlStyle = new Style();

            //Get Visibility Field
            editInfo.ProfileVisibility = new ProfileVisibility
            {
                VisibilityMode = UserVisibilityMode.AllUsers
            };
            if (!String.IsNullOrEmpty(VisibilityDataField))
            {
                property = _dataSource.GetType().GetProperty(VisibilityDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.ProfileVisibility = (ProfileVisibility)property.GetValue(_dataSource, null);
                }
            }

            //Get Validation Expression Field
            editInfo.ValidationExpression = string.Empty;
            if (!String.IsNullOrEmpty(ValidationExpressionDataField))
            {
                property = _dataSource.GetType().GetProperty(ValidationExpressionDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    editInfo.ValidationExpression = Convert.ToString(property.GetValue(_dataSource, null));
                }
            }

            //Get Length Field
            if (!String.IsNullOrEmpty(MaxLengthDataField))
            {
                property = _dataSource.GetType().GetProperty(MaxLengthDataField);
                if (!(property == null || (property.GetValue(_dataSource, null) == null)))
                {
                    int length = Convert.ToInt32(property.GetValue(_dataSource, null));
                    var attributes = new object[1];
                    attributes[0] = new MaxLengthAttribute(length);
                    editInfo.Attributes = attributes;
                }
            }

            //Remove spaces from name
            editInfo.Name = editInfo.Name.Replace(" ", "_");
            return editInfo;
        }
    }
}
