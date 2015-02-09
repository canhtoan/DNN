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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

using DotNetNuke.Entities.Modules;

#endregion
namespace DotNetNuke.Services.Exceptions
{
    public class ModuleLoadException : BasePortalException
    {
        private readonly ModuleInfo _moduleConfiguration;
        private string _friendlyName;
        private string _moduleControlSource;
        private int _moduleDefId;
        private int _moduleId;

        //default constructor
        public ModuleLoadException()
        {
        }

        //constructor with exception message
        public ModuleLoadException(string message) : base(message)
        {
            InitilizePrivateVariables();
        }

        //constructor with exception message
        public ModuleLoadException(string message, Exception inner, ModuleInfo ModuleConfiguration) : base(message, inner)
        {
            _moduleConfiguration = ModuleConfiguration;
            InitilizePrivateVariables();
        }

        //constructor with message and inner exception
        public ModuleLoadException(string message, Exception inner) : base(message, inner)
        {
            InitilizePrivateVariables();
        }

        protected ModuleLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            InitilizePrivateVariables();
            _moduleId = info.GetInt32("m_ModuleId");
            _moduleDefId = info.GetInt32("m_ModuleDefId");
            _friendlyName = info.GetString("m_FriendlyName");
        }

        [XmlElement("ModuleID")]
        public int ModuleId
        {
            get
            {
                return _moduleId;
            }
        }

        [XmlElement("ModuleDefId")]
        public int ModuleDefId
        {
            get
            {
                return _moduleDefId;
            }
        }

        [XmlElement("FriendlyName")]
        public string FriendlyName
        {
            get
            {
                return _friendlyName;
            }
        }

        [XmlElement("ModuleControlSource")]
        public string ModuleControlSource
        {
            get
            {
                return _moduleControlSource;
            }
        }

        private void InitilizePrivateVariables()
        {
            //Try and get the Portal settings from context
            //If an error occurs getting the context then set the variables to -1
            if ((_moduleConfiguration != null))
            {
                _moduleId = _moduleConfiguration.ModuleID;
                _moduleDefId = _moduleConfiguration.ModuleDefID;
                _friendlyName = _moduleConfiguration.ModuleTitle;
                _moduleControlSource = _moduleConfiguration.ModuleControl.ControlSrc;
            }
            else
            {
                _moduleId = -1;
                _moduleDefId = -1;
            }
        }

        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    //Serialize this class' state and then call the base class GetObjectData
        //    info.AddValue("m_ModuleId", m_ModuleId, typeof (Int32));
        //    info.AddValue("m_ModuleDefId", m_ModuleDefId, typeof (Int32));
        //    info.AddValue("m_FriendlyName", m_FriendlyName, typeof (string));
        //    info.AddValue("m_ModuleControlSource", m_ModuleControlSource, typeof (string));
        //    base.GetObjectData(info, context);
        //}
    }
}