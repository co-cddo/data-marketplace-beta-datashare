namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

public class DataShareRequestQuestionStatusInformationModelData
{
    public Guid DataShareRequestQuestionStatus_QuestionId { get; set; }

    public QuestionSetQuestionInformationModelData QuestionSetQuestionInformation { get; set; }

    public QuestionResponseInformationDataModel QuestionResponseInformation { get; set; }

    public List<QuestionPreRequisiteDataModel> QuestionPreRequisites { get; set; } = [];

    public List<QuestionSetQuestionApplicabilityOverride> SelectionOptionQuestionSetQuestionApplicabilityOverrides { get; set; } = [];
}