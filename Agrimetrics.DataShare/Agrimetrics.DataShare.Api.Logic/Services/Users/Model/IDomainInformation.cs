namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

public interface IDomainInformation
{
    int DomainId { get; }

    string DomainName { get; }

    string? DataShareRequestMailboxAddress { get; }
}