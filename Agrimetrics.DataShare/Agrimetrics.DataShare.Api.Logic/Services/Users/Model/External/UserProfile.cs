using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;

[ExcludeFromCodeCoverage] // This is an external class, provided from the Users service, so should be unit tested there
public class UserProfile
{
    public UserInfo User { get; set; }
    public UserDomain Domain { get; set; }
    public UserOrganisation Organisation { get; set; }
    public List<Role> Roles { get; set; }
    public bool EmailNotification { get; set; }
    public bool WelcomeNotification { get; set; }
    public DateTime LastLogin { get; set; }
}