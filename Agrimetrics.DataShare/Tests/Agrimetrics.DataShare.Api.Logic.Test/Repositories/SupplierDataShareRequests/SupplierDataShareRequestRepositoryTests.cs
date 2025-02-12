using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Data;
using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.SupplierDataShareRequests;

[TestFixture]
public class SupplierDataShareRequestRepositoryTests
{
    #region GetPendingSubmissionSummariesAsync() Tests
    [Test]
    public async Task GivenASupplierOrganisationId_WhenIGetPendingSubmissionSummariesAsync_ThenThePendingSubmissionSummariesForThatOrganisationAreReturned()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();

        var testPendingSubmissionSummaryModelDatas = testItems.Fixture
            .CreateMany<PendingSubmissionSummaryModelData>().ToList();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetPendingSubmissionSummaries)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<PendingSubmissionSummaryModelData>(
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

                return dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId
                    ? testPendingSubmissionSummaryModelDatas
                    : [];
            });

        var result = (await testItems.SupplierDataShareRequestRepository.GetPendingSubmissionSummariesAsync(
            testSupplierOrganisationId)).ToList();

        Assert.That(result, Is.EqualTo(testPendingSubmissionSummaryModelDatas));
    }

    [Test]
    public void GivenGettingPendingSubmissionSummariesWillFail_WhenIGetPendingSubmissionSummariesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<PendingSubmissionSummaryModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetPendingSubmissionSummariesAsync(
                testItems.Fixture.Create<int>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetPendingSubmissionSummaries"));
    }
    #endregion

    #region GetCompletedSubmissionSummariesAsync() Tests
    [Test]
    public async Task GivenASupplierOrganisationId_WhenIGetCompletedSubmissionSummariesAsync_ThenTheCompletedSubmissionSummariesForThatOrganisationAreReturned()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();

        var testCompletedSubmissionSummaryModelDatas = testItems.Fixture
            .CreateMany<CompletedSubmissionSummaryModelData>().ToList();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetCompletedSubmissionSummaries)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompletedSubmissionSummaryModelData>(
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

                return dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId
                    ? testCompletedSubmissionSummaryModelDatas
                    : [];
            });

        var result = (await testItems.SupplierDataShareRequestRepository.GetCompletedSubmissionSummariesAsync(
            testSupplierOrganisationId)).ToList();

        Assert.That(result, Is.EqualTo(testCompletedSubmissionSummaryModelDatas));
    }

    [Test]
    public void GivenGettingCompletedSubmissionSummariesWillFail_WhenIGetCompletedSubmissionSummariesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompletedSubmissionSummaryModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetCompletedSubmissionSummariesAsync(
                testItems.Fixture.Create<int>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetCompletedSubmissionSummaries"));
    }
    #endregion

    #region GetSubmissionInformationModelDataAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetSubmissionInformationModelDataAsync_ThenTheSubmissionInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testSubmissionInformationModelData = testItems.Fixture.Create<SubmissionInformationModelData>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetSubmissionInformationModelData)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<SubmissionInformationModelData>(
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
                    ? testSubmissionInformationModelData
                    : new SubmissionInformationModelData();
            });

        var result = await testItems.SupplierDataShareRequestRepository.GetSubmissionInformationModelDataAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testSubmissionInformationModelData));
    }

    [Test]
    public void GivenGettingSubmissionInformationWillFail_WhenIGetSubmissionInformationModelDataAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<SubmissionInformationModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetSubmissionInformationModelDataAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetSubmissionDetailsModelDataAsync"));
    }
    #endregion

    #region GetSubmissionDetailsModelDataAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetSubmissionDetailsModelDataAsync_ThenTheSubmissionDetailsForThatRequestAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetSubmissionDetailsModelDatas)
            .Returns("test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Answer Parts
        var testSubmissionDetailsAnswerPartResponseModelDatas = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartResponseModelData>()
            .CreateMany().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsAnswerPartResponseModelData?,
                    SubmissionDetailsAnswerPartResponseItemModelData?,
                    SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
                    SubmissionDetailsAnswerPartResponseModelData?>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testSubmissionDetailsAnswerPartResponseModelDatas);
        #endregion

        #region Set Up Submission Details
        var testSubmissionDetailsModelData = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => new List<SubmissionDetailsModelData> {testSubmissionDetailsModelData});
        #endregion

        var result = await testItems.SupplierDataShareRequestRepository.GetSubmissionDetailsModelDataAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testSubmissionDetailsModelData));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIGetSubmissionDetailsModelDataAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testSubmissionDetailsModelData = testItems.Fixture.Create<SubmissionDetailsModelData>();

        var submissionDetailsMappingFunctionHasBeenRun = false;
        var submissionAnswersMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData> mappingFunc,
                string _,
                object? _) =>
            {
                submissionDetailsMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<SubmissionDetailsModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsSectionModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsMainQuestionModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsAnswerPartModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsBackingQuestionModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsAnswerPartModelData>());
            })
            .ReturnsAsync(() => new List<SubmissionDetailsModelData> { testSubmissionDetailsModelData });

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsAnswerPartResponseModelData?,
                    SubmissionDetailsAnswerPartResponseItemModelData?,
                    SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
                    SubmissionDetailsAnswerPartResponseModelData?>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    SubmissionDetailsAnswerPartResponseModelData?,
                    SubmissionDetailsAnswerPartResponseItemModelData?,
                    SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
                    SubmissionDetailsAnswerPartResponseModelData?> mappingFunc,
                string _,
                object? _) =>
            {
                var isFirstRun = submissionAnswersMappingFunctionHasBeenRun == false;

                submissionAnswersMappingFunctionHasBeenRun = true;

                if (isFirstRun)
                {
                    mappingFunc(
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemFreeFormModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData>());
                }
                else
                {
                    mappingFunc(null, null, null, null, null);
                }
            });

        await testItems.SupplierDataShareRequestRepository.GetSubmissionDetailsModelDataAsync(
            testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(submissionDetailsMappingFunctionHasBeenRun, Is.True);
            Assert.That(submissionAnswersMappingFunctionHasBeenRun, Is.True);
        });
    }

    [Test]
    public void GivenGettingSubmissionDetailsWillFail_WhenIGetSubmissionDetailsModelDataAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetSubmissionDetailsModelDataAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetSubmissionDetailsModelDataAsync"));
    }
    #endregion

    #region GetSubmissionReviewInformationModelDataAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetSubmissionDetailsModelDataAsync_ThenTheSubmissionReviewInformationForThatRequestAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetSubmissionDetailsModelDatas)
            .Returns("test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        #region Set Up Answer Parts
        var testSubmissionDetailsAnswerPartResponseModelDatas = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartResponseModelData>()
            .CreateMany().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsAnswerPartResponseModelData?,
                    SubmissionDetailsAnswerPartResponseItemModelData?,
                    SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
                    SubmissionDetailsAnswerPartResponseModelData?>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testSubmissionDetailsAnswerPartResponseModelDatas);
        #endregion

        #region Set Up Submission Details
        var testSubmissionDetailsModelData = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .Create();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .ReturnsAsync(() => new List<SubmissionDetailsModelData> { testSubmissionDetailsModelData });
        #endregion

        var result = await testItems.SupplierDataShareRequestRepository.GetSubmissionReviewInformationModelDataAsync(
            testDataShareRequestId);

        Assert.That(result.SubmissionReviewInformation_SubmissionDetails, Is.EqualTo(testSubmissionDetailsModelData));
    }

    [Test]
    public async Task GivenMappingFunctions_WhenIGetSubmissionReviewInformationModelDataAsync_ThenTheMappingFunctionsAreRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testSubmissionDetailsModelData = testItems.Fixture.Create<SubmissionDetailsModelData>();

        var submissionDetailsMappingFunctionHasBeenRun = false;
        var submissionAnswersMappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData> mappingFunc,
                string _,
                object? _) =>
            {
                submissionDetailsMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<SubmissionDetailsModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsSectionModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsMainQuestionModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsAnswerPartModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsBackingQuestionModelData>(),
                    testItems.Fixture.Create<SubmissionDetailsAnswerPartModelData>());
            })
            .ReturnsAsync(() => new List<SubmissionDetailsModelData> { testSubmissionDetailsModelData });

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsAnswerPartResponseModelData?,
                    SubmissionDetailsAnswerPartResponseItemModelData?,
                    SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
                    SubmissionDetailsAnswerPartResponseModelData?>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<
                    SubmissionDetailsAnswerPartResponseModelData?,
                    SubmissionDetailsAnswerPartResponseItemModelData?,
                    SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
                    SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
                    SubmissionDetailsAnswerPartResponseModelData?> mappingFunc,
                string _,
                object? _) =>
            {
                var isFirstRun = submissionAnswersMappingFunctionHasBeenRun == false;

                submissionAnswersMappingFunctionHasBeenRun = true;

                if (isFirstRun)
                {
                    mappingFunc(
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemFreeFormModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData>(),
                        testItems.Fixture.Create<SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData>());
                }
                else
                {
                    mappingFunc(null, null, null, null, null);
                }
            });

        await testItems.SupplierDataShareRequestRepository.GetSubmissionReviewInformationModelDataAsync(
            testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(submissionDetailsMappingFunctionHasBeenRun, Is.True);
            Assert.That(submissionAnswersMappingFunctionHasBeenRun, Is.True);
        });
    }

    [Test]
    public void GivenGettingSubmissionReviewInformationWillFail_WhenIGetSubmissionReviewInformationModelDataAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<
                    SubmissionDetailsModelData,
                    SubmissionDetailsSectionModelData,
                    SubmissionDetailsMainQuestionModelData,
                    SubmissionDetailsAnswerPartModelData,
                    SubmissionDetailsBackingQuestionModelData?,
                    SubmissionDetailsAnswerPartModelData?,
                    SubmissionDetailsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetSubmissionReviewInformationModelDataAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetSubmissionDetailsModelDataAsync"));
    }
    #endregion

    #region GetReturnedSubmissionInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetReturnedSubmissionInformationAsync_ThenTheReturnedSubmissionInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testReturnedSubmissionInformationModelData = testItems.Fixture
            .Build<ReturnedSubmissionInformationModelData>()
            .With(x => x.ReturnedSubmission_RequestStatus, DataShareRequestStatusType.Returned)
            .Create();

        #region Set Up Status Query Information
        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Returned)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
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
        #endregion

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetReturnedSubmissionInformation)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<ReturnedSubmissionInformationModelData>(
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
                    ? testReturnedSubmissionInformationModelData
                    : new ReturnedSubmissionInformationModelData();
            });

        var result = await testItems.SupplierDataShareRequestRepository.GetReturnedSubmissionInformationAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testReturnedSubmissionInformationModelData));
    }

    [Test]
    public void GivenGettingReturnedSubmissionInformationWillFail_WhenIGetReturnedSubmissionInformationAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<ReturnedSubmissionInformationModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetReturnedSubmissionInformationAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetReturnedSubmissionInformation"));
    }
    #endregion

    #region GetCompletedSubmissionInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetCompletedSubmissionInformationAsync_ThenTheCompletedSubmissionInformationForThatRequestIsCompleted()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testCompletedSubmissionInformationModelData = testItems.Fixture
            .Build<CompletedSubmissionInformationModelData>()
            .With(x => x.CompletedSubmission_DataShareRequestStatus, DataShareRequestStatusType.Accepted)
            .Create();

        #region Set Up Status Query Information
        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Accepted)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
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
        #endregion

        #region Set Up Completed Submission
        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetCompletedSubmissionInformation)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<CompletedSubmissionInformationModelData>(
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
                    ? testCompletedSubmissionInformationModelData
                    : new CompletedSubmissionInformationModelData();
            });
        #endregion

        var result = await testItems.SupplierDataShareRequestRepository.GetCompletedSubmissionInformationAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testCompletedSubmissionInformationModelData));
    }

    [Test]
    public void GivenGettingCompletedSubmissionInformationWillFail_WhenIGetCompletedSubmissionInformationAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<CompletedSubmissionInformationModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetCompletedSubmissionInformationAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetCompletedSubmissionInformation"));
    }
    #endregion

    #region SetSubmissionNotesAsync() Tests
    [Test]
    public async Task GivenSubmissionNotes_WhenISetSubmissionNotesAsync_ThenSubmissionNotesAreSet()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testNotes = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.SetSubmissionNotes)
            .Returns("test sql command");

        var submissionNotesHaveBeenSet = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
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

                submissionNotesHaveBeenSet =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.Notes == testNotes;
            });

        await testItems.SupplierDataShareRequestRepository.SetSubmissionNotesAsync(
            testDataShareRequestId, testNotes);

        Assert.That(submissionNotesHaveBeenSet, Is.True);
    }

    [Test]
    public void GivenSettingsSubmissionNotesWillFail_WhenISetSubmissionNotesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.SetSubmissionNotesAsync(
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to SetSubmissionNotesAsync"));
    }
    #endregion

    #region StartSubmissionReviewAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIStartSubmissionReviewAsync_ThenTheSubmissionReviewIsStarted()
    {
        var testItems = CreateTestItems();

        var testSupplierUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.UpdateDataShareRequestStatus)
            .Returns("test sql command");

        var submissionReviewHasBeenStarted = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
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

                submissionReviewHasBeenStarted =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.DataShareRequestStatus == DataShareRequestStatusType.InReview.ToString();
            });

        await testItems.SupplierDataShareRequestRepository.StartSubmissionReviewAsync(
            testSupplierUserIdSet, testDataShareRequestId);

        Assert.That(submissionReviewHasBeenStarted, Is.True);
    }

    [Test]
    public void GivenStartingASubmissionReviewWillFail_WhenIStartSubmissionReviewAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockAuditLogRepository.Setup(x => x.RecordDataShareRequestStatusChangeAsync(
            It.IsAny<RecordDataShareRequestStatusChangeParameters>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.StartSubmissionReviewAsync(
                testItems.Fixture.Create<IUserIdSet>(),
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to StartSubmissionReview"));
    }
    #endregion

    #region AcceptSubmissionAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIAcceptSubmissionAsync_ThenTheSubmissionIsAccepted()
    {
        var testItems = CreateTestItems();

        var testSupplierUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testDecisionFeedback = testItems.Fixture.Create<string>();

        #region Set Up Status Query Information
        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.InReview)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
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
        #endregion

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.UpdateDataShareRequestStatus)
            .Returns("test sql command");

        var submissionHasBeenAccepted = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
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

                submissionHasBeenAccepted =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.DataShareRequestStatus == DataShareRequestStatusType.Accepted.ToString();
            });

        await testItems.SupplierDataShareRequestRepository.AcceptSubmissionAsync(
            testSupplierUserIdSet, testDataShareRequestId, testDecisionFeedback);

        Assert.That(submissionHasBeenAccepted, Is.True);
    }

    [Test]
    public void GivenASubmissionIsNotInTheReviewState_WhenIAcceptSubmissionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testDataShareRequestStatusTypeModelData);

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.AcceptSubmissionAsync(
                testItems.Fixture.Create<IUserIdSet>(),
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to AcceptSubmission"));
    }

    [Test]
    public void GivenAcceptingASubmissionWillFail_WhenIAcceptSubmissionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        #region Set Up Status Query Information
        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.InReview)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

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

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.AcceptSubmissionAsync(
                testItems.Fixture.Create<IUserIdSet>(),
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to AcceptSubmission"));
    }
    #endregion

    #region RejectSubmissionAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIRejectSubmissionAsync_ThenTheSubmissionIsRejected()
    {
        var testItems = CreateTestItems();

        var testSupplierUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testDecisionFeedback = testItems.Fixture.Create<string>();

        #region Set Up Status Query Information
        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.InReview)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
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
        #endregion

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.UpdateDataShareRequestStatus)
            .Returns("test sql command");

        var submissionHasBeenRejected = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
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

                submissionHasBeenRejected =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.DataShareRequestStatus == DataShareRequestStatusType.Rejected.ToString();
            });

        await testItems.SupplierDataShareRequestRepository.RejectSubmissionAsync(
            testSupplierUserIdSet, testDataShareRequestId, testDecisionFeedback);

        Assert.That(submissionHasBeenRejected, Is.True);
    }

    [Test]
    public void GivenRejectingASubmissionWillFail_WhenIRejectSubmissionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.RejectSubmissionAsync(
                testItems.Fixture.Create<IUserIdSet>(),
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to RejectSubmission"));
    }
    #endregion

    #region ReturnSubmissionAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIReturnSubmissionAsync_ThenTheSubmissionIsReturned()
    {
        var testItems = CreateTestItems();

        var testSupplierUserIdSet = testItems.Fixture.Create<UserIdSet>();
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testDecisionFeedback = testItems.Fixture.Create<string>();

        #region Set Up Status Query Information
        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.InReview)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
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
        #endregion

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.UpdateDataShareRequestStatus)
            .Returns("test sql command");

        var submissionHasBeenReturned = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
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

                submissionHasBeenReturned =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.DataShareRequestStatus == DataShareRequestStatusType.Returned.ToString();
            });

        await testItems.SupplierDataShareRequestRepository.ReturnSubmissionAsync(
            testSupplierUserIdSet, testDataShareRequestId, testDecisionFeedback);

        Assert.That(submissionHasBeenReturned, Is.True);
    }

    [Test]
    public void GivenReturningASubmissionWillFail_WhenIReturnSubmissionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.ReturnSubmissionAsync(
                testItems.Fixture.Create<IUserIdSet>(),
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<string>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to ReturnSubmission"));
    }
    #endregion

    #region GetAcceptedDecisionSummaryAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetAcceptedDecisionSummaryAsync_ThenTheAcceptedDecisionSummaryForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testAcceptedDecisionSummaryModelData = testItems.Fixture.Create<AcceptedDecisionSummaryModelData>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetAcceptedDecisionSummary)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<AcceptedDecisionSummaryModelData>(
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
                    ? testAcceptedDecisionSummaryModelData
                    : new AcceptedDecisionSummaryModelData();
            });

        var result = await testItems.SupplierDataShareRequestRepository.GetAcceptedDecisionSummaryAsync(testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testAcceptedDecisionSummaryModelData));
    }

    [Test]
    public void GivenGettingAnAcceptedDecisionSummaryWillFail_WhenIGetAcceptedDecisionSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<AcceptedDecisionSummaryModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetAcceptedDecisionSummaryAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetAcceptedDecisionSummary"));
    }
    #endregion

    #region GetRejectedDecisionSummaryAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetRejectedDecisionSummaryAsync_ThenTheRejectedDecisionSummaryForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testRejectedDecisionSummaryModelData = testItems.Fixture.Create<RejectedDecisionSummaryModelData>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetRejectedDecisionSummary)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<RejectedDecisionSummaryModelData>(
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
                    ? testRejectedDecisionSummaryModelData
                    : new RejectedDecisionSummaryModelData();
            });

        var result = await testItems.SupplierDataShareRequestRepository.GetRejectedDecisionSummaryAsync(testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testRejectedDecisionSummaryModelData));
    }

    [Test]
    public void GivenGettingAnRejectedDecisionSummaryWillFail_WhenIGetRejectedDecisionSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<RejectedDecisionSummaryModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetRejectedDecisionSummaryAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetRejectedDecisionSummary"));
    }
    #endregion

    #region GetReturnedDecisionSummaryAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetReturnedDecisionSummaryAsync_ThenTheReturnedDecisionSummaryForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testReturnedDecisionSummaryModelData = testItems.Fixture.Create<ReturnedDecisionSummaryModelData>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetReturnedDecisionSummary)
            .Returns("test sql query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<ReturnedDecisionSummaryModelData>(
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
                    ? testReturnedDecisionSummaryModelData
                    : new ReturnedDecisionSummaryModelData();
            });

        var result = await testItems.SupplierDataShareRequestRepository.GetReturnedDecisionSummaryAsync(testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testReturnedDecisionSummaryModelData));
    }

    [Test]
    public void GivenGettingAnReturnedDecisionSummaryWillFail_WhenIGetReturnedDecisionSummaryAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<ReturnedDecisionSummaryModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetReturnedDecisionSummaryAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetReturnedDecisionSummary"));
    }
    #endregion

    #region GetDataShareRequestStatusAsync() Tests
    [Theory]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestStatusAsync_ThenTheStatusOfThatRequestIsReturned(
        DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestStatusTypeModelData = testItems.Fixture
            .Build<DataShareRequestStatusTypeModelData>()
            .With(x => x.DataShareRequestStatus_RequestStatus, testDataShareRequestStatus)
            .Create();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestStatus)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
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

        var result = await testItems.SupplierDataShareRequestRepository.GetDataShareRequestStatusAsync(testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }

    [Test]
    public void GivenGettingADataShareRequestStatusWillFail_WhenIGetDataShareRequestStatusAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetDataShareRequestStatusAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestStatus"));
    }
    #endregion

    #region GetDataShareRequestNotificationInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestNotificationInformationAsync_ThenTheNotificationInformationOfThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestNotificationInformationModelData = testItems.Fixture.Create<DataShareRequestNotificationInformationModelData>();

        testItems.MockSupplierDataShareRequestSqlQueries.SetupGet(x => x.GetDataShareRequestNotificationInformation)
            .Returns("test sql status query");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestNotificationInformationModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql status query",
                It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return dynamicParameters.DataShareRequestId == testDataShareRequestId
                    ? testDataShareRequestNotificationInformationModelData
                    : new DataShareRequestNotificationInformationModelData();
            });

        var result = await testItems.SupplierDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testDataShareRequestNotificationInformationModelData));
    }

    [Test]
    public void GivenGettingNotificationInformationWillFail_WhenIGetDataShareRequestStatusAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQuerySingleAsync<DataShareRequestNotificationInformationModelData>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.SupplierDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetDataShareRequestNotificationInformation"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<SupplierDataShareRequestRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockSupplierDataShareRequestSqlQueries = Mock.Get(fixture.Freeze<ISupplierDataShareRequestSqlQueries>());
        var mockAuditLogRepository = Mock.Get(fixture.Freeze<IAuditLogRepository>());
        var mockClock = Mock.Get(fixture.Freeze<IClock>());

        var supplierDataShareRequestRepository = new SupplierDataShareRequestRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockSupplierDataShareRequestSqlQueries.Object,
            mockAuditLogRepository.Object,
            mockClock.Object);

        return new TestItems(
            fixture,
            supplierDataShareRequestRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockSupplierDataShareRequestSqlQueries,
            mockAuditLogRepository);
    }

    private class TestItems(
        IFixture fixture,
        ISupplierDataShareRequestRepository supplierDataShareRequestRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<ISupplierDataShareRequestSqlQueries> mockSupplierDataShareRequestSqlQueries,
        Mock<IAuditLogRepository> mockAuditLogRepository)
    {
        public IFixture Fixture { get; } = fixture;
        public ISupplierDataShareRequestRepository SupplierDataShareRequestRepository { get; } = supplierDataShareRequestRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<ISupplierDataShareRequestSqlQueries> MockSupplierDataShareRequestSqlQueries { get; } = mockSupplierDataShareRequestSqlQueries;
        public Mock<IAuditLogRepository> MockAuditLogRepository { get; } = mockAuditLogRepository;
    }
    #endregion
}
