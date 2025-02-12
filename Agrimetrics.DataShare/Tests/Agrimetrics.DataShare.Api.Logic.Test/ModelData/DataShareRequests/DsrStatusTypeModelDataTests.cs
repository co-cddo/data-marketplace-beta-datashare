using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestStatusTypeModelDataTests
{
    [Theory]
    public void GivenADataShareRequestStatusTypeModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testDataShareRequestStatusTypeModelData = new DataShareRequestStatusTypeModelData();

        testDataShareRequestStatusTypeModelData.DataShareRequestStatus_RequestStatus = testRequestStatus;

        var result = testDataShareRequestStatusTypeModelData.DataShareRequestStatus_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Theory]
    public void GivenADataShareRequestStatusTypeModelData_WhenISetQuestionsRemainThatRequireAResponse_ThenQuestionsRemainThatRequireAResponseIsSet(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testDataShareRequestStatusTypeModelData = new DataShareRequestStatusTypeModelData();

        testDataShareRequestStatusTypeModelData.DataShareRequestStatus_QuestionsRemainThatRequireAResponse = testQuestionsRemainThatRequireAResponse;

        var result = testDataShareRequestStatusTypeModelData.DataShareRequestStatus_QuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }
}