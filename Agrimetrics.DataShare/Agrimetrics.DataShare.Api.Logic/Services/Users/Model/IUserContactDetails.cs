namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

public interface IUserContactDetails
{
    string UserName { get; }

    string EmailAddress { get; }
    bool EmailNotification { get; }
}