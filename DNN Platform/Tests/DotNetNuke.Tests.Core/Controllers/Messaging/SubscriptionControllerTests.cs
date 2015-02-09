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
using DotNetNuke.Services.Cache;
using DotNetNuke.Services.Social.Subscriptions;
using DotNetNuke.Services.Social.Subscriptions.Data;
using DotNetNuke.Services.Social.Subscriptions.Entities;
using DotNetNuke.Tests.Core.Controllers.Messaging.Builders;
using DotNetNuke.Tests.Core.Controllers.Messaging.Mocks;
using DotNetNuke.Tests.Utilities.Mocks;
using Moq;
using NUnit.Framework;

namespace DotNetNuke.Tests.Core.Controllers.Messaging
{
    [TestFixture]
    public class SubscriptionControllerTests
    {
        private Mock<IDataService> _mockDataService;
        private Mock<ISubscriptionSecurityController> _subscriptionSecurityController;
        private Mock<CachingProvider> _mockCacheProvider;

        private SubscriptionController _subscriptionController;

        [SetUp]
        public void SetUp()
        {
            // Setup Mocks and Stub
            _mockDataService = new Mock<IDataService>();
            _mockCacheProvider = MockComponentProvider.CreateDataCacheProvider();
            _subscriptionSecurityController = new Mock<ISubscriptionSecurityController>();

            DataService.SetTestableInstance(_mockDataService.Object);
            SubscriptionSecurityController.SetTestableInstance(_subscriptionSecurityController.Object);

            // Setup SUT
            _subscriptionController = new SubscriptionController();
        }

        [TearDown]
        public void TearDown()
        {
            DataService.ClearInstance();
            SubscriptionSecurityController.ClearInstance();
            MockComponentProvider.ResetContainer();
        }

        #region IsSubscribed method tests
        [Test]
        public void IsSubscribed_ShouldReturnFalse_IfUserIsNotSubscribed()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            _mockDataService.Setup(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(SubscriptionDataReaderMockHelper.CreateEmptySubscriptionReader());

            //Act
            var isSubscribed = _subscriptionController.IsSubscribed(subscription);

            // Assert
            Assert.AreEqual(false, isSubscribed);
        }

        [Test]
        public void IsSubscribed_ShouldReturnFalse_WhenUserDoesNotHavePermissionOnTheSubscription()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            var subscriptionCollection = new[] { subscription };

            _mockDataService.Setup(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(SubscriptionDataReaderMockHelper.CreateSubscriptionReader(subscriptionCollection));

            _subscriptionSecurityController
                .Setup(ssc => ssc.HasPermission(It.IsAny<Subscription>())).Returns(false);

            //Act
            var isSubscribed = _subscriptionController.IsSubscribed(subscription);

            // Assert
            Assert.AreEqual(false, isSubscribed);
        }

        [Test]
        public void IsSubscribed_ShouldReturnTrue_WhenUserHasPermissionOnTheSubscription()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            var subscriptionCollection = new[] { subscription };

            _mockDataService.Setup(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(SubscriptionDataReaderMockHelper.CreateSubscriptionReader(subscriptionCollection));

            _subscriptionSecurityController
                .Setup(ssc => ssc.HasPermission(It.IsAny<Subscription>())).Returns(true);

            //Act
            var isSubscribed = _subscriptionController.IsSubscribed(subscription);

            // Assert
            Assert.AreEqual(true, isSubscribed);
        }

        [Test]
        public void IsSubscribed_ShouldCallDataService_WhenNoError()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            _mockDataService.Setup(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                subscription.ModuleId,
                subscription.TabId)).Returns(SubscriptionDataReaderMockHelper.CreateEmptySubscriptionReader()).Verifiable();

            //Act
            _subscriptionController.IsSubscribed(subscription);

