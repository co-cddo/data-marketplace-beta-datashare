using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

public class DataShareRequestQuestionPartModelData
{
    public QuestionPartModelData DataShareRequestQuestionPart_Question { get; set; }

    public QuestionPartAnswerModelData? DataShareRequestQuestionPart_Answer { get; set; }
}