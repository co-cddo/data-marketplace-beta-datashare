using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsAnswerPartModelData
{
    public Guid SubmissionDetailsAnswerPart_Id { get; set; }

    public int SubmissionDetailsAnswerPart_OrderWithinAnswer { get; set; }

    public string SubmissionDetailsAnswerPart_QuestionPartText { get; set; } = string.Empty;

    public QuestionPartResponseInputType SubmissionDetailsAnswerPart_InputType { get; set; }

    public QuestionPartResponseFormatType SubmissionDetailsAnswerPart_FormatType { get; set; }

    public bool SubmissionDetailsAnswerPart_MultipleResponsesAllowed { get; set; }

    public string? SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed { get; set; }

    public List<SubmissionDetailsAnswerPartResponseModelData> SubmissionDetailsAnswerPart_Responses { get; set; } = [];
}