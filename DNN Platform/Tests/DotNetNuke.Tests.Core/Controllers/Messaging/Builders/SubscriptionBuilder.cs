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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Social.Subscriptions.Entities;
using DotNetNuke.Tests.Utilities;

namespace DotNetNuke.Tests.Core.Controllers.Messaging.Builders
{
    public class SubscriptionBuilder
    {
        private int _subscriptionId;
        private int _userId;
        private int _portalId;
        private int _subscriptionTypeId;
        private string _objectKey;
        private string _description;
        private int _moduleId;
        private int _tabId;
        private string _objectData;

        internal SubscriptionBuilder()
        {
            _subscriptionId = 1;
            _userId = Constants.USER_InValidId;
            _subscriptionTypeId = 1;
            _portalId = Constants.PORTAL_ValidPortalId;
            _moduleId = Null.NullInteger;
            _tabId = Null.NullInteger;
            _objectKey = "content";
            _description = "my content description";
            _objectData = "";
        }

        internal SubscriptionBuilder WithSubscriptionId(int subscriptionId)
        {
            _subscriptionId = subscriptionId;
            return this;
        }

        internal SubscriptionBuilder WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        internal SubscriptionBuilder WithPortalId(int portalId)
        {
            _portalId = portalId;
            return this;
        }

        internal SubscriptionBuilder WithSubscriptionTypeId(int subscriptionTypeId)
        {
            _subscriptionTypeId = subscriptionTypeId;
            return this;
        }

        internal SubscriptionBuilder WithObjectKey(string objectKey)
        {
            _objectKey = objectKey;
            return this;
        }

        internal SubscriptionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        internal SubscriptionBuilder WithModuleId(int moduleId)
        {
            _moduleId = moduleId;
            return this;
        }

        internal SubscriptionBuilder WithTabId(int tabId)
        {
            _tabId = tabId;
            return this;
        }

        internal Subscription Build()
        {
            return new Subscription
            {
                SubscriptionTypeId = _subscriptionTypeId,
                SubscriptionId = _subscriptionId,
                CreatedOnDate = DateTime.UtcNow,
                ModuleId = _moduleId,
                ObjectKey = _objectKey,
                Description = _description,
                PortalId = _portalId,
                TabId = _tabId,
                UserId = _userId,
                ObjectData = _objectData
            };
        }
    }
}
