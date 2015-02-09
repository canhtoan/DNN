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

#endregion
namespace DotNetNuke.Services.Search
{
    /// -----------------------------------------------------------------------------
    /// Namespace:  DotNetNuke.Services.Search
    /// Project:    DotNetNuke
    /// Class:      SearchResultsInfo
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The SearchResultsInfo represents a Search Result Item
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///		[cnurse]	11/15/2004	documented
    /// </history>
    /// -----------------------------------------------------------------------------
    [Obsolete("Deprecated in DNN 7.1.  No longer used in the Search infrastructure.")]
    [Serializable]
    public class SearchResultsInfo
    {
        private string _author;
        private string _authorName;
        private bool _delete;
        private string _description;
        private string _guid;
        private int _image;
        private int _moduleId;
        private int _occurrences;
        private int _portalId;
        private DateTime _pubDate;
        private int _relevance;
        private int _searchItemID;
        private string _searchKey;
        private int _tabId;
        private string _title;

        public int SearchItemID
        {
            get
            {
                return _searchItemID;
            }
            set
            {
                _searchItemID = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
            }
        }

        public DateTime PubDate
        {
            get
            {
                return _pubDate;
            }
            set
            {
                _pubDate = value;
            }
        }

        public string Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        public int Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public int TabId
        {
            get
            {
                return _tabId;
            }
            set
            {
                _tabId = value;
            }
        }

        public string SearchKey
        {
            get
            {
                return _searchKey;
            }
            set
            {
                _searchKey = value;
            }
        }

        public int Occurrences
        {
            get
            {
                return _occurrences;
            }
            set
            {
                _occurrences = value;
            }
        }

        public int Relevance
        {
            get
            {
                return _relevance;
            }
            set
            {
                _relevance = value;
            }
        }

        public int ModuleId
        {
            get
            {
                return _moduleId;
            }
            set
            {
                _moduleId = value;
            }
        }

        public bool Delete
        {
            get
            {
                return _delete;
            }
            set
            {
                _delete = value;
            }
        }

        public string AuthorName
        {
            get
            {
                return _authorName;
            }
            set
            {
                _authorName = value;
            }
        }

        public int PortalId
        {
            get
            {
                return _portalId;
            }
            set
            {
                _portalId = value;
            }
        }
    }
}
