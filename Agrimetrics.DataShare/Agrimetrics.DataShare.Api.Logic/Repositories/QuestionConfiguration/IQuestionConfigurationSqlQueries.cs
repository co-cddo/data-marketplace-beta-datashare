namespace Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;

public interface IQuestionConfigurationSqlQueries
{
    string GetCompulsoryQuestions { get; }

    string SetCompulsoryQuestion { get; }

    string ClearCompulsoryQuestion { get; }

    string GetCompulsorySupplierMandatedQuestions { get; }

    string SetCompulsorySupplierMandatedQuestion { get; }

    string ClearCompulsorySupplierMandatedQuestion { get; }
}