using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;

internal class UserIdPresenter(
    IHttpContextAccessor httpContextAccessor) : IUserIdPresenter
{
    string IUserIdPresenter.GetInitiatingUserIdToken()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
            throw new UserIdTokenAccessException("No Http Context");

        var idToken = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (idToken == null)
            throw new UserIdTokenAccessException("No Bearer Token on current Http Context");

        const string bearerTokenSuffix = "Bearer ";

        if (!idToken.StartsWith(bearerTokenSuffix, StringComparison.InvariantCultureIgnoreCase))
            throw new UserIdTokenAccessException("Id token is not a bearer token");

        return idToken[bearerTokenSuffix.Length..];
    }
}