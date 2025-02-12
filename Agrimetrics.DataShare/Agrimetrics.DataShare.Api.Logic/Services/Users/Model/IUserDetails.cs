namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

public interface IUserDetails
{
    IUserIdSet UserIdSet { get; }

    IUserContactDetails UserContactDetails { get; }
}