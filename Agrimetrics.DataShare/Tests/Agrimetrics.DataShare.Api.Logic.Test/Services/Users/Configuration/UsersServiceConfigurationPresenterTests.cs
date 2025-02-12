using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.Configuration
{
    public class UsersServiceConfigurationPresenterTests
    {
        #region GetUserInfoByTokenEndPoint() Tests
        [Test]
        public void GivenADependencyServiceConfigurationPresenter_WhenIGetUserInfoByTokenEndPoint_ThenTheExpectedAddressIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> {"ExternalServices", "UsersApi"}, "api_address"))
                .Returns("test user info address");

            var result = testItems.UsersServiceConfigurationPresenter.GetUserInfoByTokenEndPoint();

            Assert.That(result, Is.EqualTo("test user info address/User/userinfo"));
        }
        #endregion

        #region GetUserInfoByUserIdEndPoint() Tests
        [Test]
        public void GivenADependencyServiceConfigurationPresenter_WhenIGetUserInfoByUserIdEndPoint_ThenTheExpectedAddressIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UsersApi" }, "api_address"))
                .Returns("test user info address");

            var result = testItems.UsersServiceConfigurationPresenter.GetUserInfoByUserIdEndPoint();

            Assert.That(result, Is.EqualTo("test user info address/User/UserById"));
        }
        #endregion

        #region GetUserInfosByUserIdsEndPoint() Tests
        [Test]
        public void GivenADependencyServiceConfigurationPresenter_WhenIGetUserInfosByUserIdsEndPoint_ThenTheExpectedAddressIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UsersApi" }, "api_address"))
                .Returns("test user info address");

            var result = testItems.UsersServiceConfigurationPresenter.GetUserInfosByUserIdsEndPoint();

            Assert.That(result, Is.EqualTo("test user info address/User/UsersById"));
        }
        #endregion

        #region GetUserInfoByUserEmailEndAddressPoint() Tests
        [Test]
        public void GivenADependencyServiceConfigurationPresenter_WhenIGetUserInfoByUserEmailEndAddressPoint_ThenTheExpectedAddressIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UsersApi" }, "api_address"))
                .Returns("test user info address");

            var result = testItems.UsersServiceConfigurationPresenter.GetUserInfoByUserEmailEndAddressPoint();

            Assert.That(result, Is.EqualTo("test user info address/User/UserByEmail"));
        }
        #endregion

        #region GetUserOrganisationByOrganisationIdEndPoint() Tests
        [Test]
        public void GivenADependencyServiceConfigurationPresenter_WhenIGetUserOrganisationByOrganisationIdEndPoint_ThenTheExpectedAddressIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UsersApi" }, "api_address"))
                .Returns("test user info address");

            var result = testItems.UsersServiceConfigurationPresenter.GetUserOrganisationByOrganisationIdEndPoint();

            Assert.That(result, Is.EqualTo("test user info address/Organisations"));
        }
        #endregion

        #region GetAllSettings() Tests
        [Test]
        public void GivenAUsersServiceConfigurationPresenter_WhenIGetAllSettings_ThenAllSettingsValuesAreRetrieved()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UsersApi" }, "api_address"))
                .Returns("test user info address");

            var result = testItems.UsersServiceConfigurationPresenter.GetAllSettings().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(5).Items);

                Assert.That(result.Any(x =>
                        x is {Description: "Get User Info By Token EndPoint", Value: "test user info address/User/userinfo"}),
                    Is.True);

                Assert.That(result.Any(x =>
                        x is {Description: "Get User Info By User Id EndPoint", Value: "test user info address/User/UserById"}),
                    Is.True);

                Assert.That(result.Any(x =>
                        x is {Description: "Get User Infos By User Ids EndPoint", Value: "test user info address/User/UsersById"}),
                    Is.True);

                Assert.That(result.Any(x =>
                        x is {Description: "Get User Info By User Email Address EndPoint", Value: "test user info address/User/UserByEmail"}),
                    Is.True);

                Assert.That(result.Any(x =>
                        x is {Description: "Get UserOrganisation By Organisation Id EndPoint", Value: "test user info address/Organisations"}),
                    Is.True);
            });
        }

        [Test]
        public void GivenGettingASettingValueFails_WhenIGetAllSettingValues_ThenAnErrorMessageIsReturned()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("oh noes!");

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UsersApi" }, "api_address"))
                .Throws(testException);

            var result = testItems.UsersServiceConfigurationPresenter.GetAllSettings().ToList();

            Assert.That(result.Any(x => x is { Description: "Get User Info By Token EndPoint", Value: "ERROR: oh noes!" }));
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockServiceConfigurationPresenter = Mock.Get(fixture.Create<IServiceConfigurationPresenter>());

            var usersServiceConfigurationPresenter = new UsersServiceConfigurationPresenter(
                mockServiceConfigurationPresenter.Object);

            return new TestItems(
                usersServiceConfigurationPresenter,
                mockServiceConfigurationPresenter);
        }

        private class TestItems(
            IUsersServiceConfigurationPresenter usersServiceConfigurationPresenter,
            Mock<IServiceConfigurationPresenter> mockServiceConfigurationPresenter)
        {
            public IUsersServiceConfigurationPresenter UsersServiceConfigurationPresenter { get; } = usersServiceConfigurationPresenter;
            public Mock<IServiceConfigurationPresenter> MockServiceConfigurationPresenter { get; } = mockServiceConfigurationPresenter;
        }
        #endregion
    }
}
