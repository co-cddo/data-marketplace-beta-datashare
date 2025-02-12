using System.Text.Json;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;
using Moq;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;
using Flurl.Http.Testing;
using Flurl;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users;

[TestFixture]
public class UserProfilePresenterTests
{
    #region GetInitiatingUserIdSetAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetInitiatingUserIdSetAsync_ThenTheUserIdSetOfTheInitiatingUserIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByTokenEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserProfile = testItems.Fixture
            .Build<UserProfile>()
            .Create();

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByTokenEndPoint())
            .Returns(testUserInfoByTokenEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByTokenEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfile));

        var result = await testItems.UserProfilePresenter.GetInitiatingUserIdSetAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.UserId, Is.EqualTo(testUserProfile.User.UserId));
            Assert.That(result.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
            Assert.That(result.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
            Assert.That(result.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

            httpTest.ShouldHaveCalled(testUserInfoByTokenEndPoint)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }

    [Test]
    public void GivenGettingUserInfoWillFail_WhenIGetInitiatingUserIdSetAsync_ThenAnExternalServiceAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByTokenEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByTokenEndPoint())
            .Returns(testUserInfoByTokenEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByTokenEndPoint)
            .RespondWith("some error", 400);

        Assert.That(async () => await testItems.UserProfilePresenter.GetInitiatingUserIdSetAsync(),
            Throws.TypeOf<ExternalServiceAccessException>().With.Message.EqualTo("GetUserProfileOfInitiatingUserAsync: Failed to fetch user profile from users service: 400"));
    }
    #endregion

    #region GetInitiatingUserDetailsAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetInitiatingUserDetailsAsync_ThenTheUserDetailsOfTheInitiatingUserIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByTokenEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserProfile = testItems.Fixture
            .Build<UserProfile>()
            .Create();

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByTokenEndPoint())
            .Returns(testUserInfoByTokenEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByTokenEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfile));

        var result = await testItems.UserProfilePresenter.GetInitiatingUserDetailsAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.UserIdSet.UserId, Is.EqualTo(testUserProfile.User.UserId));
            Assert.That(result.UserIdSet.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
            Assert.That(result.UserIdSet.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
            Assert.That(result.UserIdSet.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

            Assert.That(result.UserContactDetails.EmailAddress, Is.EqualTo(testUserProfile.User.UserEmail));
            Assert.That(result.UserContactDetails.UserName, Is.EqualTo(testUserProfile.User.UserName));
            Assert.That(result.UserContactDetails.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

            httpTest.ShouldHaveCalled(testUserInfoByTokenEndPoint)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }
    #endregion

    #region GetUserDetailsByUserIdAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetUserDetailsByUserIdAsync_ThenTheUserDetailsOfTheGivenUserIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByUserIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserProfile = testItems.Fixture
            .Build<UserProfile>()
            .Create();

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByUserIdEndPoint())
            .Returns(testUserInfoByUserIdEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByUserIdEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfile));

        var testUserId = testItems.Fixture.Create<int>();

        var result = await testItems.UserProfilePresenter.GetUserDetailsByUserIdAsync(testUserId);

        Assert.Multiple(() =>
        {
            Assert.That(result.UserIdSet.UserId, Is.EqualTo(testUserProfile.User.UserId));
            Assert.That(result.UserIdSet.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
            Assert.That(result.UserIdSet.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
            Assert.That(result.UserIdSet.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

            Assert.That(result.UserContactDetails.EmailAddress, Is.EqualTo(testUserProfile.User.UserEmail));
            Assert.That(result.UserContactDetails.UserName, Is.EqualTo(testUserProfile.User.UserName));
            Assert.That(result.UserContactDetails.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

            httpTest.ShouldHaveCalled(testUserInfoByUserIdEndPoint)
                .WithQueryParam("userid", testUserId)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }

    [Test]
    public void GivenGettingUserInformationWillThrowAnException_WhenIGetUserDetailsByUserIdAsync_ThenAnExternalServiceAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByUserIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByUserIdEndPoint())
            .Returns(testUserInfoByUserIdEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByUserIdEndPoint)
            .RespondWith("some error", 400);

        var testUserId = testItems.Fixture.Create<int>();

        Assert.That(async () => await testItems.UserProfilePresenter.GetUserDetailsByUserIdAsync(testUserId),
            Throws.TypeOf<ExternalServiceAccessException>().With.Message.EqualTo("GetUserProfileByIdAsync: Failed to fetch user profile from users service: 400"));
    }
    #endregion

    #region GetUserDetailsByUserIdsAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetUserDetailsByUserIdsAsync_ThenTheUserDetailsOfTheGivenUsersAreReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfosByUserIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserIds = testItems.Fixture.CreateMany<int>().ToList();

        var testUserProfiles = testItems.Fixture
            .Build<UserProfile>()
            .CreateMany(testUserIds.Count)
            .Select((item, index) =>
            {
                item.User.UserId = testUserIds[index];
                return item;
            })
            .ToList();

        testItems.MockUsersServiceConfigurationPresenter
            .Setup(x => x.GetUserInfosByUserIdsEndPoint())
            .Returns(testUserInfosByUserIdEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfosByUserIdEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfiles));

        var result = (await testItems.UserProfilePresenter.GetUserDetailsByUserIdsAsync(testUserIds)).ToList();

        Assert.Multiple(() =>
        {
            foreach (var testUserProfile in testUserProfiles)
            {
                var resultUserDetails = result.FirstOrDefault(x => x.UserIdSet.UserId == testUserProfile.User.UserId);

                Assert.That(resultUserDetails!.UserIdSet.UserId, Is.EqualTo(testUserProfile.User.UserId));
                Assert.That(resultUserDetails.UserIdSet.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
                Assert.That(resultUserDetails.UserIdSet.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
                Assert.That(resultUserDetails.UserIdSet.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

                Assert.That(resultUserDetails.UserContactDetails.EmailAddress, Is.EqualTo(testUserProfile.User.UserEmail));
                Assert.That(resultUserDetails.UserContactDetails.UserName, Is.EqualTo(testUserProfile.User.UserName));
                Assert.That(resultUserDetails.UserContactDetails.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));
            }

            httpTest.ShouldHaveCalled(testUserInfosByUserIdEndPoint)
                .WithQueryParam("userIds", testUserIds)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }

    [Test]
    public void GivenGettingUserInformationWillFail_WhenIGetUserDetailsByUserIdsAsync_ThenAnExternalServiceAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        const string testUserInfosByUserIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserIds = testItems.Fixture.CreateMany<int>().ToList();

        testItems.MockUsersServiceConfigurationPresenter
            .Setup(x => x.GetUserInfosByUserIdsEndPoint())
            .Returns(testUserInfosByUserIdEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfosByUserIdEndPoint)
            .RespondWith("some error", 400);

        Assert.That(async () => await testItems.UserProfilePresenter.GetUserDetailsByUserIdsAsync(testUserIds),
            Throws.TypeOf<ExternalServiceAccessException>().With.Message.EqualTo("GetUserProfilesByIdsAsync: Failed to fetch user profiles from users service: 400"));
    }
    #endregion

    #region GetUserDetailsByUserEmailAddressAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetUserDetailsByUserEmailAddressAsync_ThenTheUserDetailsOfTheGivenUserEmailAddressAreReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByUserEmailAddressEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserProfile = testItems.Fixture
            .Build<UserProfile>()
            .Create();

        var testUserEmailAddress = testItems.Fixture.Create<string>();

        testItems.MockUsersServiceConfigurationPresenter
            .Setup(x => x.GetUserInfoByUserEmailEndAddressPoint())
            .Returns(testUserInfoByUserEmailAddressEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByUserEmailAddressEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfile));

        var result = await testItems.UserProfilePresenter.GetUserDetailsByUserEmailAddressAsync(testUserEmailAddress);

        Assert.Multiple(() =>
        {
            Assert.That(result!.UserIdSet.UserId, Is.EqualTo(testUserProfile.User.UserId));
            Assert.That(result.UserIdSet.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
            Assert.That(result.UserIdSet.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
            Assert.That(result.UserIdSet.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));

            Assert.That(result.UserContactDetails.EmailAddress, Is.EqualTo(testUserProfile.User.UserEmail));
            Assert.That(result.UserContactDetails.UserName, Is.EqualTo(testUserProfile.User.UserName));
            Assert.That(result.UserContactDetails.EmailNotification, Is.EqualTo(testUserProfile.EmailNotification));


            httpTest.ShouldHaveCalled(testUserInfoByUserEmailAddressEndPoint)
                .WithQueryParam("email", testUserEmailAddress)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }

    [Test]
    public async Task GivenAnUnknownEmailAddress_WhenIGetUserDetailsByUserEmailAddressAsync_ThenNullIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByUserEmailAddressEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserEmailAddress = testItems.Fixture.Create<string>();

        testItems.MockUsersServiceConfigurationPresenter
            .Setup(x => x.GetUserInfoByUserEmailEndAddressPoint())
            .Returns(testUserInfoByUserEmailAddressEndPoint);

        using var httpTest = new HttpTest();

        var result = await testItems.UserProfilePresenter.GetUserDetailsByUserEmailAddressAsync(testUserEmailAddress);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Null);

            httpTest.ShouldHaveCalled(testUserInfoByUserEmailAddressEndPoint)
                .WithQueryParam("email", testUserEmailAddress)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }

    [Test]
    public async Task GivenGettingUserInformationWillFail_WhenIGetUserDetailsByUserEmailAddressAsync_ThenNullIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByUserEmailAddressEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserEmailAddress = testItems.Fixture.Create<string>();

        testItems.MockUsersServiceConfigurationPresenter
            .Setup(x => x.GetUserInfoByUserEmailEndAddressPoint())
            .Returns(testUserInfoByUserEmailAddressEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByUserEmailAddressEndPoint)
            .RespondWith("some error", 400);

        var result = await testItems.UserProfilePresenter.GetUserDetailsByUserEmailAddressAsync(testUserEmailAddress);

        Assert.That(result, Is.Null);
    }
    #endregion

    #region GetInitiatingUserOrganisationInformationAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetInitiatingUserOrganisationInformationAsync_ThenTheOrganisationInformationOfTheInitiatingUserIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserInfoByTokenEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserProfile = testItems.Fixture
            .Build<UserProfile>()
            .Create();

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByTokenEndPoint())
            .Returns(testUserInfoByTokenEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserInfoByTokenEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfile));

        var result = await testItems.UserProfilePresenter.GetInitiatingUserOrganisationInformationAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
            Assert.That(result.OrganisationName, Is.EqualTo(testUserProfile.Organisation.OrganisationName));

            var resultDomain = result.Domains.Single();
            Assert.That(resultDomain.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
            Assert.That(resultDomain.DomainName, Is.EqualTo(testUserProfile.Domain.DomainName));
            Assert.That(resultDomain.DataShareRequestMailboxAddress, Is.EqualTo(testUserProfile.Domain.DataShareRequestMailboxAddress));

            httpTest.ShouldHaveCalled(testUserInfoByTokenEndPoint)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }
    #endregion

    #region GetOrganisationInformationByUserIdAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetOrganisationInformationByUserIdAsync_ThenTheOrganisationInformationOfTheUserWithTheGivenIdIsReturned()
    {
        var testItems = CreateTestItems();

        const string testGetUserInfoByUserIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserProfile = testItems.Fixture
            .Build<UserProfile>()
            .Create();

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserInfoByUserIdEndPoint())
            .Returns(testGetUserInfoByUserIdEndPoint);

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testGetUserInfoByUserIdEndPoint)
            .RespondWith(JsonSerializer.Serialize(testUserProfile));

        var testUserId = testItems.Fixture.Create<int>();

        var result = await testItems.UserProfilePresenter.GetOrganisationInformationByUserIdAsync(testUserId);

        Assert.Multiple(() =>
        {
            Assert.That(result.OrganisationId, Is.EqualTo(testUserProfile.Organisation.OrganisationId));
            Assert.That(result.OrganisationName, Is.EqualTo(testUserProfile.Organisation.OrganisationName));

            var resultDomain = result.Domains.Single();
            Assert.That(resultDomain.DomainId, Is.EqualTo(testUserProfile.Domain.DomainId));
            Assert.That(resultDomain.DomainName, Is.EqualTo(testUserProfile.Domain.DomainName));
            Assert.That(resultDomain.DataShareRequestMailboxAddress, Is.EqualTo(testUserProfile.Domain.DataShareRequestMailboxAddress));

            httpTest.ShouldHaveCalled(testGetUserInfoByUserIdEndPoint)
                .WithQueryParam("userid", testUserId)
                .WithOAuthBearerToken(testUserIdToken);
        });
    }
    #endregion

    #region GetOrganisationDetailsByOrganisationIdAsync() Tests
    [Test]
    public async Task GivenAUserProfilePresenter_WhenIGetOrganisationDetailsByOrganisationIdAsync_ThenTheOrganisationInformationOfTheUserWithTheGivenIdIsReturned()
    {
        var testItems = CreateTestItems();

        const string testUserOrganisationByOrganisationIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        var testUserOrganisation = testItems.Fixture
            .Build<UserOrganisation>()
            .Create();

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserOrganisationByOrganisationIdEndPoint())
            .Returns(testUserOrganisationByOrganisationIdEndPoint);

        var testOrganisationId = testItems.Fixture.Create<int>();

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserOrganisationByOrganisationIdEndPoint.AppendPathSegment(testOrganisationId))
            .RespondWith(JsonSerializer.Serialize(testUserOrganisation));
        
        var result = await testItems.UserProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(testOrganisationId);

        Assert.Multiple(() =>
        {
            Assert.That(result.OrganisationId, Is.EqualTo(testUserOrganisation.OrganisationId));
            Assert.That(result.OrganisationName, Is.EqualTo(testUserOrganisation.OrganisationName));

            foreach (var testDomain in testUserOrganisation.Domains)
            {
                var resultDomain = result.Domains.FirstOrDefault(x => x.DomainName == testDomain.DomainName);

                Assert.That(resultDomain!.DomainId, Is.EqualTo(testDomain.DomainId));
                Assert.That(resultDomain.DomainName, Is.EqualTo(testDomain.DomainName));
                Assert.That(resultDomain.DataShareRequestMailboxAddress, Is.EqualTo(testDomain.DataShareRequestMailboxAddress));
            }

            httpTest.ShouldHaveCalled(testUserOrganisationByOrganisationIdEndPoint.AppendPathSegment(testOrganisationId))
                .WithOAuthBearerToken(testUserIdToken);
        });
    }

    [Test]
    public void GivenGettingOrganisationInformationWillFail_WhenIGetOrganisationDetailsByOrganisationIdAsync_ThenAnExternalServiceAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        const string testUserOrganisationByOrganisationIdEndPoint = "http://test_endpoint";
        const string testUserIdToken = "test user id token";

        testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
            .Returns(() => testUserIdToken);

        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetUserOrganisationByOrganisationIdEndPoint())
            .Returns(testUserOrganisationByOrganisationIdEndPoint);

        var testOrganisationId = testItems.Fixture.Create<int>();

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(testUserOrganisationByOrganisationIdEndPoint.AppendPathSegment(testOrganisationId))
            .RespondWith("some error", 400);

        Assert.That(async () => await testItems.UserProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(testOrganisationId),
            Throws.TypeOf<ExternalServiceAccessException>().With.Message.EqualTo("GetUserOrganisationByOrganisationId: Failed to get user organisation from users service: 400"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockUserIdPresenter = Mock.Get(fixture.Freeze<IUserIdPresenter>());
        var mockUsersServiceConfigurationPresenter = Mock.Get(fixture.Freeze<IUsersServiceConfigurationPresenter>());

        var userProfilePresenter = new UserProfilePresenter(
            mockUserIdPresenter.Object,
            mockUsersServiceConfigurationPresenter.Object);

        return new TestItems(
            fixture,
            userProfilePresenter,
            mockUserIdPresenter,
            mockUsersServiceConfigurationPresenter);
    }

    private class TestItems(
        IFixture fixture,
        IUserProfilePresenter userProfilePresenter,
        Mock<IUserIdPresenter> mockUserIdPresenter,
        Mock<IUsersServiceConfigurationPresenter> mockUsersServiceConfigurationPresenter)
    {
        public IFixture Fixture { get; } = fixture;

        public IUserProfilePresenter UserProfilePresenter { get; } = userProfilePresenter;

        public Mock<IUserIdPresenter> MockUserIdPresenter { get; } = mockUserIdPresenter;

        public Mock<IUsersServiceConfigurationPresenter> MockUsersServiceConfigurationPresenter { get; } = mockUsersServiceConfigurationPresenter;
    }
    #endregion
}
