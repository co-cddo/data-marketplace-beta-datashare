using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Microsoft.Extensions.Primitives;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.UserIdPresentation;

[TestFixture]
public class UserIdPresenterTests
{
    #region GetInitiatingUserIdToken() Tests
    [Test]
    public void GivenThereIsNoHttpContext_WhenIGetInitiatingUserIdToken_ThenAUserIdTokenAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns((HttpContext?) null);

        Assert.That(() => testItems.UserIdPresenter.GetInitiatingUserIdToken(),
            Throws.TypeOf<UserIdTokenAccessException>().With.Message.EqualTo("No Http Context"));
    }

    [Test]
    public void GivenThereAreNoAuthorizationHeaders_WhenIGetInitiatingUserIdToken_ThenAUserIdTokenAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testHttpContext = new DefaultHttpContext
        {
            Request =
            {
                Headers = { }
            }
        };

        testItems.MockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns(testHttpContext);

        Assert.That(() => testItems.UserIdPresenter.GetInitiatingUserIdToken(),
            Throws.TypeOf<UserIdTokenAccessException>().With.Message.EqualTo("No Bearer Token on current Http Context"));
    }

    [Test]
    public void GivenThereIsANonBearerUserIdTokenInTheAuthorizationHeader_WhenIGetInitiatingUserIdToken_ThenAUserIdTokenAccessExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testUserIdToken = testItems.Fixture.Create<string>();

        var testHttpContext = new DefaultHttpContext
        {
            Request =
            {
                Headers =
                {
                    {"Authorization", new StringValues(testUserIdToken)}
                }
            }
        };

        testItems.MockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns(testHttpContext);

        Assert.That(() => testItems.UserIdPresenter.GetInitiatingUserIdToken(),
            Throws.TypeOf<UserIdTokenAccessException>().With.Message.EqualTo("Id token is not a bearer token"));
    }

    [Test]
    public void GivenThereIsAnAuthorizationHeader_WhenIGetInitiatingUserIdToken_ThenTheUserIdTokenIsReturned()
    {
        var testItems = CreateTestItems();

        var testUserIdToken = testItems.Fixture.Create<string>();

        var testHttpContext = new DefaultHttpContext
        {
            Request =
            {
                Headers =
                {
                    {"Authorization", new StringValues("Bearer " + testUserIdToken)}
                }
            }
        };

        testItems.MockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns(testHttpContext);

        var result = testItems.UserIdPresenter.GetInitiatingUserIdToken();

        Assert.That(result, Is.EqualTo(testUserIdToken));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockHttpContextAccessor = Mock.Get(fixture.Freeze<IHttpContextAccessor>());

        var userIdPresenter = new UserIdPresenter(
            mockHttpContextAccessor.Object);

        return new TestItems(
            fixture,
            mockHttpContextAccessor,
            userIdPresenter);
    }

    private class TestItems(
        IFixture fixture,
        Mock<IHttpContextAccessor> mockHttpContextAccessor,
        IUserIdPresenter userIdPresenter)
    {
        public IFixture Fixture { get; } = fixture;
        public Mock<IHttpContextAccessor> MockHttpContextAccessor { get; } = mockHttpContextAccessor;
        public IUserIdPresenter UserIdPresenter { get; } = userIdPresenter;
    }
    #endregion
}
