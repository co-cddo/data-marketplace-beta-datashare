using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;

[ExcludeFromCodeCoverage] // This is an external class, provided from the Users service, so should be unit tested there
public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    // Other role-related properties
}