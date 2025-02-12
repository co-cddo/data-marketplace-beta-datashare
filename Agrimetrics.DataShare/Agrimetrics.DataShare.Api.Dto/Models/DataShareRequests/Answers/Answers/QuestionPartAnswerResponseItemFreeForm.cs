using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;

public class QuestionPartAnswerResponseItemFreeForm
    : QuestionPartAnswerResponseItemBase
{
    public override QuestionPartResponseInputType InputType { get; set; } = QuestionPartResponseInputType.FreeForm;

    public string EnteredValue { get; set; } = string.Empty;

    public bool ValueEntryDeclined { get; set; }
}