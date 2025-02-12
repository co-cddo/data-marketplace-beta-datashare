using System.Diagnostics.CodeAnalysis;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;

public abstract class DataShareRequestQuestionAnswerPartResponseWriteModelData
{
    [ExcludeFromCodeCoverage] // Abstract, tested on derived classes
    public virtual QuestionPartResponseInputType InputType { get; }

    public required int OrderWithinAnswerPart { get; init; }
}