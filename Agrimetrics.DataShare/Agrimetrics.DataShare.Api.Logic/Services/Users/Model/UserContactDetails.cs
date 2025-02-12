namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

internal class UserContactDetails : IUserContactDetails
{
    public required string UserName { get; init; }

    public required string EmailAddress { get; init; }

    public bool EmailNotification { get; init; }
}