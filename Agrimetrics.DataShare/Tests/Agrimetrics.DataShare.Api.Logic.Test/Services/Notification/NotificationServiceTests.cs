using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Building;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification
{
    [TestFixture]
    public class NotificationServiceTests
    {
        #region SendToSupplierNewDataShareRequestReceivedNotificationAsync() Tests
        [Test]
        [TestCaseSource(nameof(SendNewDataShareRequestReceivedNotificationWithNullParameterTestCaseData))]
        public void GivenAnEmptyParameter_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string supplierOrganisationEmailAddress,
            string supplierOrganisationName,
            string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(supplierOrganisationEmailAddress, supplierOrganisationName, "_", esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> SendNewDataShareRequestReceivedNotificationWithNullParameterTestCaseData()
        {
            const string supplierOrganisationEmailAddress = "test recipient email address";
            const string supplierOrganisationName = "test supplier organisation name";
            const string esdaName = "test esda name";

            var emptyParameterValues = new List<string> { "", "   " };

            foreach (var emptyParameterValue in emptyParameterValues)
            {
                yield return new TestCaseData("supplierOrganisationEmailAddress", emptyParameterValue, supplierOrganisationName, esdaName);
                yield return new TestCaseData("supplierOrganisationName", supplierOrganisationEmailAddress, emptyParameterValue, esdaName);
                yield return new TestCaseData("esdaName", supplierOrganisationEmailAddress, supplierOrganisationName, emptyParameterValue);
            }
        }

        [Test]
        public async Task GivenDependencyNotificationConfiguration_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenANotificationClientIsCreatedUsingTheConfiguredGovNotifyApiKey()
        {
            var testItems = CreateTestItems();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetGovNotifyApiKey()).Returns("test gov notify api key");

            await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync("_", "_", "_", "_");

            testItems.MockNotificationClientProxyFactory.Verify(x => x.Create("test gov notify api key"),
                Times.Once);
        }

        [Test]
        public async Task GivenValidParameters_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenANewDataShareRequestReceivedNotificationForTheGivenParametersIsSentToTheRecipient()
        {
            var testItems = CreateTestItems();

            var testNewDataShareRequestReceivedNotification = testItems.Fixture.Create<ISupplierNewDataShareRequestReceivedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierNewDataShareRequestReceivedNotification(
                    "test supplier organisation email address",
                    "test supplier organisation name",
                    "test acquirer organisation name",
                    "test esda name"))
                .Returns(testNewDataShareRequestReceivedNotification);

            await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                "test supplier organisation email address",
                "test supplier organisation name",
                "test acquirer organisation name",
                "test esda name");

            testItems.MockNotificationClientProxy.Verify(x => x.SendEmailAsync(testNewDataShareRequestReceivedNotification),
                Times.Once);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenASuccessfulResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testNewDataShareRequestReceivedNotification = testItems.Fixture.Create<ISupplierNewDataShareRequestReceivedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierNewDataShareRequestReceivedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testNewDataShareRequestReceivedNotification);

            var result = await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenAMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testNewDataShareRequestReceivedNotification = testItems.Fixture.Build<SupplierNewDataShareRequestReceivedNotification>()
                .With(x => x.SupplierOrganisationEmailAddress, "test email address")
                .Create();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierNewDataShareRequestReceivedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testNewDataShareRequestReceivedNotification);

            await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                "_",
                "_",
                "_",
            "_");

            testItems.MockLogger.VerifyLog(LogLevel.Trace, "New Data Share Request Received notification sent successfully to Supplier 'test email address'");
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenAFailedResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testNewDataShareRequestReceivedNotification = testItems.Fixture.Create<ISupplierNewDataShareRequestReceivedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierNewDataShareRequestReceivedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testNewDataShareRequestReceivedNotification);

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testNewDataShareRequestReceivedNotification))
                .ThrowsAsync(new Exception());

            var result = await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendNewDataShareRequestReceivedNotificationAsync_ThenAnErrorMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testNewDataShareRequestReceivedNotification = testItems.Fixture.Create<ISupplierNewDataShareRequestReceivedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierNewDataShareRequestReceivedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testNewDataShareRequestReceivedNotification);

            var testException = new Exception();

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testNewDataShareRequestReceivedNotification))
                .ThrowsAsync(testException);

            await testItems.NotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                "_",
                "_",
                "_",
                "_");

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Exception thrown sending New Data Share Request Received notification", testException);
        }
        #endregion

        #region SendToSupplierDataShareRequestCancelledNotificationAsync() Tests
        [Test]
        [TestCaseSource(nameof(SendToSupplierDataShareRequestCancelledNotificationWithNullParameterTestCaseData))]
        public void GivenEmptyParameters_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string supplierOrganisationEmailAddress,
            string supplierOrganisationName,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                    supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId, "_"),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> SendToSupplierDataShareRequestCancelledNotificationWithNullParameterTestCaseData()
        {
            const string supplierOrganisationEmailAddress = "test recipient email address";
            const string supplierOrganisationName = "test supplier organisation name";
            const string acquirerUserName = "test acquirer user name";
            const string esdaName = "test esda name";
            const string dataShareRequestRequestId = "test data share request request id";

            var emptyParameterValues = new List<string> { "", "   " };

            foreach (var emptyParameterValue in emptyParameterValues)
            {
                yield return new TestCaseData("supplierOrganisationEmailAddress", emptyParameterValue, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("supplierOrganisationName", supplierOrganisationEmailAddress, emptyParameterValue, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("acquirerUserName", supplierOrganisationEmailAddress, supplierOrganisationName, emptyParameterValue, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("esdaName", supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, emptyParameterValue, dataShareRequestRequestId);
                yield return new TestCaseData("dataShareRequestRequestId", supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, emptyParameterValue);
            }
        }

        [Test]
        public void GivenNullCancellationReasons_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                    "_", "_", "_", "_", "_", null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("cancellationReasons"));
        }

        [Test]
        public async Task GivenDependencyNotificationConfiguration_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenANotificationClientIsCreatedUsingTheConfiguredGovNotifyApiKey()
        {
            var testItems = CreateTestItems();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetGovNotifyApiKey()).Returns("test gov notify api key");

            await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockNotificationClientProxyFactory.Verify(x => x.Create("test gov notify api key"),
                Times.Once);
        }

        [Test]
        public async Task GivenValidParameters_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenADataShareRequestCancelledNotificationForTheGivenParametersIsSentToTheRecipient()
        {
            var testItems = CreateTestItems();

            var testSupplierDataShareRequestCancelledNotification = testItems.Fixture.Create<ISupplierDataShareRequestCancelledNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierDataShareRequestCancelledNotification(
                    "test supplier organisation email address",
                    "test supplier organisation name",
                    "test acquirer user name",
                    "test esda name",
                    "data share request request id",
                    "cancellation reasons"))
                .Returns(testSupplierDataShareRequestCancelledNotification);

            await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                "test supplier organisation email address",
                "test supplier organisation name",
                "test acquirer user name",
                "test esda name",
                "data share request request id",
                "cancellation reasons");

            testItems.MockNotificationClientProxy.Verify(x => x.SendEmailAsync(testSupplierDataShareRequestCancelledNotification),
                Times.Once);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_SendToSupplierNewDataShareRequestReceivedNotificationAsync_ThenASuccessfulResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testSupplierDataShareRequestCancelledNotification = testItems.Fixture.Create<ISupplierDataShareRequestCancelledNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierDataShareRequestCancelledNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testSupplierDataShareRequestCancelledNotification);

            var result = await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenAMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testSupplierDataShareRequestCancelledNotification = testItems.Fixture.Build<SupplierDataShareRequestCancelledNotification>()
                .With(x => x.SupplierOrganisationEmailAddress, "test email address")
                .Create();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierDataShareRequestCancelledNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testSupplierDataShareRequestCancelledNotification);

            await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
            "_");

            testItems.MockLogger.VerifyLog(LogLevel.Trace, "Data Share Request Cancelled notification sent successfully to Supplier 'test email address'");
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenAFailedResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testSupplierDataShareRequestCancelledNotification = testItems.Fixture.Create<ISupplierDataShareRequestCancelledNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierDataShareRequestCancelledNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testSupplierDataShareRequestCancelledNotification);

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testSupplierDataShareRequestCancelledNotification))
                .ThrowsAsync(new Exception());

            var result = await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToSupplierDataShareRequestCancelledNotificationAsync_ThenAnErrorMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testSupplierDataShareRequestCancelledNotification = testItems.Fixture.Create<ISupplierDataShareRequestCancelledNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildSupplierDataShareRequestCancelledNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testSupplierDataShareRequestCancelledNotification);

            var testException = new Exception();

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testSupplierDataShareRequestCancelledNotification))
                .ThrowsAsync(testException);

            await testItems.NotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Exception thrown sending Data Share Request Cancelled notification", testException);
        }
        #endregion

        #region SendToAcquirerDataShareRequestAcceptedNotificationAsync() Tests
        [Test]
        [TestCaseSource(nameof(SendToAcquirerDataShareRequestAcceptedNotificationWithNullParameterTestCaseData))]
        public void GivenEmptyParameters_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string acquirerUserEmailAddress,
            string supplierOrganisationEmailAddress,
            string supplierOrganisationName,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                    acquirerUserEmailAddress, supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> SendToAcquirerDataShareRequestAcceptedNotificationWithNullParameterTestCaseData()
        {
            const string acquirerUserEmailAddress = "test acquirer user email address";
            const string supplierOrganisationEmailAddress = "test supplier organisation email address";
            const string supplierOrganisationName = "test supplier organisation name";
            const string acquirerUserName = "test acquirer user name";
            const string esdaName = "test esda name";
            const string dataShareRequestRequestId = "test data share request request id";

            var emptyParameterValues = new List<string> { "", "   " };

            foreach (var emptyParameterValue in emptyParameterValues)
            {
                yield return new TestCaseData("acquirerUserEmailAddress", emptyParameterValue, supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("supplierOrganisationEmailAddress", acquirerUserEmailAddress, emptyParameterValue, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("supplierOrganisationName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, emptyParameterValue, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("acquirerUserName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, supplierOrganisationName, emptyParameterValue, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("esdaName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, emptyParameterValue, dataShareRequestRequestId);
                yield return new TestCaseData("dataShareRequestRequestId", acquirerUserEmailAddress, supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, emptyParameterValue);
            }
        }

        [Test]
        public async Task GivenDependencyNotificationConfiguration_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenANotificationClientIsCreatedUsingTheConfiguredGovNotifyApiKey()
        {
            var testItems = CreateTestItems();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetGovNotifyApiKey()).Returns("test gov notify api key");

            await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockNotificationClientProxyFactory.Verify(x => x.Create("test gov notify api key"),
                Times.Once);
        }

        [Test]
        public async Task GivenValidParameters_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenADataShareRequestAcceptedNotificationForTheGivenParametersIsSentToTheRecipient()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestAcceptedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestAcceptedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestAcceptedNotification(
                    "test acquirer user email address",
                    "test supplier organisation email address",
                    "test supplier organisation name",
                    "test acquirer user name",
                    "test esda name",
                    "data share request request id"))
                .Returns(testAcquirerDataShareRequestAcceptedNotification);

            await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                "test acquirer user email address",
                "test supplier organisation email address",
                "test supplier organisation name",
                "test acquirer user name",
                "test esda name",
                "data share request request id");

            testItems.MockNotificationClientProxy.Verify(x => x.SendEmailAsync(testAcquirerDataShareRequestAcceptedNotification),
                Times.Once);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenASuccessfulResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestAcceptedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestAcceptedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestAcceptedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestAcceptedNotification);

            var result = await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenAMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestAcceptedNotification = testItems.Fixture.Build<AcquirerDataShareRequestAcceptedNotification>()
                .With(x => x.AcquirerUserEmailAddress, "test email address")
                .Create();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestAcceptedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestAcceptedNotification);

            await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
            "_");

            testItems.MockLogger.VerifyLog(LogLevel.Trace, "Data Share Request Accepted notification sent successfully to Acquirer 'test email address'");
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenAFailedResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestAcceptedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestAcceptedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestAcceptedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestAcceptedNotification);

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testAcquirerDataShareRequestAcceptedNotification))
                .ThrowsAsync(new Exception());

            var result = await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToAcquirerDataShareRequestAcceptedNotificationAsync_ThenAnErrorMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestAcceptedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestAcceptedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestAcceptedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestAcceptedNotification);

            var testException = new Exception();

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testAcquirerDataShareRequestAcceptedNotification))
                .ThrowsAsync(testException);

            await testItems.NotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Exception thrown sending Data Share Request Accepted notification", testException);
        }
        #endregion

        #region SendToAcquirerDataShareRequestRejectedNotificationAsync() Tests
        [Test]
        [TestCaseSource(nameof(SendToAcquirerDataShareRequestRejectedNotificationNullParameterTestCaseData))]
        public void GivenEmptyParameters_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string acquirerUserEmailAddress,
            string supplierOrganisationEmailAddress,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                    acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, esdaName, dataShareRequestRequestId, "_"),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> SendToAcquirerDataShareRequestRejectedNotificationNullParameterTestCaseData()
        {
            const string acquirerUserEmailAddress = "test acquirer user email address";
            const string supplierOrganisationEmailAddress = "test supplier organisation email address";
            const string acquirerUserName = "test acquirer user name";
            const string esdaName = "test esda name";
            const string dataShareRequestRequestId = "test data share request request id";

            var emptyParameterValues = new List<string> { "", "   " };

            foreach (var emptyParameterValue in emptyParameterValues)
            {
                yield return new TestCaseData("acquirerUserEmailAddress", emptyParameterValue, supplierOrganisationEmailAddress, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("supplierOrganisationEmailAddress", acquirerUserEmailAddress, emptyParameterValue, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("acquirerUserName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, emptyParameterValue, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("esdaName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, emptyParameterValue, dataShareRequestRequestId);
                yield return new TestCaseData("dataShareRequestRequestId", acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, esdaName, emptyParameterValue);
            }
        }

        [Test]
        public void GivenNullReasonsForRejection_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                    "_", "_", "_", "_", "_", null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("reasonsForRejection"));
        }

        [Test]
        public void GivenDependencyNotificationConfiguration_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenANotificationClientIsCreatedUsingTheConfiguredGovNotifyApiKey()
        {
            var testItems = CreateTestItems();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetGovNotifyApiKey()).Returns("test gov notify api key");

            testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockNotificationClientProxyFactory.Verify(x => x.Create("test gov notify api key"),
                Times.Once);
        }

        [Test]
        public async Task GivenValidParameters_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenADataShareRequestRejectedNotificationForTheGivenParametersIsSentToTheRecipient()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestRejectedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestRejectedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestRejectedNotification(
                    "test acquirer user email address",
                    "test supplier organisation email address",
                    "test acquirer user name",
                    "test esda name",
                    "data share request request id",
                    "reasons for rejection"))
                .Returns(testAcquirerDataShareRequestRejectedNotification);

            await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                "test acquirer user email address",
                "test supplier organisation email address",
                "test acquirer user name",
                "test esda name",
                "data share request request id",
                "reasons for rejection");

            testItems.MockNotificationClientProxy.Verify(x => x.SendEmailAsync(testAcquirerDataShareRequestRejectedNotification),
                Times.Once);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenASuccessfulResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestRejectedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestRejectedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestRejectedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestRejectedNotification);

            var result = await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenAMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestRejectedNotification = testItems.Fixture.Build<AcquirerDataShareRequestRejectedNotification>()
                .With(x => x.AcquirerUserEmailAddress, "test email address")
                .Create();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestRejectedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestRejectedNotification);

            await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
            "_");

            testItems.MockLogger.VerifyLog(LogLevel.Trace, "Data Share Request Rejected notification sent successfully to Acquirer 'test email address'");
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenAFailedResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestRejectedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestRejectedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestRejectedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestRejectedNotification);

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testAcquirerDataShareRequestRejectedNotification))
                .ThrowsAsync(new Exception());

            var result = await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToAcquirerDataShareRequestRejectedNotificationAsync_ThenAnErrorMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestRejectedNotification = testItems.Fixture.Create<IAcquirerDataShareRequestRejectedNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestRejectedNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestRejectedNotification);

            var testException = new Exception();

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testAcquirerDataShareRequestRejectedNotification))
                .ThrowsAsync(testException);

            await testItems.NotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Exception thrown sending Data Share Request Rejected notification", testException);
        }
        #endregion

        #region SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync() Tests
        [Test]
        [TestCaseSource(nameof(SendToAcquirerDataShareRequestReturnedWithCommentsNotificationNullParameterTestCaseData))]
        public void GivenEmptyParameters_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string acquirerUserEmailAddress,
            string supplierOrganisationEmailAddress,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(async () => await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                    acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, esdaName, dataShareRequestRequestId),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> SendToAcquirerDataShareRequestReturnedWithCommentsNotificationNullParameterTestCaseData()
        {
            const string acquirerUserEmailAddress = "test acquirer user email address";
            const string supplierOrganisationEmailAddress = "test supplier organisation email address";
            const string acquirerUserName = "test acquirer user name";
            const string esdaName = "test esda name";
            const string dataShareRequestRequestId = "test data share request request id";

            var emptyParameterValues = new List<string> { "", "   " };

            foreach (var emptyParameterValue in emptyParameterValues)
            {
                yield return new TestCaseData("acquirerUserEmailAddress", emptyParameterValue, supplierOrganisationEmailAddress, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("supplierOrganisationEmailAddress", acquirerUserEmailAddress, emptyParameterValue, acquirerUserName, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("acquirerUserName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, emptyParameterValue, esdaName, dataShareRequestRequestId);
                yield return new TestCaseData("esdaName", acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, emptyParameterValue, dataShareRequestRequestId);
                yield return new TestCaseData("dataShareRequestRequestId", acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, esdaName, emptyParameterValue);
            }
        }

        [Test]
        public async Task GivenDependencyNotificationConfiguration_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenANotificationClientIsCreatedUsingTheConfiguredGovNotifyApiKey()
        {
            var testItems = CreateTestItems();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetGovNotifyApiKey()).Returns("test gov notify api key");

            await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockNotificationClientProxyFactory.Verify(x => x.Create("test gov notify api key"),
                Times.Once);
        }

        [Test]
        public async Task GivenValidParameters_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenADataShareRequestReturnedWithCommentsNotificationForTheGivenParametersIsSentToTheRecipient()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestReturnedWithCommentsNotification = testItems.Fixture.Create<IAcquirerDataShareRequestReturnedWithCommentsNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                    "test acquirer user email address",
                    "test supplier organisation email address",
                    "test acquirer user name",
                    "test esda name",
                    "data share request request id"))
                .Returns(testAcquirerDataShareRequestReturnedWithCommentsNotification);

            await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                "test acquirer user email address",
                "test supplier organisation email address",
                "test acquirer user name",
                "test esda name",
                "data share request request id");

            testItems.MockNotificationClientProxy.Verify(x => x.SendEmailAsync(testAcquirerDataShareRequestReturnedWithCommentsNotification),
                Times.Once);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenASuccessfulResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestReturnedWithCommentsNotification = testItems.Fixture.Create<IAcquirerDataShareRequestReturnedWithCommentsNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestReturnedWithCommentsNotification);

            var result = await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillSucceed_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenAMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestReturnedWithCommentsNotification = testItems.Fixture.Build<AcquirerDataShareRequestReturnedWithCommentsNotification>()
                .With(x => x.AcquirerUserEmailAddress, "test email address")
                .Create();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestReturnedWithCommentsNotification);

            await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                "_",
                "_",
                "_",
                "_",
            "_");

            testItems.MockLogger.VerifyLog(LogLevel.Trace, "Data Share Request Returned With Comments notification sent successfully to Acquirer 'test email address'");
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenAFailedResultIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestReturnedWithCommentsNotification = testItems.Fixture.Create<IAcquirerDataShareRequestReturnedWithCommentsNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestReturnedWithCommentsNotification);

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testAcquirerDataShareRequestReturnedWithCommentsNotification))
                .ThrowsAsync(new Exception());

            var result = await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_");

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task GivenSendingTheNotificationWillFail_WhenISendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync_ThenAnErrorMessageIsLogged()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestReturnedWithCommentsNotification = testItems.Fixture.Create<IAcquirerDataShareRequestReturnedWithCommentsNotification>();

            testItems.MockNotificationBuilder.Setup(x => x.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(testAcquirerDataShareRequestReturnedWithCommentsNotification);

            var testException = new Exception();

            testItems.MockNotificationClientProxy.Setup(x => x.SendEmailAsync(testAcquirerDataShareRequestReturnedWithCommentsNotification))
                .ThrowsAsync(testException);

            await testItems.NotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                "_",
                "_",
                "_",
                "_",
                "_");

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Exception thrown sending Data Share Request Returned With Comments notification", testException);
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockLogger = Mock.Get(fixture.Freeze<ILogger<NotificationService>>());
            var mockNotificationsConfigurationPresenter = Mock.Get(fixture.Create<INotificationsConfigurationPresenter>());
            var mockNotificationClientProxyFactory = Mock.Get(fixture.Create<INotificationClientProxyFactory>());
            var mockNotificationBuilder = Mock.Get(fixture.Create<INotificationBuilder>());

            var mockNotificationClientProxy = Mock.Get(fixture.Create<INotificationClientProxy>());

            mockNotificationClientProxyFactory.Setup(x => x.Create(It.IsAny<string>()))
                .Returns(() => mockNotificationClientProxy.Object);

            var notificationService = new NotificationService(
                mockLogger.Object,
                mockNotificationsConfigurationPresenter.Object,
                mockNotificationClientProxyFactory.Object,
                mockNotificationBuilder.Object);

            return new TestItems(
                fixture,
                notificationService,
                mockLogger,
                mockNotificationsConfigurationPresenter,
                mockNotificationClientProxyFactory,
                mockNotificationBuilder,
                mockNotificationClientProxy);
        }

        private class TestItems(
            IFixture fixture,
            INotificationService notificationService,
            Mock<ILogger<NotificationService>> mockLogger,
            Mock<INotificationsConfigurationPresenter> mockNotificationsConfigurationPresenter,
            Mock<INotificationClientProxyFactory> mockNotificationClientProxyFactory,
            Mock<INotificationBuilder> mockNotificationBuilder,
            Mock<INotificationClientProxy> mockNotificationClientProxy)
        {
            public IFixture Fixture { get; } = fixture;
            public INotificationService NotificationService { get; } = notificationService;
            public Mock<ILogger<NotificationService>> MockLogger { get; } = mockLogger;
            public Mock<INotificationsConfigurationPresenter> MockNotificationsConfigurationPresenter { get; } = mockNotificationsConfigurationPresenter;
            public Mock<INotificationClientProxyFactory> MockNotificationClientProxyFactory { get; } = mockNotificationClientProxyFactory;
            public Mock<INotificationBuilder> MockNotificationBuilder { get; } = mockNotificationBuilder;
            public Mock<INotificationClientProxy> MockNotificationClientProxy { get; } = mockNotificationClientProxy;
        }
        #endregion
    }
}
