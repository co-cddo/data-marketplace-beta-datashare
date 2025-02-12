using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;

[ExcludeFromCodeCoverage] // This is an external class, provided from the Users service, so should be unit tested there
public class UserDomain
{
    public int DomainId { get; set; }
    public string DomainName { get; set; }
    public string? DataShareRequestMailboxAddress { get; set; }
    // Other domain-related properties
}