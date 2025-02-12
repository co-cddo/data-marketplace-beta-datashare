namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;

public interface IAnswerHighlightsSqlQueries
{
    string GetQuestionSetSelectionOptionQuestionHighlightModelDatas { get; }

    string GetDataShareRequestSelectedOptionsModelDatas { get; }
}