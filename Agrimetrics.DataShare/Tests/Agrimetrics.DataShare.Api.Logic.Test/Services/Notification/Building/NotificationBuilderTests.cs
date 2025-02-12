using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Building;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification.Building
{
    [TestFixture]
    public class NotificationBuilderTests
    {
        #region BuildSupplierNewDataShareRequestReceivedNotification() Tests
        [Test]
        [TestCaseSource(nameof(BuildNewDataShareRequestReceivedNotificationWithNullParameterTestCaseData))]
        public void GivenAnEmptyParameter_WhenIBuildNewDataShareRequestReceivedNotification_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string supplierOrganisationEmailAddress,
            string supplierOrganisationName,
            string acquirerOrganisationName,
            string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildSupplierNewDataShareRequestReceivedNotification(supplierOrganisationEmailAddress, supplierOrganisationName, acquirerOrganisationName, esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> BuildNewDataShareRequestReceivedNotificationWithNullParameterTestCaseData()
        {
            const string supplierOrganisationEmailAddress = "test supplier organisation email address";
            const string supplierOrganisationName = "test supplier organisation name";
            const string acquirerOrganisationName = "test acquirer organisation name";
            const string esdaName = "test esda name";

            var emptyParameterValues = new List<string> {"", "   "};

            foreach (var emptyParameterValue in emptyParameterValues)
            {
                yield return new TestCaseData("supplierOrganisationEmailAddress", emptyParameterValue, supplierOrganisationName, acquirerOrganisationName, esdaName);
                yield return new TestCaseData("supplierOrganisationName", supplierOrganisationEmailAddress, emptyParameterValue, acquirerOrganisationName, esdaName);
                yield return new TestCaseData("acquirerOrganisationName", supplierOrganisationEmailAddress, supplierOrganisationName, emptyParameterValue, esdaName);
                yield return new TestCaseData("esdaName", supplierOrganisationEmailAddress, supplierOrganisationName, acquirerOrganisationName, emptyParameterValue);
            }
        }

        [Test]
        public void GivenValidParameters_WhenIBuildNewDataShareRequestReceivedNotification_ThenAnInstanceOfNewDataShareRequestReceivedNotificationIsConfiguredWithTheGivenValues()
        {
            var testItems = CreateTestItems();

            var result = testItems.NotificationBuilder.BuildSupplierNewDataShareRequestReceivedNotification(
                "test supplier organisation email address",
                "test supplier organisation name",
                "test acquirer organisation name",
                "test esda name");

            Assert.Multiple(() =>
            {
                Assert.That(result.RecipientEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(result.SupplierOrganisationName, Is.EqualTo("test supplier organisation name"));
                Assert.That(result.AcquirerOrganisationName, Is.EqualTo("test acquirer organisation name"));
                Assert.That(result.EsdaName, Is.EqualTo("test esda name"));
            });
        }

        [Test]
        public void GivenADependencyPageLinksConfiguration_WhenIBuildNewDataShareRequestReceivedNotification_ThenDataMarketPlaceSignInAddressIsConfiguredToTheValueReportByThePageLinksConfiguration()
        {
            var testItems = CreateTestItems();

            testItems.MockPageLinksConfigurationPresenter.Setup(x => x.GetDataMarketPlaceSignInAddress())
                .Returns("test data marketplace sign in address");

            var result = testItems.NotificationBuilder.BuildSupplierNewDataShareRequestReceivedNotification(
                "_", "_", "_", "_");

            Assert.That(result.DataMarketPlaceSignInAddress, Is.EqualTo("test data marketplace sign in address"));
        }
        #endregion

        #region BuildSupplierDataShareRequestCancelledNotification() Tests
        [Test]
        [TestCaseSource(nameof(BuildSupplierDataShareRequestCancelledNotificationWithNullParameterTestCaseData))]
        public void GivenAnEmptyParameter_WhenIBuildSupplierDataShareRequestCancelledNotification_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string supplierOrganisationEmailAddress,
            string supplierOrganisationName,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildSupplierDataShareRequestCancelledNotification(
                    supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId, "_"),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> BuildSupplierDataShareRequestCancelledNotificationWithNullParameterTestCaseData()
        {
            const string supplierOrganisationEmailAddress = "test supplier organisation email address";
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
        public void GivenNullCancellationReasons_WhenIBuildSupplierDataShareRequestCancelledNotification_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildSupplierDataShareRequestCancelledNotification(
                    "_", "_", "_", "_", "_", null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("cancellationReasons"));
        }

        [Test]
        public void GivenValidParameters_WhenIBuildSupplierDataShareRequestCancelledNotification_ThenAnInstanceOfISupplierDataShareRequestCancelledNotificationIsConfiguredWithTheGivenValues()
        {
            var testItems = CreateTestItems();

            var result = testItems.NotificationBuilder.BuildSupplierDataShareRequestCancelledNotification(
                "test supplier organisation email address",
                "test supplier organisation name",
                "test acquirer organisation name",
                "test esda name",
                "test data share request request id",
                "test cancellation reasons");

            Assert.Multiple(() =>
            {
                Assert.That(result.RecipientEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(result.SupplierOrganisationEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(result.SupplierOrganisationName, Is.EqualTo("test supplier organisation name"));
                Assert.That(result.AcquirerUserName, Is.EqualTo("test acquirer organisation name"));
                Assert.That(result.EsdaName, Is.EqualTo("test esda name"));
                Assert.That(result.DataShareRequestRequestId, Is.EqualTo("test data share request request id"));
                Assert.That(result.CancellationReasons, Is.EqualTo("test cancellation reasons"));
            });
        }
        #endregion

        #region BuildAcquirerDataShareRequestAcceptedNotification() Tests
        [Test]
        [TestCaseSource(nameof(BuildAcquirerDataShareRequestAcceptedNotificationWithNullParameterTestCaseData))]
        public void GivenAnEmptyParameter_WhenIBuildAcquirerDataShareRequestAcceptedNotification_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string acquirerUserEmailAddress,
            string supplierOrganisationEmailAddress,
            string supplierOrganisationName,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildAcquirerDataShareRequestAcceptedNotification(
                    acquirerUserEmailAddress, supplierOrganisationEmailAddress, supplierOrganisationName, acquirerUserName, esdaName, dataShareRequestRequestId),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> BuildAcquirerDataShareRequestAcceptedNotificationWithNullParameterTestCaseData()
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
        public void GivenValidParameters_WhenIBuildAcquirerDataShareRequestAcceptedNotification_ThenAnInstanceOfIAcquirerDataShareRequestAcceptedNotificationWithTheGivenValues()
        {
            var testItems = CreateTestItems();

            var result = testItems.NotificationBuilder.BuildAcquirerDataShareRequestAcceptedNotification(
                "test acquirer user email address",
                "test supplier organisation email address",
                "test supplier organisation name",
                "test acquirer organisation name",
                "test esda name",
                "test data share request request id");

            Assert.Multiple(() =>
            {
                Assert.That(result.RecipientEmailAddress, Is.EqualTo("test acquirer user email address"));
                Assert.That(result.AcquirerUserEmailAddress, Is.EqualTo("test acquirer user email address"));
                Assert.That(result.SupplierOrganisationEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(result.SupplierOrganisationName, Is.EqualTo("test supplier organisation name"));
                Assert.That(result.AcquirerUserName, Is.EqualTo("test acquirer organisation name"));
                Assert.That(result.EsdaName, Is.EqualTo("test esda name"));
                Assert.That(result.DataShareRequestRequestId, Is.EqualTo("test data share request request id"));
            });
        }
        #endregion

        #region BuildAcquirerDataShareRequestRejectedNotification() Tests
        [Test]
        [TestCaseSource(nameof(BuildAcquirerDataShareRequestRejectedNotificationWithEmptyParameterTestCaseData))]
        public void GivenAnEmptyParameter_WhenIBuildAcquirerDataShareRequestRejectedNotification_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string acquirerUserEmailAddress,
            string supplierOrganisationEmailAddress,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildAcquirerDataShareRequestRejectedNotification(
                    acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, esdaName, dataShareRequestRequestId, "_"),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> BuildAcquirerDataShareRequestRejectedNotificationWithEmptyParameterTestCaseData()
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
        public void GivenANullParameter_WhenIBuildAcquirerDataShareRequestRejectedNotification_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildAcquirerDataShareRequestRejectedNotification(
                    "_", "_", "_", "_", "_", null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("reasonsForRejection"));
        }

        [Test]
        public void GivenValidParameters_WhenIBuildAcquirerDataShareRequestRejectedNotification_ThenAnInstanceOfIAcquirerDataShareRequestRejectedNotificationIsCreatedWithTheGivenValues()
        {
            var testItems = CreateTestItems();

            var result = testItems.NotificationBuilder.BuildAcquirerDataShareRequestRejectedNotification(
                "test acquirer user email address",
                "test supplier organisation email address",
                "test acquirer organisation name",
                "test esda name",
                "test data share request request id",
                "test reasons for rejection");

            Assert.Multiple(() =>
            {
                Assert.That(result.RecipientEmailAddress, Is.EqualTo("test acquirer user email address"));
                Assert.That(result.AcquirerUserEmailAddress, Is.EqualTo("test acquirer user email address"));
                Assert.That(result.SupplierOrganisationEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(result.AcquirerUserName, Is.EqualTo("test acquirer organisation name"));
                Assert.That(result.EsdaName, Is.EqualTo("test esda name"));
                Assert.That(result.DataShareRequestRequestId, Is.EqualTo("test data share request request id"));
                Assert.That(result.ReasonsForRejection, Is.EqualTo("test reasons for rejection"));
            });
        }
        #endregion

        #region BuildAcquirerDataShareRequestReturnedWithCommentsNotification() Tests
        [Test]
        [TestCaseSource(nameof(BuildAcquirerDataShareRequestReturnedWithCommentsNotificationWithEmptyParameterTestCaseData))]
        public void GivenAnEmptyParameter_WhenIBuildAcquirerDataShareRequestReturnedWithCommentsNotification_ThenAnArgumentExceptionIsThrown(
            string expectedExceptionParameterName,
            string acquirerUserEmailAddress,
            string supplierOrganisationEmailAddress,
            string acquirerUserName,
            string esdaName,
            string dataShareRequestRequestId)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationBuilder.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                    acquirerUserEmailAddress, supplierOrganisationEmailAddress, acquirerUserName, esdaName, dataShareRequestRequestId),
                Throws.ArgumentException.With.Property("ParamName").EqualTo(expectedExceptionParameterName));
        }

        private static IEnumerable<TestCaseData> BuildAcquirerDataShareRequestReturnedWithCommentsNotificationWithEmptyParameterTestCaseData()
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
        public void GivenValidParameters_WhenIBuildAcquirerDataShareRequestReturnedWithCommentsNotification_ThenAnIAcquirerDataShareRequestReturnedWithCommentsNotificationIsCreatedWithTheGivenValues()
        {
            var testItems = CreateTestItems();

            var result = testItems.NotificationBuilder.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                "test acquirer user email address",
                "test supplier organisation email address",
                "test acquirer organisation name",
                "test esda name",
                "test data share request request id");

            Assert.Multiple(() =>
            {
                Assert.That(result.RecipientEmailAddress, Is.EqualTo("test acquirer user email address"));
                Assert.That(result.AcquirerUserEmailAddress, Is.EqualTo("test acquirer user email address"));
                Assert.That(result.SupplierOrganisationEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(result.AcquirerUserName, Is.EqualTo("test acquirer organisation name"));
                Assert.That(result.EsdaName, Is.EqualTo("test esda name"));
                Assert.That(result.DataShareRequestRequestId, Is.EqualTo("test data share request request id"));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockNotificationConfiguration = Mock.Get(fixture.Create<INotificationConfiguration>());
            var mockPageLinksConfigurationPresenter = Mock.Get(fixture.Create<IPageLinksConfigurationPresenter>());

            var notificationBuilder = new NotificationBuilder(
                mockNotificationConfiguration.Object,
                mockPageLinksConfigurationPresenter.Object);

            return new TestItems(
                notificationBuilder,
                mockPageLinksConfigurationPresenter);
        }

        private class TestItems(
            INotificationBuilder notificationBuilder,
            Mock<IPageLinksConfigurationPresenter> mockPageLinksConfigurationPresenter)
        {
            public INotificationBuilder NotificationBuilder { get; } = notificationBuilder;
            public Mock<IPageLinksConfigurationPresenter> MockPageLinksConfigurationPresenter { get; } = mockPageLinksConfigurationPresenter;
        }
        #endregion
    }
}
