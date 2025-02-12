namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

internal class UserDetails : IUserDetails
{
    public required IUserIdSet UserIdSet { get; init; }

    public required IUserContactDetails UserContactDetails { get; init; }
}