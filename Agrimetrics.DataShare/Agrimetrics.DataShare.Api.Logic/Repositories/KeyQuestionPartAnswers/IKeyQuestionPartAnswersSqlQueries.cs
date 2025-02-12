namespace Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;

public interface IKeyQuestionPartAnswersSqlQueries
{
    string GetKeyQuestionPartModelData { get; }

    string GetKeyQuestionPartFreeFormResponseItemModelDatas { get; }

    string GetKeyQuestionPartOptionSelectionResponseItemModelDatas { get; }
}