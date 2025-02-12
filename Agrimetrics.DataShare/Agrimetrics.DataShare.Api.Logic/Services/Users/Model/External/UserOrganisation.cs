using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;

[ExcludeFromCodeCoverage] // This is an external class, provided from the Users service, so should be unit tested there
public class UserOrganisation
{
    public int OrganisationId { get; set; }
    public string OrganisationName { get; set; }
    public List<UserDomain> Domains { get; set; }
}