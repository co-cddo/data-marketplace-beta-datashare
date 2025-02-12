using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using System.Data.Common;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.AuditLogs.ParameterModels;

[TestFixture]
public class RecordDataShareRequestStatusChangeWithCommentsParametersTests
{
    [Test]
    public void GivenARecordDataShareRequestStatusChangeWithCommentsParameters_WhenISetProperties_ThenPropertiesAreSet()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var testDbConnection = fixture.Create<DbConnection>();
        var testDbTransaction = fixture.Create<DbTransaction>();
        var testDataShareRequestId = fixture.Create<Guid>();
        var testFromStatus = fixture.Create<DataShareRequestStatusType>();
        var testToStatus = fixture.Create<DataShareRequestStatusType>();
        var testChangedByUser = fixture.Create<UserIdSet>();
        var testChangedAtLocalTime = fixture.Create<DateTime>();
        var testComments = fixture.CreateMany<string>().ToList();

        var testRecordDataShareRequestStatusChangeWithCommentsParameters = new RecordDataShareRequestStatusChangeWithCommentsParameters
        {
            DbConnection = testDbConnection,
            DbTransaction = testDbTransaction,
            DataShareRequestId = testDataShareRequestId,
            FromStatus = testFromStatus,
            ToStatus = testToStatus,
            ChangedByUser = testChangedByUser,
            ChangedAtLocalTime = testChangedAtLocalTime,
            Comments = testComments
        };

        Assert.Multiple(() =>
        {
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.DbConnection, Is.SameAs(testDbConnection));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.DbTransaction, Is.SameAs(testDbTransaction));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.DataShareRequestId, Is.EqualTo(testDataShareRequestId));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.FromStatus, Is.EqualTo(testFromStatus));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.ToStatus, Is.EqualTo(testToStatus));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.ChangedByUser, Is.EqualTo(testChangedByUser));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.ChangedAtLocalTime, Is.EqualTo(testChangedAtLocalTime));
            Assert.That(testRecordDataShareRequestStatusChangeWithCommentsParameters.Comments, Is.EqualTo(testComments));
        });
    }
}