            // Assert
            _mockDataService.Verify(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                subscription.ModuleId,
                subscription.TabId), Times.Once);
        }
        #endregion

        #region AddSubscription method tests
        [Test]
        public void AddSubscription_ShouldThrowArgumentNullException_WhenSubscriptionIsNull()
        {
            //Act, Arrange
            Assert.Throws<ArgumentNullException>(() => _subscriptionController.AddSubscription(null));
        }

        [Test]
        public void AddSubscription_ShouldThrowArgumentOutOfRangeException_WhenSubscriptionUserIdPropertyIsNegative()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .WithUserId(-1)
                .Build();

            //Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _subscriptionController.AddSubscription(subscription));
        }

        [Test]
        public void AddSubscription_ShouldThrowArgumentOutOfRangeException_WhenSubscriptionSubscriptionTypePropertyIsNegative()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .WithSubscriptionTypeId(-1)
                .Build();

            //Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _subscriptionController.AddSubscription(subscription));
        }

        [Test]
        public void AddSubscription_ShouldThrowArgumentNullException_WhenSubscriptionObjectKeyIsNull()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .WithObjectKey(null)
                .Build();

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => _subscriptionController.AddSubscription(subscription));
        }

        [Test]
        public void AddSubscription_ShouldCallDataService_WhenNoError()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            _mockDataService.Setup(ds => ds.AddSubscription(
                subscription.UserId,
                subscription.PortalId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                subscription.Description,
                subscription.ModuleId,
                subscription.TabId,
                subscription.ObjectData)).Verifiable();

            //Act
            _subscriptionController.AddSubscription(subscription);

            // Assert
            _mockDataService.Verify(ds => ds.AddSubscription(
                subscription.UserId,
                subscription.PortalId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                subscription.Description,
                subscription.ModuleId,
                subscription.TabId,
                subscription.ObjectData), Times.Once);
        }

        [Test]
        public void AddSubscription_ShouldFilledUpTheSubscriptionIdPropertyOfTheInputSubscriptionEntity_WhenNoError()
        {
            // Arrange
            const int expectedSubscriptionId = 12;

            var subscription = new SubscriptionBuilder()
                .Build();

            _mockDataService.Setup(ds => ds.AddSubscription(
                subscription.UserId,
                subscription.PortalId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                subscription.Description,
                subscription.ModuleId,
                subscription.TabId,
                subscription.ObjectData)).Returns(expectedSubscriptionId);

            //Act
            _subscriptionController.AddSubscription(subscription);

            // Assert
            Assert.AreEqual(expectedSubscriptionId, subscription.SubscriptionId);
        }
        #endregion

        #region DeleteSubscription method tests
        [Test]
        public void DeleteSubscription_ShouldThrowArgumentNullException_WhenSubscriptionIsNull()
        {
            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => _subscriptionController.DeleteSubscription(null));
        }

        [Test]
        public void DeleteSubscriptionType_ShouldCallDeleteSubscriptionDataService_WhenSubscriptionExists()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            _mockDataService.Setup(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(SubscriptionDataReaderMockHelper.CreateSubscriptionReader(new[] { subscription }));

            _mockDataService.Setup(ds => ds.DeleteSubscription(It.IsAny<int>())).Verifiable();

            //Act
            _subscriptionController.DeleteSubscription(subscription);

            //Assert
            _mockDataService.Verify(ds => ds.DeleteSubscription(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void DeleteSubscriptionType_ShouldNotCallDeleteSubscriptionDataService_WhenSubscriptionDoesNotExist()
        {
            // Arrange
            var subscription = new SubscriptionBuilder()
                .Build();

            _mockDataService.Setup(ds => ds.IsSubscribed(
                subscription.PortalId,
                subscription.UserId,
                subscription.SubscriptionTypeId,
                subscription.ObjectKey,
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(SubscriptionDataReaderMockHelper.CreateEmptySubscriptionReader());

            //Act
            _subscriptionController.DeleteSubscription(subscription);

            //Assert
            _mockDataService.Verify(ds => ds.DeleteSubscription(It.IsAny<int>()), Times.Never);
        }
        #endregion
    }
}
