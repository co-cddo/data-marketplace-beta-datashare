using Agrimetrics.DataShare.Api.Controllers.Admin;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Dto.Requests.Admin;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Admin
{
    [TestFixture]
    public class AdminResponseFactoryTests
    {
        #region CreateGetAllSettingsResponse() Tests
        [Test]
        public void GivenANullGetAllSettingsRequest_WhenICreateGetAllSettingsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AdminResponseFactory.CreateGetAllSettingsResponse(
                    null!,
                    testItems.Fixture.Create<SettingValueSet>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getAllSettingsRequest"));
        }

        [Test]
        public void GivenANullSettingValueSet_WhenICreateGetAllSettingsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AdminResponseFactory.CreateGetAllSettingsResponse(
                    testItems.Fixture.Create<GetAllSettingsRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("settingValueSet"));
        }

        [Test]
        public void GivenASettingValueSet_WhenICreateGetAllSettingsResponse_ThenAGetAllSettingsResponseIsCreatedUsingTheSettingValueSet()
        {
            var testItems = CreateTestItems();

            var testSettingValueSet = testItems.Fixture.Create<SettingValueSet>();

            var result = testItems.AdminResponseFactory.CreateGetAllSettingsResponse(
                testItems.Fixture.Create<GetAllSettingsRequest>(),
                testSettingValueSet);

            Assert.That(result.SettingValues, Is.EqualTo(testSettingValueSet));
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var adminResponseFactory = new AdminResponseFactory();

            return new TestItems(fixture, adminResponseFactory);
        }

        private class TestItems(
            IFixture fixture,
            IAdminResponseFactory adminResponseFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public IAdminResponseFactory AdminResponseFactory { get; } = adminResponseFactory;
        }
        #endregion
    }
}
