namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

public interface IUserIdSet
{
    int UserId { get; }
    int DomainId { get; }
    int OrganisationId { get; }
    bool EmailNotification { get; }
}