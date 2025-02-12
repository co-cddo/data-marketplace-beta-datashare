namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

internal class OrganisationInformation : IOrganisationInformation
{
    public required int OrganisationId { get; init; }

    public required string OrganisationName { get; init; }

    public required IEnumerable<IDomainInformation> Domains { get; init; }
}