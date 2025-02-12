using System.Data;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using AutoFixture;
using Moq;

namespace Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;

public static class TestDatabaseChannelSetup
{
    public class TestDatabaseChannelResources(
        Mock<IDatabaseChannel> mockDatabaseChannel,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction)
    {
        public Mock<IDatabaseChannel> MockDatabaseChannel { get; } = mockDatabaseChannel;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
    }

    public static TestDatabaseChannelResources CreateTestableDatabaseChannelResources(
        this Mock<IDatabaseChannelCreation> mockDatabaseChannelCreation,
        IFixture fixture)
    {
        var mockDatabaseChannel = Mock.Get(fixture.Freeze<IDatabaseChannel>());
        var mockDbConnection = Mock.Get(fixture.Freeze<IDbConnection>());
        var mockDbTransaction = Mock.Get(fixture.Freeze<IDbTransaction>());

        mockDatabaseChannelCreation.Setup(x => x.CreateAsync(It.IsAny<bool>()))
            .Callback((bool beginTransaction) =>
            {
                mockDatabaseChannel.SetupGet(x => x.Connection).Returns(mockDbConnection.Object);
                mockDatabaseChannel.SetupGet(x => x.Transaction).Returns((IDbTransaction?)null);

                mockDatabaseChannel.Setup(x => x.BeginTransactionAsync())
                    .Callback(() => { mockDatabaseChannel.SetupGet(x => x.Transaction).Returns(mockDbTransaction.Object); });
                mockDatabaseChannel.Setup(x => x.CommitTransactionAsync())
                    .Callback(() => { mockDatabaseChannel.SetupGet(x => x.Transaction).Returns((IDbTransaction?)null); });
                mockDatabaseChannel.Setup(x => x.RollbackTransactionAsync())
                    .Callback(() => { mockDatabaseChannel.SetupGet(x => x.Transaction).Returns((IDbTransaction?)null); });

                if (beginTransaction)
                {
                    mockDatabaseChannel.SetupGet(x => x.Transaction).Returns(mockDbTransaction.Object);
                }
            })
            .ReturnsAsync(() => mockDatabaseChannel.Object);

        return new TestDatabaseChannelResources(
            mockDatabaseChannel,
            mockDbConnection,
            mockDbTransaction);
    }
}