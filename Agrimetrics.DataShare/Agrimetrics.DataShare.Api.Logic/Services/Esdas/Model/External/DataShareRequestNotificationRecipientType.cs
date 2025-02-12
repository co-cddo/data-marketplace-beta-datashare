using System.ComponentModel;
using System.Runtime.Serialization;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;

public enum DataShareRequestNotificationRecipientType
{
    [EnumMember(Value = "DOMAIN")]
    [Description("Domain data share request notification address")]
    DomainDsrNotificationAddress,

    [EnumMember(Value = "ESDA_CONTACT_POINT")]
    [Description("Data description contact point address")]
    EsdaContactPointEmailAddress,

    [EnumMember(Value = "ESDA_CUSTOM")]
    [Description("Other address")]
    EsdaCustomDsrNotificationAddress
}