using Agrimetrics.DataShare.Api.Core.Configuration;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Configuration;

namespace Agrimetrics.DataShare.Api.Logic.Test.Configuration
{
    [TestFixture]
    public class NotificationsConfigurationPresenterTests
    {
        #region GetGovNotifyApiKey()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetGovNotifyApiKey_ThenTheConfiguredKeyIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    "Notifications", "gov_notify_api_key"))
                .Returns("test gov notify api key");

            var result = testItems.NotificationsConfigurationPresenter.GetGovNotifyApiKey();

            Assert.That(result, Is.EqualTo("test gov notify api key"));
        }
        #endregion

        #region GetSupplierNewDataShareRequestReceivedTemplateId()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetNewDataShareRequestReceivedTemplateId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string>{ "Notifications", "TemplateIds"},
            "new_data_share_request_received_template_id"))
                .Returns("441AC09F-B88E-4EF3-87B2-E00A41206DCB");

            var result = testItems.NotificationsConfigurationPresenter.GetSupplierNewDataShareRequestReceivedTemplateId();

            Assert.That(result, Is.EqualTo(Guid.Parse("441AC09F-B88E-4EF3-87B2-E00A41206DCB")));
        }
        #endregion

        #region GetSupplierDataShareRequestCancelledTemplateId()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetSupplierDataShareRequestCancelledTemplateId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" },
                    "data_share_request_cancelled_template_id"))
                .Returns("441AC09F-B88E-4EF3-87B2-E00A41206DCB");

            var result = testItems.NotificationsConfigurationPresenter.GetSupplierDataShareRequestCancelledTemplateId();

            Assert.That(result, Is.EqualTo(Guid.Parse("441AC09F-B88E-4EF3-87B2-E00A41206DCB")));
        }
        #endregion

        #region GetAcquirerDataShareRequestAcceptedTemplateId()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetAcquirerDataShareRequestAcceptedTemplateId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" },
                    "data_share_request_accepted_template_id"))
                .Returns("441AC09F-B88E-4EF3-87B2-E00A41206DCB");

            var result = testItems.NotificationsConfigurationPresenter.GetAcquirerDataShareRequestAcceptedTemplateId();

            Assert.That(result, Is.EqualTo(Guid.Parse("441AC09F-B88E-4EF3-87B2-E00A41206DCB")));
        }
        #endregion

        #region GetAcquirerDataShareRequestRejectedTemplateId()
        [Test]
        public void GivenANotificationsConfigurationPresenter_GetAcquirerDataShareRequestRejectedTemplateId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" },
                    "data_share_request_rejected_template_id"))
                .Returns("441AC09F-B88E-4EF3-87B2-E00A41206DCB");

            var result = testItems.NotificationsConfigurationPresenter.GetAcquirerDataShareRequestRejectedTemplateId();

            Assert.That(result, Is.EqualTo(Guid.Parse("441AC09F-B88E-4EF3-87B2-E00A41206DCB")));
        }
        #endregion

        #region GetAcquirerDataShareRequestReturnedWithCommentsTemplateId()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetAcquirerDataShareRequestReturnedWithCommentsTemplateId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" },
                    "data_share_request_returned_with_comments_template_id"))
                .Returns("441AC09F-B88E-4EF3-87B2-E00A41206DCB");

            var result = testItems.NotificationsConfigurationPresenter.GetAcquirerDataShareRequestReturnedWithCommentsTemplateId();

            Assert.That(result, Is.EqualTo(Guid.Parse("441AC09F-B88E-4EF3-87B2-E00A41206DCB")));
        }
        #endregion

        #region GetDataShareRequestNotificationCddoAdminEmailAddress()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetDataShareRequestNotificationCddoAdminEmailAddress_ThenTheConfiguredDataShareRequestNotificationCddoAdminEmailAddressIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    "Notifications",
                    "cddo_admin_supplier_dsr_notification_email_address"))
                .Returns("test email address");

            var result = testItems.NotificationsConfigurationPresenter.GetDataShareRequestNotificationCddoAdminEmailAddress();

            Assert.That(result, Is.EqualTo("test email address"));
        }
        #endregion

        #region GetDataShareRequestNotificationCddoAdminName()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetDataShareRequestNotificationCddoAdminName_ThenTheConfiguredDataShareRequestNotificationCddoAdminNameIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    "Notifications",
                    "cddo_admin_supplier_dsr_notification_name"))
                .Returns("test name");

            var result = testItems.NotificationsConfigurationPresenter.GetDataShareRequestNotificationCddoAdminName();

            Assert.That(result, Is.EqualTo("test name"));
        }
        #endregion

        #region GetAllSettingValues()
        [Test]
        public void GivenANotificationsConfigurationPresenter_WhenIGetAllSettingValues_ThenADescriptionOfAllSettingsIsReturned()
        {
            var testItems = CreateTestItems();

            var testGovNotifyApiKey = testItems.Fixture.Create<string>();
            var testNewDataShareRequestReceivedTemplateId = testItems.Fixture.Create<Guid>().ToString();
            var testDataShareRequestCancelledTemplateId = testItems.Fixture.Create<Guid>().ToString();
            var testDataShareRequestAcceptedTemplateId = testItems.Fixture.Create<Guid>().ToString();
            var testDataShareRequestRejectedTemplateId = testItems.Fixture.Create<Guid>().ToString();
            var testDataShareRequestReturnedWithCommentsTemplateId = testItems.Fixture.Create<Guid>().ToString();
            var testCddoAdminEmailAddress = testItems.Fixture.Create<string>();
            var testCddoAdminName = testItems.Fixture.Create<string>();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    "Notifications", "gov_notify_api_key"))
                .Returns(testGovNotifyApiKey);
            
            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" }, "new_data_share_request_received_template_id"))
                .Returns(testNewDataShareRequestReceivedTemplateId);

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" }, "data_share_request_cancelled_template_id"))
                .Returns(testDataShareRequestCancelledTemplateId);

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" }, "data_share_request_accepted_template_id"))
                .Returns(testDataShareRequestAcceptedTemplateId);

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "Notifications", "TemplateIds" }, "data_share_request_rejected_template_id"))
                .Returns(testDataShareRequestRejectedTemplateId);

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(new List<string> { "Notifications", "TemplateIds" },
                    "data_share_request_returned_with_comments_template_id"))
                .Returns(testDataShareRequestReturnedWithCommentsTemplateId);

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    "Notifications", "cddo_admin_supplier_dsr_notification_email_address"))
                .Returns(testCddoAdminEmailAddress);

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    "Notifications", "cddo_admin_supplier_dsr_notification_name"))
                .Returns(testCddoAdminName);

            var result = testItems.NotificationsConfigurationPresenter.GetAllSettingValues().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(8).Items);

                Assert.That(result.Any(x =>
                        x.Description == "Gov Notify Api Key" &&
                        x.Value == testGovNotifyApiKey),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "New Data Share Request Received Template Id" &&
                        x.Value == testNewDataShareRequestReceivedTemplateId),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "Data Share Request Cancelled Template Id" &&
                        x.Value == testDataShareRequestCancelledTemplateId),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "Data Share Request Accepted Template Id" &&
                        x.Value == testDataShareRequestAcceptedTemplateId),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "Data Share Request Rejected Template Id" &&
                        x.Value == testDataShareRequestRejectedTemplateId),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "Data Share Request Returned With Comments Template Id" &&
                        x.Value == testDataShareRequestReturnedWithCommentsTemplateId),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "Data Share Request Notification Cddo Admin Email Address" &&
                        x.Value == testCddoAdminEmailAddress),
                    Is.True);

                Assert.That(result.Any(x =>
                        x.Description == "Data Share Request Notification Cddo Admin Name" &&
                        x.Value == testCddoAdminName),
                    Is.True);
            });
            
        }

        [Test]
        public void GivenGettingAConfigurationValueWithThrowAnException_WhenIGetAllSettingValues_ThenTheErrorIsReturnedForThatValue()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => "C6D904ED-2C52-4B68-A5B3-6333837596DB");

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .Returns(() => "C6D904ED-2C52-4B68-A5B3-6333837596DB");

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    It.IsAny<IEnumerable<string>>(), "data_share_request_cancelled_template_id"))
                .Throws(new Exception("error 1"));

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                    It.IsAny<string>(), "cddo_admin_supplier_dsr_notification_email_address"))
               .Throws(new Exception("error 2"));

            var result = testItems.NotificationsConfigurationPresenter.GetAllSettingValues().ToList();

            Assert.Multiple(() =>
            {
                var valuesNotFound = result.Where(x =>
                    x.Value.StartsWith("ERROR")).ToList();

                Assert.That(valuesNotFound, Has.Exactly(2).Items);

                Assert.That(valuesNotFound.FirstOrDefault(x =>
                    x is {Description: "Data Share Request Cancelled Template Id", Value: "ERROR: error 1"}), Is.Not.Null);

                Assert.That(valuesNotFound.FirstOrDefault(x =>
                    x is { Description: "Data Share Request Notification Cddo Admin Email Address", Value: "ERROR: error 2" }), Is.Not.Null);
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockServiceConfigurationPresenter = new Mock<IServiceConfigurationPresenter>();

            var notificationsConfigurationPresenter = new NotificationsConfigurationPresenter(
                mockServiceConfigurationPresenter.Object);

            return new TestItems(
                fixture,
                notificationsConfigurationPresenter,
                mockServiceConfigurationPresenter);
        }

        private class TestItems(
            IFixture fixture,
            INotificationsConfigurationPresenter notificationsConfigurationPresenter,
            Mock<IServiceConfigurationPresenter> mockServiceConfigurationPresenter)
        {
            public IFixture Fixture { get; } = fixture;
            public INotificationsConfigurationPresenter NotificationsConfigurationPresenter { get; } = notificationsConfigurationPresenter;
            public Mock<IServiceConfigurationPresenter> MockServiceConfigurationPresenter { get; } = mockServiceConfigurationPresenter;
        }
        #endregion
    }
}
