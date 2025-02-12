using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Data;
using Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.AcquirerDataShareRequests;

[TestFixture]
public class AcquirerDataShareRequestRepositoryTests
{
    #region FindQuestionSetAsync() Tests
    [Test]
    public async Task GivenQuestionSetDetails_WhenIFindQuestionSetAsync_ThenTheIdOfTheMatchingQuestionSetIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetMasterQuestionSetId)
            .Returns("test sql query");

        var testMasterQuestionSetId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                null))
            .ReturnsAsync(() => testMasterQuestionSetId);

        var result = await testItems.AcquirerDataShareRequestRepository.FindQuestionSetAsync(
            testItems.Fixture.Create<int?>(),
            testItems.Fixture.Create<int>(),
            testItems.Fixture.Create<Guid>());

        Assert.That(result, Is.EqualTo(testMasterQuestionSetId));
    }

    [Test]
    public void GivenFindingAQuestionSetWillFail_WhenIFindQuestionSetAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<Guid>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                null))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.FindQuestionSetAsync(
            testItems.Fixture.Create<int?>(),
            testItems.Fixture.Create<int>(),
            testItems.Fixture.Create<Guid>()),
                Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to FindQuestionSet in database"));
    }
    #endregion

    #region GetQuestionSetOutlineRequestAsync() Tests
    [Test]
    public async Task GivenAQuestionSetId_WhenIGetQuestionSetOutlineRequestAsync_ThenTheQuestionSetOutlineIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetQuestionSetOutlineRequest)
            .Returns("test sql query");

        var testQuestionSetOutlineModelDatas = testItems.Fixture
            .Build<QuestionSetOutlineModelData>()
            .CreateMany(1);

        var testQuestionSetId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    QuestionSetOutlineModelData,
                    QuestionSetSectionOutlineModelData,
                    QuestionSetQuestionOutlineModelData,
                    QuestionSetOutlineModelData>>(),
                It.IsAny<string>(),
                    It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    QuestionSetOutlineModelData,
                    QuestionSetSectionOutlineModelData,
                    QuestionSetQuestionOutlineModelData,
                    QuestionSetOutlineModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return dynamicParameters.QuestionSetId == testQuestionSetId
                    ? testQuestionSetOutlineModelDatas
                    : [];
            });

        var result = await testItems.AcquirerDataShareRequestRepository.GetQuestionSetOutlineRequestAsync(
            testQuestionSetId);

        Assert.That(result.QuestionSetOutline_Id, Is.EqualTo(testQuestionSetOutlineModelDatas.Single().QuestionSetOutline_Id));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIGetQuestionSetOutlineRequestAsync_ThenMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetQuestionSetOutlineRequest)
            .Returns("test sql query");

        var testQuestionSetOutlineModelDatas = testItems.Fixture
            .Build<QuestionSetOutlineModelData>()
            .CreateMany(1);

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    QuestionSetOutlineModelData,
                    QuestionSetSectionOutlineModelData,
                    QuestionSetQuestionOutlineModelData,
                    QuestionSetOutlineModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    QuestionSetOutlineModelData,
                    QuestionSetSectionOutlineModelData,
                    QuestionSetQuestionOutlineModelData,
                    QuestionSetOutlineModelData> mappingFunction,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunction(
                    testItems.Fixture.Create<QuestionSetOutlineModelData>(),
                    testItems.Fixture.Create<QuestionSetSectionOutlineModelData>(),
                    testItems.Fixture.Create<QuestionSetQuestionOutlineModelData>());
            })
            .ReturnsAsync(() => testQuestionSetOutlineModelDatas);

        await testItems.AcquirerDataShareRequestRepository.GetQuestionSetOutlineRequestAsync(It.IsAny<Guid>());

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }

    [Test]
    public void GivenGettingAQuestionSetOutlineWillFail_WhenIGetQuestionSetOutlineRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    QuestionSetOutlineModelData,
                    QuestionSetSectionOutlineModelData,
                    QuestionSetQuestionOutlineModelData,
                    QuestionSetOutlineModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetQuestionSetOutlineRequestAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetQuestionSetOutlineRequest"));
    }
    #endregion

    #region GetDataShareRequestResourceNameAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestResourceNameAsync_ThenTheNameOfTheRequestedResourceIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetEsdaNameForDataShareRequest)
            .Returns("test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResourceName = testItems.Fixture.Create<string>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<string>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return dynamicParameters.DataShareRequestId == testDataShareRequestId
                    ? testResourceName
                    : string.Empty;
            });

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestResourceNameAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testResourceName));
    }

    [Test]
    public void GivenGettingARequestedResourceNameWillFail_WhenIGetDataShareRequestResourceNameAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<string>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestResourceNameAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestResourceName from database"));
    }
    #endregion

    #region StartDataShareRequestAsync() Tests
    [Test]
    public async Task GivenParameters_WhenIStartDataShareRequestAsync_ThenTheIdOfTheStartedRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testSupplierDomainId = testItems.Fixture.Create<int>();
        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testEsdaId = testItems.Fixture.Create<Guid>();
        var testEsdaName = testItems.Fixture.Create<string>();
        var testQuestionSetId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Part Retrieval
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetQuestionSetQuestionParts)
            .Returns("test question parts sql query");

        var testQuestionSetQuestionModelDatas = testItems.Fixture.CreateMany<QuestionSetQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test question parts sql query",
            It.IsAny<Func<QuestionSetQuestionModelData, QuestionSetQuestionPartModelData, QuestionSetQuestionModelData>>(),
            It.IsAny<string>(),
            It.IsAny<object?>()))
            .ReturnsAsync(() => testQuestionSetQuestionModelDatas);
        #endregion

        #region Set Up Request Creation
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CreateDataShareRequest)
            .Returns("test sql command");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()
            ))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.AcquirerUserId == testAcquirerUserIdSet.UserId &&
                    dynamicParameters.AcquirerDomainId == testAcquirerUserIdSet.DomainId &&
                    dynamicParameters.AcquirerOrganisationId == testAcquirerUserIdSet.OrganisationId &&
                    dynamicParameters.EsdaId == testEsdaId &&
                    dynamicParameters.EsdaName == testEsdaName &&
                    dynamicParameters.SupplierDomainId == testSupplierDomainId &&
                    dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId &&
                    dynamicParameters.QuestionSetId == testQuestionSetId
                        ? testDataShareRequestId
                        : Guid.Empty;
            });
        #endregion

        var result = await testItems.AcquirerDataShareRequestRepository.StartDataShareRequestAsync(
            testAcquirerUserIdSet,
            testSupplierDomainId,
            testSupplierOrganisationId,
            testEsdaId,
            testEsdaName,
            testQuestionSetId);

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIStartDataShareRequestAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testSupplierDomainId = testItems.Fixture.Create<int>();
        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testEsdaId = testItems.Fixture.Create<Guid>();
        var testEsdaName = testItems.Fixture.Create<string>();
        var testQuestionSetId = testItems.Fixture.Create<Guid>();

        #region Set Up Data Retrieval
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CreateDataShareRequest)
            .Returns("test sql command");
        
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()
            ))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.AcquirerUserId == testAcquirerUserIdSet.UserId &&
                    dynamicParameters.AcquirerDomainId == testAcquirerUserIdSet.DomainId &&
                    dynamicParameters.AcquirerOrganisationId == testAcquirerUserIdSet.OrganisationId &&
                    dynamicParameters.EsdaId == testEsdaId &&
                    dynamicParameters.EsdaName == testEsdaName &&
                    dynamicParameters.SupplierDomainId == testSupplierDomainId &&
                    dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId &&
                    dynamicParameters.QuestionSetId == testQuestionSetId
                        ? testDataShareRequestId
                        : Guid.Empty;
            });
        #endregion

        #region Set Up Mapping Functions
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetQuestionSetQuestionParts)
            .Returns("test question parts sql query");

        var getQuestionPartsMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question parts sql query",
                It.IsAny<Func<QuestionSetQuestionModelData, QuestionSetQuestionPartModelData, QuestionSetQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<QuestionSetQuestionModelData, QuestionSetQuestionPartModelData, QuestionSetQuestionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                getQuestionPartsMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<QuestionSetQuestionModelData>(),
                    testItems.Fixture.Create<QuestionSetQuestionPartModelData>());
            });
        #endregion

        await testItems.AcquirerDataShareRequestRepository.StartDataShareRequestAsync(
            testAcquirerUserIdSet,
            testSupplierDomainId,
            testSupplierOrganisationId,
            testEsdaId,
            testEsdaName,
            testQuestionSetId);

        Assert.That(getQuestionPartsMappingFunctionHasBeenRun, Is.True);
    }

    [Test]
    public void GivenStartingADataShareRequestWillFail_WhenIStartDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()
            ))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.StartDataShareRequestAsync(
            testItems.Fixture.Create<UserIdSet>(),
            testItems.Fixture.Create<int>(),
            testItems.Fixture.Create<int>(),
            testItems.Fixture.Create<Guid>(),
            testItems.Fixture.Create<string>(),
            testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to StartDataShareRequest"));
    }

    [Test]
    public void GivenGettingQuestionSetPartsWillFail_WhenIStartDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<QuestionSetQuestionModelData, QuestionSetQuestionPartModelData, QuestionSetQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.StartDataShareRequestAsync(
                testItems.Fixture.Create<UserIdSet>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>(),
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to StartDataShareRequest"));
    }

    [Test]
    public void GivenRecordingTheAuditEntryWillFail_WhenIStartDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAuditLogRepository.Setup(x => x.RecordDataShareRequestStatusChangeAsync(
                It.IsAny<RecordDataShareRequestStatusChangeParameters>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.StartDataShareRequestAsync(
                testItems.Fixture.Create<UserIdSet>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>(),
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to StartDataShareRequest"));
    }
    #endregion

    #region GetDataShareRequestStatusAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestStatusAsync_ThenTheRequestStatusIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestStatusTypeModelData = testItems.Fixture.Create<DataShareRequestStatusTypeModelData>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return dynamicParameters.DataShareRequestId == testDataShareRequestId
                    ? testDataShareRequestStatusTypeModelData
                    : new DataShareRequestStatusTypeModelData();
            });

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testDataShareRequestStatusTypeModelData));
    }

    [Test]
    public void GivenGettingARequestStatusWillFail_WhenIGetDataShareRequestStatusAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestStatus from database"));
    }
    #endregion

    #region UpdateDataShareRequestQuestionStatusesAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIUpdateDataShareRequestQuestionStatusesAsync_ThenQuestionStatusesAreUpdated()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionsRemainThatRequireAResponse = testItems.Fixture.Create<bool>();
        var testDataShareRequestQuestionStatuses = testItems.Fixture
            .Build<DataShareRequestQuestionStatusDataModel>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.UpdateDataShareRequestCompleteness)
            .Returns("test sql command");

        var dataShareCompletenessHasBeenUpdated = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                    testItems.MockDbConnection.Object,
                    testItems.MockDbTransaction.Object,
                    "test sql command",
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                dataShareCompletenessHasBeenUpdated =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionsRemainThatRequireAResponse == testQuestionsRemainThatRequireAResponse;
            });

        await testItems.AcquirerDataShareRequestRepository.UpdateDataShareRequestQuestionStatusesAsync(
            testDataShareRequestId,
            testQuestionsRemainThatRequireAResponse,
            testDataShareRequestQuestionStatuses);

        Assert.That(dataShareCompletenessHasBeenUpdated, Is.True);
    }

    [Test]
    public void GivenSettingQuestionStatusesWillFail_WhenIUpdateDataShareRequestQuestionStatusesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.UpdateDataShareRequestQuestionStatusesAsync(
                    testItems.Fixture.Create<Guid>(),
                    testItems.Fixture.Create<bool>(),
                    testItems.Fixture
                        .CreateMany<DataShareRequestQuestionStatusDataModel>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to UpdateDataShareRequestQuestionStatuses in database"));
    }
    #endregion

    #region GetDataShareRequestsAsync() Tests
    [Test]
    public async Task GivenFilterParameters_WhenIGetDataShareRequestsAsync_ThenTheMatchingDataShareRequestsAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestModelData)
            .Returns("test sql query");

        var testAcquirerUserId = testItems.Fixture.Create<int>();
        var testAcquirerDomainId = testItems.Fixture.Create<int>();
        var testAcquirerOrganisationId = testItems.Fixture.Create<int>();
        var testSupplierDomainId = testItems.Fixture.Create<int>();
        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testEsdaId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestStatuses = testItems.Fixture.CreateMany<DataShareRequestStatus>();

        var testDataShareRequestModelDatas = testItems.Fixture.CreateMany<DataShareRequestModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<DataShareRequestModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.AcquirerUserId == testAcquirerUserId &&
                    dynamicParameters.AcquirerDomainId == testAcquirerDomainId &&
                    dynamicParameters.AcquirerOrganisationId == testAcquirerOrganisationId &&
                    dynamicParameters.SupplierDomainId == testSupplierDomainId &&
                    dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId &&
                    dynamicParameters.EsdaId == testEsdaId
                    ? testDataShareRequestModelDatas
                    : [];
            });

        var result = (await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestsAsync(
            testAcquirerUserId,
            testAcquirerDomainId,
            testAcquirerOrganisationId,
            testSupplierDomainId,
            testSupplierOrganisationId,
            testEsdaId,
            testDataShareRequestStatuses)).ToList();

        Assert.That(result, Is.EqualTo(testDataShareRequestModelDatas));
    }

    [Test]
    public void GivenGettingDataShareRequestsWillFail_WhenIGetDataShareRequestsAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<DataShareRequestModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestsAsync(
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<int>(),
                testItems.Fixture.Create<Guid>(),
                null),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequests from database"));
    }
    #endregion

    #region GetDataShareRequestQuestionsSummaryAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenTheQuestionsSummaryOfTheRequestAreReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestQuestionsSummaryModelDatas =
            testItems.Fixture.CreateMany<DataShareRequestQuestionsSummaryModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionsSummaryModelData,
                    QuestionSetSummaryModelData,
                    QuestionSetSectionSummaryModelData,
                    QuestionSummaryModelData,
                    DataShareRequestQuestionsSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionsSummaryModelData,
                    QuestionSetSummaryModelData,
                    QuestionSetSectionSummaryModelData,
                    QuestionSummaryModelData,
                    DataShareRequestQuestionsSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return dynamicParameters.DataShareRequestId == testDataShareRequestId
                    ? testDataShareRequestQuestionsSummaryModelDatas
                    : [];
            });

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionsSummaryAsync(
            testDataShareRequestId);

        Assert.That(result.DataShareRequest_Id, Is.EqualTo(testDataShareRequestQuestionsSummaryModelDatas.First().DataShareRequest_Id));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestQuestionsSummaryModelDatas =
            testItems.Fixture.CreateMany<DataShareRequestQuestionsSummaryModelData>().ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionsSummaryModelData,
                    QuestionSetSummaryModelData,
                    QuestionSetSectionSummaryModelData,
                    QuestionSummaryModelData,
                    DataShareRequestQuestionsSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionsSummaryModelData,
                    QuestionSetSummaryModelData,
                    QuestionSetSectionSummaryModelData,
                    QuestionSummaryModelData,
                    DataShareRequestQuestionsSummaryModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestQuestionsSummaryModelData>(),
                    testItems.Fixture.Create<QuestionSetSummaryModelData>(),
                    testItems.Fixture.Create<QuestionSetSectionSummaryModelData>(),
                    testItems.Fixture.Create<QuestionSummaryModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestQuestionsSummaryModelDatas);

        await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionsSummaryAsync(
            testDataShareRequestId);

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }

    [Test]
    public void GivenGettingAQuestionSummaryWillFail_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionsSummaryModelData,
                    QuestionSetSummaryModelData,
                    QuestionSetSectionSummaryModelData,
                    QuestionSummaryModelData,
                    DataShareRequestQuestionsSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionsSummaryAsync(
            testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestQuestionsSummary from database"));
    }
    #endregion

    #region GetDataShareRequestQuestionsSummaryAsync() Tests
    [Test]
    [TestCase(QuestionPartResponseFormatType.Text)]
    [TestCase(QuestionPartResponseFormatType.Numeric)]
    [TestCase(QuestionPartResponseFormatType.Date)]
    [TestCase(QuestionPartResponseFormatType.Time)]
    [TestCase(QuestionPartResponseFormatType.DateTime)]
    [TestCase(QuestionPartResponseFormatType.Country)]
    public async Task GivenAFreeFormQuestion_WhenIGetDataShareRequestQuestionAsync_ThenTheQuestionIsReturned(
        QuestionPartResponseFormatType testFormatType)
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, testFormatType)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .With(x => x.QuestionPart_QuestionPartType, QuestionPartType.MainQuestionPart)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        #region Set Up Answer Part
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseItemId)
            .Returns("test answer part sql query");

        var testAnswerPartId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleOrDefaultAsync<Guid?>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer part sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testAnswerPartId);
        #endregion

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.That(result.DataShareRequestQuestion_DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionModelDatas.First().DataShareRequestQuestion_DataShareRequestId));
    }

    [Test]
    public void GivenAFreeFormQuestionWithInvalidFormatType_WhenIGetDataShareRequestQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var invalidFormatType =
            (QuestionPartResponseFormatType) Enum.GetValues<QuestionPartResponseFormatType>().Cast<int>().Max() + 1;

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, invalidFormatType)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetDataShareRequestQuestion from database"));
    }

    [Test]
    public void GivenAFreeFormQuestionWithInvalidQuestionPartType_WhenIGetDataShareRequestQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.Text)
            .Create();

        var invalidQuestionPartType = (QuestionPartType) Enum.GetValues<QuestionPartType>().Cast<int>().Max() + 1;

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .With(x => x.QuestionPart_QuestionPartType, invalidQuestionPartType)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetDataShareRequestQuestion from database"));
    }

    [Test]
    public async Task GivenASingleSelectOptionSelectionQuestionWithSupplementaryAnswer_WhenIGetDataShareRequestQuestionAsync_ThenTheQuestionIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .With(x => x.QuestionPartAnswerItem_InputType, QuestionPartResponseInputType.OptionSelection)
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.SelectSingle)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .With(x => x.QuestionPart_QuestionPartType, QuestionPartType.MainQuestionPart)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        #region Set Up Selection Option
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSelectionOptionSingleValueModelData)
            .Returns("test single select options sql query");

        var testSelectionItemForSingleSelectionModelDatas = testItems.Fixture
            .CreateMany<QuestionPartOptionSelectionItemForSingleSelectionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<QuestionPartOptionSelectionItemForSingleSelectionModelData>(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test single select options sql query",
            It.IsAny<object?>()))
            .ReturnsAsync(() => testSelectionItemForSingleSelectionModelDatas);

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetQuestionPartAnswerResponseItemOptionSelectionModelData)
            .Returns("test item option selection sql query");

        var testSelectedOptionItems = testItems.Fixture
            .Build<QuestionPartAnswerItemSelectionOptionItemModelData>()
            .CreateMany(1).ToList();

        var testItemOptionSelectionModelDatas = testItems.Fixture
            .Build<QuestionPartAnswerResponseItemOptionSelectionModelData>()
            .With(x => x.QuestionPartAnswerItem_SelectedOptionItems, testSelectedOptionItems)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test item option selection sql query",
                It.IsAny<Func<
                    QuestionPartAnswerResponseItemOptionSelectionModelData,
                    QuestionPartAnswerItemSelectionOptionItemModelData,
                    QuestionPartAnswerResponseItemOptionSelectionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testItemOptionSelectionModelDatas);
        #endregion

        #region Set Up Supplementary Question
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSupplementaryQuestionPartModelData)
            .Returns("test supplementary question part sql query");

        var testSupplementaryMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, true)
            .Create();

        var testSupplementaryResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.Text)
            .Create();

        var testSupplementaryQuestionPartModelData = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testSupplementaryMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testSupplementaryResponseTypeInformation)
            .With(x => x.QuestionPart_QuestionPartType, QuestionPartType.SupplementaryQuestionPart)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test supplementary question part sql query",
                It.IsAny<Func<
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testSupplementaryQuestionPartModelData);
        #endregion

        #region Set Up Answer Part
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseItemId)
            .Returns("test answer part sql query");

        var testAnswerPartId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleOrDefaultAsync<Guid?>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer part sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testAnswerPartId);
        #endregion

        #region Set Up Supplementary Answer Part
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSupplementaryQuestionAnswerPartAnswerModelData)
            .Returns("test supplementary answer part sql query");

        var testSupplementaryAnswerPartResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .With(x => x.QuestionPartAnswerResponse_ResponseItem, (QuestionPartAnswerResponseItemModelData?) null)
            .CreateMany(1).ToList();

        var testSupplementaryAnswerResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .With(x => x.QuestionPartAnswerItem_InputType, QuestionPartResponseInputType.FreeForm)
            .CreateMany(1).ToList();

        var testSupplementaryAnswerModelDatas = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testSupplementaryAnswerPartResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testSupplementaryAnswerResponseInformations)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test supplementary answer part sql query",
                It.IsAny<Func<
                    QuestionPartAnswerModelData,
                    QuestionPartAnswerResponseInformationModelData,
                    QuestionPartAnswerModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    QuestionPartAnswerModelData,
                    QuestionPartAnswerResponseInformationModelData,
                    QuestionPartAnswerModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunc(
                    testItems.Fixture.Create<QuestionPartAnswerModelData>(),
                    testItems.Fixture.Create<QuestionPartAnswerResponseInformationModelData>());
            })
            .ReturnsAsync(() => testSupplementaryAnswerModelDatas);
        #endregion

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.That(result.DataShareRequestQuestion_DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionModelDatas.First().DataShareRequestQuestion_DataShareRequestId));
    }

    [Test]
    public async Task GivenASingleSelectOptionSelectionQuestionWithNoSupplementaryAnswer_WhenIGetDataShareRequestQuestionAsync_ThenTheQuestionIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .With(x => x.QuestionPartAnswerItem_InputType, QuestionPartResponseInputType.OptionSelection)
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.SelectSingle)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        #region Set Up Selection Option
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSelectionOptionSingleValueModelData)
            .Returns("test single select options sql query");

        var testSelectionItemForSingleSelectionModelDatas = testItems.Fixture
            .Build<QuestionPartOptionSelectionItemForSingleSelectionModelData>()
            .With(x => x.OptionSelectionItem_SupplementaryQuestionPartId, (Guid?) null)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<QuestionPartOptionSelectionItemForSingleSelectionModelData>(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test single select options sql query",
            It.IsAny<object?>()))
            .ReturnsAsync(() => testSelectionItemForSingleSelectionModelDatas);

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetQuestionPartAnswerResponseItemOptionSelectionModelData)
            .Returns("test item option selection sql query");

        var testSelectedOptionItems = testItems.Fixture
            .Build<QuestionPartAnswerItemSelectionOptionItemModelData>()
            .With(x => x.QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId, (Guid?) null)
            .CreateMany(1).ToList();

        var testItemOptionSelectionModelDatas = testItems.Fixture
            .Build<QuestionPartAnswerResponseItemOptionSelectionModelData>()
            .With(x => x.QuestionPartAnswerItem_SelectedOptionItems, testSelectedOptionItems)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test item option selection sql query",
                It.IsAny<Func<
                    QuestionPartAnswerResponseItemOptionSelectionModelData,
                    QuestionPartAnswerItemSelectionOptionItemModelData,
                    QuestionPartAnswerResponseItemOptionSelectionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    QuestionPartAnswerResponseItemOptionSelectionModelData,
                    QuestionPartAnswerItemSelectionOptionItemModelData,
                    QuestionPartAnswerResponseItemOptionSelectionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunc(
                    testItems.Fixture.Create<QuestionPartAnswerResponseItemOptionSelectionModelData>(),
                    testItems.Fixture.Create<QuestionPartAnswerItemSelectionOptionItemModelData>());
            })
            .ReturnsAsync(() => testItemOptionSelectionModelDatas);
        #endregion

        #region Set Up Answer Part
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseItemId)
            .Returns("test answer part sql query");

        var testAnswerPartId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleOrDefaultAsync<Guid?>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer part sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testAnswerPartId);
        #endregion

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.That(result.DataShareRequestQuestion_DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionModelDatas.First().DataShareRequestQuestion_DataShareRequestId));
    }

    [Test]
    public async Task GivenASingleSelectOptionSelectionQuestion_WhenIGetDataShareRequestQuestionAsync_ThenMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.SelectSingle)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .With(x => x.QuestionPart_QuestionPartType, QuestionPartType.MainQuestionPart)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        #region Set Up Selection Option
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSelectionOptionSingleValueModelData)
            .Returns("test single select options sql query");

        var testSelectionItemForSingleSelectionModelDatas = testItems.Fixture
            .CreateMany<QuestionPartOptionSelectionItemForSingleSelectionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<QuestionPartOptionSelectionItemForSingleSelectionModelData>(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test single select options sql query",
            It.IsAny<object?>()))
            .ReturnsAsync(() => testSelectionItemForSingleSelectionModelDatas);
        #endregion

        #region Set Up Supplementary Question
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSupplementaryQuestionPartModelData)
            .Returns("test supplementary question part sql query");

        var testSupplementaryMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testSupplementaryResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.Text)
            .Create();

        var testSupplementaryQuestionPartModelData = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testSupplementaryMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testSupplementaryResponseTypeInformation)
            .With(x => x.QuestionPart_QuestionPartType, QuestionPartType.SupplementaryQuestionPart)
            .CreateMany(1).ToList();

        var optionSelectionMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test supplementary question part sql query",
                It.IsAny<Func<
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartModelData> mappingFunc,
                string _,
                object? _) =>
            {
                optionSelectionMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<QuestionPartModelData>(),
                    testItems.Fixture.Create<QuestionPartPromptsModelData>(),
                    testItems.Fixture.Create<QuestionPartMultipleAnswerItemControlModelData>(),
                    testItems.Fixture.Create<QuestionPartResponseTypeInformationModelData>());
            })
            .ReturnsAsync(() => testSupplementaryQuestionPartModelData);
        #endregion

        await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.That(optionSelectionMappingFunctionHasBeenRun);
    }

    [Test]
    public async Task GivenAMultiSelectOptionSelectionQuestion_WhenIGetDataShareRequestQuestionAsync_ThenTheQuestionIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.SelectMulti)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        #region Set Up Selection Option
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSelectionOptionMultiValueModelData)
            .Returns("test multi select options sql query");

        var testSelectionItemForMultiSelectionModelDatas = testItems.Fixture
            .CreateMany<QuestionPartResponseFormatOptionSelectMultiValueModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<QuestionPartResponseFormatOptionSelectMultiValueModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test multi select options sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testSelectionItemForMultiSelectionModelDatas);
        #endregion

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.That(result.DataShareRequestQuestion_DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionModelDatas.First().DataShareRequestQuestion_DataShareRequestId));
    }

    [Test]
    public void GivenAnOptionSelectionQuestionWithInvalidFormatType_WhenIGetDataShareRequestQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var invalidFormatType =
            (QuestionPartResponseFormatType)Enum.GetValues<QuestionPartResponseFormatType>().Cast<int>().Max() + 1;

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, invalidFormatType)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetDataShareRequestQuestion from database"));
    }

    [Test]
    public async Task GivenANoResponseQuestion_WhenIGetDataShareRequestQuestionAsync_ThenTheQuestionIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.None)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, QuestionPartResponseFormatType.ReadOnly)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.That(result.DataShareRequestQuestion_DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionModelDatas.First().DataShareRequestQuestion_DataShareRequestId));
    }

    [Test]
    public void GivenANoResponseQuestionWithInvalidFormatType_WhenIGetDataShareRequestQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var invalidFormatType =
            (QuestionPartResponseFormatType) Enum.GetValues<QuestionPartResponseFormatType>().Cast<int>().Max() + 1;

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, QuestionPartResponseInputType.None)
            .With(x => x.QuestionPartResponseTypeInformation_FormatType, invalidFormatType)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
                testDataShareRequestId, testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetDataShareRequestQuestion from database"));
    }

    [Test]
    public void GivenAQuestionWithInvalidInputType_WhenIGetDataShareRequestQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionModelData)
            .Returns("test sql query");

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testResponseInformations = testItems.Fixture
            .Build<QuestionPartAnswerResponseInformationModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerItem_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .With(x => x.QuestionPartAnswer_AnswerPartResponseInformations, testResponseInformations)
            .Create();

        var testMultiAnswerControl = testItems.Fixture
            .Build<QuestionPartMultipleAnswerItemControlModelData>()
            .With(x => x.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed, false)
            .Create();

        var invalidQuestionPartResponseInputType =
            (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testResponseTypeInformation = testItems.Fixture
            .Build<QuestionPartResponseTypeInformationModelData>()
            .With(x => x.QuestionPartResponseTypeInformation_InputType, invalidQuestionPartResponseInputType)
            .Create();

        var testQuestion = testItems.Fixture
            .Build<QuestionPartModelData>()
            .With(x => x.QuestionPart_MultipleAnswerItemControl, testMultiAnswerControl)
            .With(x => x.QuestionPart_ResponseTypeInformation, testResponseTypeInformation)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionId == testQuestionId
                    ? testDataShareRequestQuestionModelDatas
                    : [];
            });

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
                testDataShareRequestId, testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetDataShareRequestQuestion from database"));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIGetDataShareRequestQuestionAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testQuestionId = testItems.Fixture.Create<Guid>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Data
        var testResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testAnswer = testItems.Fixture
            .Build<QuestionPartAnswerModelData>()
            .With(x => x.QuestionPartAnswer_AnswerPartResponses, testResponses)
            .Create();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .With(x => x.DataShareRequestQuestionPart_Answer, testAnswer)
            .CreateMany().ToList();

        var testDataShareRequestQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .CreateMany(1).ToList();

        var questionModelMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                questionModelMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestQuestionModelData>(),
                    testItems.Fixture.Create<QuestionPartModelData>(),
                    testItems.Fixture.Create<QuestionPartPromptsModelData>(),
                    testItems.Fixture.Create<QuestionPartMultipleAnswerItemControlModelData>(),
                    testItems.Fixture.Create<QuestionPartResponseTypeInformationModelData>(),
                    testItems.Fixture.Create<QuestionPartAnswerModelData>(),
                    testItems.Fixture.Create<QuestionPartAnswerResponseInformationModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestQuestionModelDatas);
        #endregion

        #region Set Up Footer Data
        var testDataShareRequestQuestionFooterModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionFooterModelData>()
            .CreateMany().ToList();

        var footerMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionFooterModelData?,
                    DataShareRequestQuestionFooterItemModelData?,
                    DataShareRequestQuestionFooterModelData?>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionFooterModelData?,
                    DataShareRequestQuestionFooterItemModelData?,
                    DataShareRequestQuestionFooterModelData?> mappingFunc,
                string _,
                object? _) =>
            {
                footerMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestQuestionFooterModelData>(),
                    testItems.Fixture.Create<DataShareRequestQuestionFooterItemModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestQuestionFooterModelDatas);
        #endregion

        await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testDataShareRequestId, testQuestionId);

        Assert.Multiple(() =>
        {
            Assert.That(questionModelMappingFunctionHasBeenRun, Is.True);
            Assert.That(footerMappingFunctionHasBeenRun, Is.True);
        });
    }

    [Test]
    public void GivenGettingAQuestionWillFail_WhenIGetDataShareRequestQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionModelData,
                    QuestionPartModelData,
                    QuestionPartPromptsModelData,
                    QuestionPartMultipleAnswerItemControlModelData,
                    QuestionPartResponseTypeInformationModelData,
                    QuestionPartAnswerModelData?,
                    QuestionPartAnswerResponseInformationModelData?,
                    DataShareRequestQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            testItems.Fixture.Create<Guid>(), testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestQuestion from database"));
    }
    #endregion

    #region GetDataShareRequestQuestionStatusInformationsAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestQuestionStatusInformationsAsync_ThenTheQuestionStatusInformationIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestQuestionStatusInformationsModelData)
            .Returns("test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestQuestionStatusInformationSetModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionStatusInformationSetModelData>()
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    DataShareRequestQuestionStatusInformationSetModelData,
                    DataShareRequestQuestionStatusInformationModelData,
                    QuestionSetQuestionInformationModelData,
                    QuestionResponseInformationDataModel,
                    QuestionPartResponseDataModel,
                    QuestionPreRequisiteDataModel?,
                    QuestionSetQuestionApplicabilityOverride?,
                    DataShareRequestQuestionStatusInformationSetModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionStatusInformationSetModelData,
                    DataShareRequestQuestionStatusInformationModelData,
                    QuestionSetQuestionInformationModelData,
                    QuestionResponseInformationDataModel,
                    QuestionPartResponseDataModel,
                    QuestionPreRequisiteDataModel?,
                    QuestionSetQuestionApplicabilityOverride?,
                    DataShareRequestQuestionStatusInformationSetModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return dynamicParameters.DataShareRequestId == testDataShareRequestId
                    ? testDataShareRequestQuestionStatusInformationSetModelDatas
                    : [];
            });

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionStatusInformationsAsync(
            testDataShareRequestId);

        Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionStatusInformationSetModelDatas.First().DataShareRequestId));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIGetDataShareRequestQuestionStatusInformations_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestQuestionStatusInformationSetModelDatas = testItems.Fixture
            .Build<DataShareRequestQuestionStatusInformationSetModelData>()
            .CreateMany(1).ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionStatusInformationSetModelData,
                    DataShareRequestQuestionStatusInformationModelData,
                    QuestionSetQuestionInformationModelData,
                    QuestionResponseInformationDataModel,
                    QuestionPartResponseDataModel,
                    QuestionPreRequisiteDataModel?,
                    QuestionSetQuestionApplicabilityOverride?,
                    DataShareRequestQuestionStatusInformationSetModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestQuestionStatusInformationSetModelData,
                    DataShareRequestQuestionStatusInformationModelData,
                    QuestionSetQuestionInformationModelData,
                    QuestionResponseInformationDataModel,
                    QuestionPartResponseDataModel,
                    QuestionPreRequisiteDataModel?,
                    QuestionSetQuestionApplicabilityOverride?,
                    DataShareRequestQuestionStatusInformationSetModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestQuestionStatusInformationSetModelData>(),
                    testItems.Fixture.Create<DataShareRequestQuestionStatusInformationModelData>(),
                    testItems.Fixture.Create<QuestionSetQuestionInformationModelData>(),
                    testItems.Fixture.Create<QuestionResponseInformationDataModel>(),
                    testItems.Fixture.Create<QuestionPartResponseDataModel>(),
                    testItems.Fixture.Create<QuestionPreRequisiteDataModel>(),
                    testItems.Fixture.Create<QuestionSetQuestionApplicabilityOverride>());
            })
            .ReturnsAsync(() => testDataShareRequestQuestionStatusInformationSetModelDatas);

        await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionStatusInformationsAsync(
            testDataShareRequestId);

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }

    [Test]
    public void GivenGettingQuestionStatusInformationWillFail_WhenIGetDataShareRequestQuestionStatusInformationsAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestQuestionStatusInformationSetModelData,
                    DataShareRequestQuestionStatusInformationModelData,
                    QuestionSetQuestionInformationModelData,
                    QuestionResponseInformationDataModel,
                    QuestionPartResponseDataModel,
                    QuestionPreRequisiteDataModel?,
                    QuestionSetQuestionApplicabilityOverride?,
                    DataShareRequestQuestionStatusInformationSetModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestQuestionStatusInformationsAsync(
            testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestQuestionStatusInformations from database"));
    }
    #endregion

    #region SetDataShareRequestQuestionAnswerAsync() Tests
    [Test]
    public async Task GivenFreeFormQuestionAnswerData_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheAnswerDataIsWritten()
    {
        var testItems = CreateTestItems();

        #region Set Up Responses
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CreateAnswerPartResponse)
            .Returns("test sql command");
        
        var testResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData>()
            .CreateMany().ToList();

        var testResponsesAsBaseClass = new List<DataShareRequestQuestionAnswerPartResponseWriteModelData>();
        testResponsesAsBaseClass.AddRange(testResponses);

        var testAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartWriteModelData>()
            .With(x => x.AnswerPartResponses, testResponsesAsBaseClass)
            .CreateMany().ToList();

        var testDataShareRequestQuestionAnswerWriteModelData = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerWriteModelData>()
            .With(x => x.AnswerParts, testAnswerParts)
            .Create();

        var answerDataHasBeenWritten = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()))
            .Callback(() =>
            {
                answerDataHasBeenWritten = true;
            });
        #endregion

        #region Set Up Existing Responses
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseIds)
            .Returns("test existing response ids sql query");

        var testExistingResponseItems = testItems.Fixture.CreateMany<Guid>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test existing response ids sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testExistingResponseItems);

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseItemId)
            .Returns("test existing answer part response item id sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test existing answer part response item id sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testItems.Fixture.CreateMany<Guid>(1).ToList());
        #endregion

        await testItems.AcquirerDataShareRequestRepository.SetDataShareRequestQuestionAnswerAsync(
            testDataShareRequestQuestionAnswerWriteModelData);

        Assert.That(answerDataHasBeenWritten, Is.True);
    }

    [Test]
    public async Task GivenOptionSelectionResponseTypeInQuestionAnswerData_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheAnswerDataIsWritten()
    {
        var testItems = CreateTestItems();

        #region Set Up Responses
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CreateAnswerPartResponse)
            .Returns("test sql command");

        var testSupplementaryResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData>()
            .CreateMany().ToList();

        var testSupplementaryResponsesAsBaseClass = new List<DataShareRequestQuestionAnswerPartResponseWriteModelData>();
        testSupplementaryResponsesAsBaseClass.AddRange(testSupplementaryResponses);

        var testSupplementaryAnswerPart = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartWriteModelData>()
            .With(x => x.AnswerPartResponses, testSupplementaryResponsesAsBaseClass)
            .Create();

        var testSelectionOptions = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData>()
            .With(x => x.SupplementaryQuestionAnswerPart, testSupplementaryAnswerPart)
            .CreateMany(1).ToList();

        var testResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData>()
            .With(x => x.SelectionOptions, testSelectionOptions)
            .CreateMany(1).ToList();

        var testResponsesAsBaseClass = new List<DataShareRequestQuestionAnswerPartResponseWriteModelData>();
        testResponsesAsBaseClass.AddRange(testResponses);

        var testAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartWriteModelData>()
            .With(x => x.AnswerPartResponses, testResponsesAsBaseClass)
            .CreateMany().ToList();

        var testDataShareRequestQuestionAnswerWriteModelData = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerWriteModelData>()
            .With(x => x.AnswerParts, testAnswerParts)
            .Create();

        var answerDataHasBeenWritten = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()))
            .Callback(() =>
            {
                answerDataHasBeenWritten = true;
            });
        #endregion

        #region Set Up Existing Responses
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseIds)
            .Returns("test existing response ids sql query");

        var testExistingResponseItems = testItems.Fixture.CreateMany<Guid>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test existing response ids sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testExistingResponseItems);

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseItemId)
            .Returns("test existing answer part response item id sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test existing answer part response item id sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testItems.Fixture.CreateMany<Guid>(1).ToList());

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerPartResponseItemSelectionOptionIds)
            .Returns("test existing answer part response item selection option id sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test existing answer part response item selection option id sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testItems.Fixture.CreateMany<Guid>(1).ToList());
        #endregion

        await testItems.AcquirerDataShareRequestRepository.SetDataShareRequestQuestionAnswerAsync(
            testDataShareRequestQuestionAnswerWriteModelData);

        Assert.That(answerDataHasBeenWritten, Is.True);
    }

    [Test]
    public void GivenInvalidResponseTypeInQuestionAnswerData_WhenISetDataShareRequestQuestionAnswerAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CreateAnswerPartResponse)
            .Returns("test sql command");

        var testResponses = new List<TestDataShareRequestQuestionAnswerPartResponseWriteModelData>
        {
            new()
            {
                OrderWithinAnswerPart = 1
            }
        };

        var testResponsesAsBaseClass = new List<DataShareRequestQuestionAnswerPartResponseWriteModelData>();
        testResponsesAsBaseClass.AddRange(testResponses);

        var testAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartWriteModelData>()
            .With(x => x.AnswerPartResponses, testResponsesAsBaseClass)
            .CreateMany().ToList();

        var testDataShareRequestQuestionAnswerWriteModelData = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerWriteModelData>()
            .With(x => x.AnswerParts, testAnswerParts)
            .Create();

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SetDataShareRequestQuestionAnswerAsync(
                testDataShareRequestQuestionAnswerWriteModelData),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SetDataShareRequestQuestionAnswerAsync in database")
                .With.InnerException.With.Message.EqualTo("AnswerPart has unsupported InputType"));
    }

    private class TestDataShareRequestQuestionAnswerPartResponseWriteModelData : DataShareRequestQuestionAnswerPartResponseWriteModelData
    {
        public override QuestionPartResponseInputType InputType => (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;
    }

    [Test]
    public void GivenSettingQuestionAnswerWillFail_WhenISetDataShareRequestQuestionAnswerAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<Guid>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SetDataShareRequestQuestionAnswerAsync(
                testItems.Fixture.Create<DataShareRequestQuestionAnswerWriteModelData>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to SetDataShareRequestQuestionAnswerAsync in database"));
    }
    #endregion

    #region SubmitDataShareRequestAsync() Tests
    [Test]
    public async Task GivenADraftDataShareRequest_WhenISubmitDataShareRequestAsync_ThenTheRequestIsSubmitted()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Submission
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.SubmitDataShareRequest)
            .Returns("test sql command");

        var dataShareRequestHasBeenSubmitted = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                dataShareRequestHasBeenSubmitted = dynamicParameters.DataShareRequestId == testDataShareRequestId;
            });
        #endregion

        await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(
            testAcquirerUserIdSet, testDataShareRequestId);

        Assert.That(dataShareRequestHasBeenSubmitted, Is.True);
    }

    [Test]
    public void GivenADraftDataShareRequestWithExistingSubmissions_WhenISubmitDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Existing Submission
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSubmissionIdsForDataShareRequest)
            .Returns("test sql existing submission query");

        var testExistingSubmissionIds = testItems.Fixture.CreateMany<Guid>(1);

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql existing submission query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testExistingSubmissionIds);
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(testAcquirerUserIdSet, testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SubmitDataShareRequest in database").And
                .With.InnerException.Message.EqualTo("Unable to create Submission for Data Share Request in Draft status as one already exists"));
    }

    [Test]
    public async Task GivenAReturnedDataShareRequest_WhenISubmitDataShareRequestAsync_ThenTheRequestIsSubmitted()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Returned)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Existing Submission
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSubmissionIdsForDataShareRequest)
            .Returns("test sql existing submission query");

        var testExistingSubmissionIds = testItems.Fixture.CreateMany<Guid>(1);

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql existing submission query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testExistingSubmissionIds);
        #endregion

        #region Set Up Submission
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.SubmitDataShareRequest)
            .Returns("test sql command");

        var dataShareRequestHasBeenSubmitted = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                dataShareRequestHasBeenSubmitted = dynamicParameters.DataShareRequestId == testDataShareRequestId;
            });
        #endregion

        await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(
            testAcquirerUserIdSet, testDataShareRequestId);

        Assert.That(dataShareRequestHasBeenSubmitted, Is.True);
    }

    [Test]
    public void GivenAReturnedDataShareRequestWithNoExistingSubmissions_WhenISubmitDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Returned)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Existing Submission
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetSubmissionIdsForDataShareRequest)
            .Returns("test sql existing submission query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<Guid>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql existing submission query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => []);
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(testAcquirerUserIdSet, testDataShareRequestId),
                Throws.TypeOf<DatabaseAccessGeneralException>()
                    .With.Message.EqualTo("Failed to SubmitDataShareRequest in database").And
                    .With.InnerException.Message.EqualTo("No Submission found for Data Share Request in Returned status"));
    }

    [Test]
    public void GivenADataShareRequestWithAnUnExpectedStatus_WhenISubmitDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Accepted)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(testAcquirerUserIdSet, testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SubmitDataShareRequest in database").And
                .With.InnerException.Message.StartsWith("Attempt made to create Submission for Data Share Request with unsupported status"));
    }

    [Test]
    public void GivenSubmittingTheDataShareRequestWillFail_WhenISubmitDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Submission
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.SubmitDataShareRequest)
            .Returns("test sql command");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql command",
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(testAcquirerUserIdSet, testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SubmitDataShareRequest in database").And
                .With.InnerException.With.Message.EqualTo("Failed to SubmitDataShareRequest as failed to set submitted in database"));
    }

    [Test]
    public void GivenRecordingTheStatusChangeWillFail_WhenISubmitDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        testItems.MockAuditLogRepository.Setup(x => x.RecordDataShareRequestStatusChangeAsync(
                It.IsAny<IRecordDataShareRequestStatusChangeParameters>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(testAcquirerUserIdSet, testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SubmitDataShareRequest in database").And
                .With.InnerException.With.Message.EqualTo("Failed to record DataShareRequest Status Change in audit log"));
    }
    #endregion

    #region GetDataShareRequestAnswersSummaryAsync() Tests
    [Test]
    public async Task GivenAFreeFormTextQuestion_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheSummariesForThatRequestAreReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Groups
        var testDataShareRequestAnswersSummaryModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryModelData>(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionGroups)
            .Returns("test question group sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question group sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId
                        ? testDataShareRequestAnswersSummaryModelDatas
                        : [];
            });
        #endregion

        #region Set Up Answer Part
        var testDataShareRequestAnswersSummaryQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswer)
            .Returns("test answer summary sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
                    DataShareRequestAnswersSummaryQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionModelDatas);
        #endregion

        #region Set Up Answer Summary
        var testFreeFormTextAnswerSummaryPart = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType, QuestionPartResponseFormatType.Text)
            .Create();

        var testDataShareRequestAnswersSummaryQuestionPartModelDatas = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
        {
            testFreeFormTextAnswerSummaryPart
        };

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds)
            .Returns("test answer summary answer part sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary answer part sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartModelDatas);
        #endregion
        
        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
            testDataShareRequestId);

        Assert.That(result.DataShareRequestAnswersSummary_DataShareRequestId, Is.EqualTo(testDataShareRequestAnswersSummaryModelDatas.First().DataShareRequestAnswersSummary_DataShareRequestId));
    }

    [Test]
    public async Task GivenAOptionSelectionSingleSelectQuestion_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheAnswersSummariesForThatRequestAreReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Groups
        var testDataShareRequestAnswersSummaryModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryModelData>(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionGroups)
            .Returns("test question group sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question group sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId
                        ? testDataShareRequestAnswersSummaryModelDatas
                        : [];
            });
        #endregion

        #region Set Up Answer Part
        var testDataShareRequestAnswersSummaryQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswer)
            .Returns("test answer summary sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
                    DataShareRequestAnswersSummaryQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionModelDatas);
        #endregion

        #region Set Up Answer Summary
        var testFreeFormTextAnswerSummaryPart = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType, QuestionPartResponseFormatType.SelectSingle)
            .Create();

        var testDataShareRequestAnswersSummaryQuestionPartModelDatas = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
        {
            testFreeFormTextAnswerSummaryPart
        };

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds)
            .Returns("test answer summary answer part sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary answer part sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartModelDatas);
        #endregion

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
            testDataShareRequestId);

        Assert.That(result.DataShareRequestAnswersSummary_DataShareRequestId, Is.EqualTo(testDataShareRequestAnswersSummaryModelDatas.First().DataShareRequestAnswersSummary_DataShareRequestId));
    }

    [Test]
    public void GivenANoResponseQuestion_WhenIGetDataShareRequestAnswersSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Groups
        var testDataShareRequestAnswersSummaryModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryModelData>(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionGroups)
            .Returns("test question group sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question group sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId
                        ? testDataShareRequestAnswersSummaryModelDatas
                        : [];
            });
        #endregion

        #region Set Up Answer Part
        var testDataShareRequestAnswersSummaryQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswer)
            .Returns("test answer summary sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
                    DataShareRequestAnswersSummaryQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionModelDatas);
        #endregion

        #region Set Up Answer Summary
        var testFreeFormTextAnswerSummaryPart = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, QuestionPartResponseInputType.None)
            .Create();

        var testDataShareRequestAnswersSummaryQuestionPartModelDatas = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
        {
            testFreeFormTextAnswerSummaryPart
        };

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds)
            .Returns("test answer summary answer part sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary answer part sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartModelDatas);
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
            testDataShareRequestId),
                Throws.TypeOf<DatabaseAccessGeneralException>()
                    .With.Message.EqualTo("Failed to GetDataShareRequestAnswersSummary from database"));
    }

    [Test]
    public void GivenAQuestionWithUnknownResponseType_WhenIGetDataShareRequestAnswersSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Groups
        var testDataShareRequestAnswersSummaryModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryModelData>(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionGroups)
            .Returns("test question group sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question group sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId
                        ? testDataShareRequestAnswersSummaryModelDatas
                        : [];
            });
        #endregion

        #region Set Up Answer Part
        var testDataShareRequestAnswersSummaryQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswer)
            .Returns("test answer summary sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
                    DataShareRequestAnswersSummaryQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionModelDatas);
        #endregion

        #region Set Up Answer Summary
        var invalidQuestionPartResponseInputType = (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testFreeFormTextAnswerSummaryPart = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, invalidQuestionPartResponseInputType)
            .Create();

        var testDataShareRequestAnswersSummaryQuestionPartModelDatas = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
        {
            testFreeFormTextAnswerSummaryPart
        };

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds)
            .Returns("test answer summary answer part sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary answer part sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartModelDatas);
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
            testDataShareRequestId),
                Throws.TypeOf<DatabaseAccessGeneralException>()
                    .With.Message.EqualTo("Failed to GetDataShareRequestAnswersSummary from database"));
    }

    [Test]
    public async Task GivenMappingFunctionsForAFreeFormQuestion_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Groups
        var testDataShareRequestAnswersSummaryModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryModelData>(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionGroups)
            .Returns("test question group sql query");

        var questionGroupMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question group sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> mappingFunc,
                string _,
                object? _) =>
            {
                questionGroupMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummarySectionModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionGroupModelData>(),
                    testItems.Fixture.Create<Guid>());
            })
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId
                        ? testDataShareRequestAnswersSummaryModelDatas
                        : [];
            });
        #endregion

        #region Set Up Answer Summary
        var testDataShareRequestAnswersSummaryQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswer)
            .Returns("test answer summary sql query");

        var answerSummaryMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
                    DataShareRequestAnswersSummaryQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData?,
                    DataShareRequestAnswersSummaryQuestionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                answerSummaryMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionModelDatas);
        #endregion

        #region Set Up Answer Parts
        var testFreeFormTextAnswerSummaryPart = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType, QuestionPartResponseFormatType.Text)
            .Create();

        var testDataShareRequestAnswersSummaryQuestionPartModelDatas = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
        {
            testFreeFormTextAnswerSummaryPart
        };

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds)
            .Returns("test answer summary answer part sql query");

        var answerSummaryPartMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary answer part sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData> mappingFunc,
                string _,
                object? _) =>
            {
                answerSummaryPartMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionPartModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionPartResponseModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartModelDatas);
        #endregion

        await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(questionGroupMappingFunctionHasBeenRun, Is.True);
            Assert.That(answerSummaryMappingFunctionHasBeenRun, Is.True);
            Assert.That(answerSummaryPartMappingFunctionHasBeenRun, Is.True);
        });
    }

    [Test]
    public async Task GivenMappingFunctionsForAnOptionSelectionQuestion_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Question Groups
        var testDataShareRequestAnswersSummaryModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryModelData>(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionGroups)
            .Returns("test question group sql query");

        var questionGroupMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question group sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> mappingFunc,
                string _,
                object? _) =>
            {
                questionGroupMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummarySectionModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionGroupModelData>(),
                    testItems.Fixture.Create<Guid>());
            })
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testDataShareRequestId
                        ? testDataShareRequestAnswersSummaryModelDatas
                        : [];
            });
        #endregion

        #region Set Up Answer Summary
        var testDataShareRequestAnswersSummaryQuestionModelDatas = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .CreateMany(1).ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswer)
            .Returns("test answer summary sql query");

        var answerSummaryMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
                    DataShareRequestAnswersSummaryQuestionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryQuestionModelData,
                    DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData?,
                    DataShareRequestAnswersSummaryQuestionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                answerSummaryMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionModelDatas);
        #endregion

        #region Set Up Answer Parts
        var testFreeFormTextAnswerSummaryPart = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType, QuestionPartResponseFormatType.SelectSingle)
            .Create();

        var testDataShareRequestAnswersSummaryQuestionPartModelDatas = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
        {
            testFreeFormTextAnswerSummaryPart
        };

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds)
            .Returns("test answer summary answer part sql query");

        var answerSummaryPartMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test answer summary answer part sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryQuestionPartModelData,
                    DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                    DataShareRequestAnswersSummaryQuestionPartModelData> mappingFunc,
                string _,
                object? _) =>
            {
                answerSummaryPartMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionPartModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionPartResponseModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartModelDatas);
        #endregion

        #region Set Up Option Selection Items
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelDatas = testItems.Fixture
            .CreateMany<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData>().ToList();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetAnswerResponseOptionSelectionItems)
            .Returns("test option selection item sql query");

        var optionSelectionItemsFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test option selection item sql query",
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData,
                    DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData?,
                    DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData,
                    DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData?,
                    DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                optionSelectionItemsFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData>(),
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelDatas);
        #endregion

        await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(questionGroupMappingFunctionHasBeenRun, Is.True);
            Assert.That(answerSummaryMappingFunctionHasBeenRun, Is.True);
            Assert.That(answerSummaryPartMappingFunctionHasBeenRun, Is.True);
            Assert.That(optionSelectionItemsFunctionHasBeenRun, Is.True);
        });
    }

    [Test]
    public void GivenGettingAnswersSummaryWillFail_WhenIGetDataShareRequestAnswersSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    DataShareRequestAnswersSummaryModelData,
                    DataShareRequestAnswersSummarySectionModelData,
                    DataShareRequestAnswersSummaryQuestionGroupModelData,
                    Guid?,
                    DataShareRequestAnswersSummaryModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestAnswersSummary from database"));
    }
    #endregion

    #region CancelDataShareRequestAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenICancelDataShareRequestAsync_ThenTheRequestIsCancelled()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testReasonsForCancellation = testItems.Fixture.Create<string>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Submitted)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Cancellation
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CancelDataShareRequest)
            .Returns("test cancellation sql command");

        var requestHasBeenCancelled = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test cancellation sql command",
                It.IsAny<object?>()))
            .Callback(() => requestHasBeenCancelled = true);
        #endregion

        await testItems.AcquirerDataShareRequestRepository.CancelDataShareRequestAsync(
            testAcquirerUserIdSet,
            testDataShareRequestId,
            testReasonsForCancellation);

        Assert.That(requestHasBeenCancelled, Is.True);
    }

    [Test]
    public void GivenCancellingASubmittedDataShareRequestWillFail_WhenICancelDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testReasonsForCancellation = testItems.Fixture.Create<string>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Submitted)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Cancellation
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CancelDataShareRequest)
            .Returns("test cancellation sql command");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test cancellation sql command",
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.CancelDataShareRequestAsync(
                testAcquirerUserIdSet,
                testDataShareRequestId,
                testReasonsForCancellation),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to CancelDataShareRequest in database").And
                .With.InnerException.With.Message.EqualTo("Failed to CancelDataShareRequest as failed to set cancelled in database"));
    }

    [Test]
    public void GivenRecordingTheCancellationWillFail_WhenICancelDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testReasonsForCancellation = testItems.Fixture.Create<string>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Submitted)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        testItems.MockAuditLogRepository.Setup(x => x.RecordDataShareRequestStatusChangeWithCommentsAsync(
                It.IsAny<RecordDataShareRequestStatusChangeWithCommentsParameters>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.CancelDataShareRequestAsync(
                testAcquirerUserIdSet,
                testDataShareRequestId,
                testReasonsForCancellation),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to CancelDataShareRequest in database").And
                .With.InnerException.With.Message.EqualTo("Failed to record DataShareRequest Status Change in audit log"));
    }
    #endregion

    #region DeleteDataShareRequestAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIDeleteDataShareRequestAsync_ThenTheRequestIsDeleted()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Submitted)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Deletion
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.DeleteDataShareRequest)
            .Returns("test deletion sql command");

        var requestHasBeenDeleted = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test deletion sql command",
                It.IsAny<object?>()))
            .Callback(() => requestHasBeenDeleted = true);
        #endregion

        await testItems.AcquirerDataShareRequestRepository.DeleteDataShareRequestAsync(
            testAcquirerUserIdSet,
            testDataShareRequestId);

        Assert.That(requestHasBeenDeleted, Is.True);
    }

    [Test]
    public void GivenDeletingASubmittedDataShareRequestWillFail_WhenIDeleteDataShareRequestAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Request Status
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Submitted)
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);
        #endregion

        #region Set Up Deletion
        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.DeleteDataShareRequest)
            .Returns("test deletion sql command");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test deletion sql command",
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());
        #endregion

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.DeleteDataShareRequestAsync(
                testAcquirerUserIdSet,
                testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to DeleteDataShareRequest in database").And
                .With.InnerException.With.Message.EqualTo("Failed to DeleteDataShareRequest as failed to set deleted in database"));
    }
    #endregion

    #region GetWhetherQuestionsRemainThatRequireAResponseAsync() Tests
    [Theory]
    public async Task GivenQuestionsRemainThatRequireAResponse_WhenIGetWhetherQuestionsRemainThatRequireAResponseAsync_ThenWhetherQuestionsRemainThatRequireAResponseIsReturned(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetWhetherQuestionsRemainThatRequireAResponse)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<bool>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testQuestionsRemainThatRequireAResponse);

        var result = await testItems.AcquirerDataShareRequestRepository.GetWhetherQuestionsRemainThatRequireAResponseAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }

    [Theory]
    public void GivenGettingWhetherResponsesRemainWillFail_WhenIGetWhetherQuestionsRemainThatRequireAResponseAsync_ThenADatabaseAccessGeneralExceptionIsThrown(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<bool>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetWhetherQuestionsRemainThatRequireAResponseAsync(
                testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetWhetherQuestionsRemainThatRequireAResponse from database"));
    }
    #endregion

    #region CheckIfDataShareRequestExistsAsync() Tests
    [Test]
    public async Task GivenDataShareRequestExists_WhenICheckIfDataShareRequestExistsAsync_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CheckIfDataShareRequestExists)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<int>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => 1);

        var result = await testItems.AcquirerDataShareRequestRepository.CheckIfDataShareRequestExistsAsync(
            testDataShareRequestId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task GivenDataShareRequestDoesNotExist_WhenICheckIfDataShareRequestExistsAsync_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.CheckIfDataShareRequestExists)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<int>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => 0);

        var result = await testItems.AcquirerDataShareRequestRepository.CheckIfDataShareRequestExistsAsync(
            testDataShareRequestId);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenCheckingExistenceWillFail_WhenICheckIfDataShareRequestExistsAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<int>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.CheckIfDataShareRequestExistsAsync(
                testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to CheckIfDataShareRequestExistAsync in database"));
    }
    #endregion

    #region GetDataShareRequestNotificationInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequest_WhenIGetDataShareRequestNotificationInformationAsync_ThenTheNotificationInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAcquirerDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestNotificationInformation)
            .Returns("test sql query");

        var testDataShareRequestNotificationInformationModelData =
            testItems.Fixture.Create<DataShareRequestNotificationInformationModelData>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestNotificationInformationModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestNotificationInformationModelData);

        var result = await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testDataShareRequestNotificationInformationModelData));
    }

    [Test]
    public void GivenGettingNotificationInformationWillFail_WhenIGetDataShareRequestNotificationInformationAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestNotificationInformationModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.AcquirerDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestNotificationInformation from database"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AcquirerDataShareRequestRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockAcquirerDataShareRequestSqlQueries = Mock.Get(fixture.Freeze<IAcquirerDataShareRequestSqlQueries>());
        var mockClock = Mock.Get(fixture.Freeze<IClock>());
        var mockAuditLogRepository = Mock.Get(fixture.Freeze<IAuditLogRepository>());
        var mockInputConstraintConfigurationPresenter = Mock.Get(fixture.Freeze<IInputConstraintConfigurationPresenter>());

        var acquirerDataShareRequestRepository = new AcquirerDataShareRequestRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockAcquirerDataShareRequestSqlQueries.Object,
            mockClock.Object,
            mockAuditLogRepository.Object,
            mockInputConstraintConfigurationPresenter.Object);

        return new TestItems(
            fixture,
            acquirerDataShareRequestRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockAcquirerDataShareRequestSqlQueries,
            mockAuditLogRepository);
    }

    private class TestItems(
        IFixture fixture,
        IAcquirerDataShareRequestRepository acquirerDataShareRequestRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IAcquirerDataShareRequestSqlQueries> mockAcquirerDataShareRequestSqlQueries,
        Mock<IAuditLogRepository> mockAuditLogRepository)
    {
        public IFixture Fixture { get; } = fixture;
        public IAcquirerDataShareRequestRepository AcquirerDataShareRequestRepository { get; } = acquirerDataShareRequestRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IAcquirerDataShareRequestSqlQueries> MockAcquirerDataShareRequestSqlQueries { get; } = mockAcquirerDataShareRequestSqlQueries;
        public Mock<IAuditLogRepository> MockAuditLogRepository { get; } = mockAuditLogRepository;
    }
    #endregion
}
