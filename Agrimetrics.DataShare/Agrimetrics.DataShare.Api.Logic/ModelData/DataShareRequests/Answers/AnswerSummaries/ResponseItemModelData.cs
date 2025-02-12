using System.Diagnostics.CodeAnalysis;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public abstract class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData
{
    [ExcludeFromCodeCoverage] // Abstract, tested on derived classes
    public virtual QuestionPartResponseInputType DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType { get; }

    public Guid DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseItemId { get; set; }
}