using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

public class QuestionResponseInformationDataModel
{
    public Guid QuestionResponseInformation_QuestionId { get; set; }

    public Guid QuestionResponseInformation_AnswerId { get; set; }

    public QuestionStatusType QuestionResponseInformation_QuestionStatusType { get; set; }

    public List<QuestionPartResponseDataModel> QuestionResponseInformation_QuestionPartResponses { get; set; } = [];
}