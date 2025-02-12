using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;

[ExcludeFromCodeCoverage] // This is an external class, provided from the Users service, so should be unit tested there
public class UserInfo
{
    public int UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    // Other user-related properties
}