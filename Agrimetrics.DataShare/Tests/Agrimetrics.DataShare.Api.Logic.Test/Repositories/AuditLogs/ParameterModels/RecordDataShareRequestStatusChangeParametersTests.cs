using System.Data;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.AuditLogs.ParameterModels;

[TestFixture]
public class RecordDataShareRequestStatusChangeParametersTests
{
    [Test]
    public void GivenARecordDataShareRequestStatusChangeParameters_WhenISetProperties_ThenPropertiesAreSet()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var testDbConnection = fixture.Create<IDbConnection>();
        var testDbTransaction = fixture.Create<IDbTransaction>();
        var testDataShareRequestId = fixture.Create<Guid>();
        var testFromStatus = fixture.Create<DataShareRequestStatusType>();
        var testToStatus = fixture.Create<DataShareRequestStatusType>();
        var testChangedByUser = fixture.Create<UserIdSet>();
        var testChangedAtLocalTime = fixture.Create<DateTime>();

        var testRecordDataShareRequestStatusChangeParameters = new RecordDataShareRequestStatusChangeParameters
        {
            DbConnection = testDbConnection,
            DbTransaction = testDbTransaction,
            DataShareRequestId = testDataShareRequestId,
            FromStatus = testFromStatus,
            ToStatus = testToStatus,
            ChangedByUser = testChangedByUser,
            ChangedAtLocalTime = testChangedAtLocalTime
        };

        Assert.Multiple(() =>
        {
            Assert.That(testRecordDataShareRequestStatusChangeParameters.DbConnection, Is.SameAs(testDbConnection));
            Assert.That(testRecordDataShareRequestStatusChangeParameters.DbTransaction, Is.SameAs(testDbTransaction));
            Assert.That(testRecordDataShareRequestStatusChangeParameters.DataShareRequestId, Is.EqualTo(testDataShareRequestId));
            Assert.That(testRecordDataShareRequestStatusChangeParameters.FromStatus, Is.EqualTo(testFromStatus));
            Assert.That(testRecordDataShareRequestStatusChangeParameters.ToStatus, Is.EqualTo(testToStatus));
            Assert.That(testRecordDataShareRequestStatusChangeParameters.ChangedByUser, Is.EqualTo(testChangedByUser));
            Assert.That(testRecordDataShareRequestStatusChangeParameters.ChangedAtLocalTime, Is.EqualTo(testChangedAtLocalTime));
        });
    }
}