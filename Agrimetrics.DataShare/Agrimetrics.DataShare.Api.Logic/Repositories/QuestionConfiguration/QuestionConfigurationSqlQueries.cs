using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class QuestionConfigurationSqlQueries : IQuestionConfigurationSqlQueries
{
    string IQuestionConfigurationSqlQueries.GetCompulsoryQuestions =>
      @"SELECT
            [cq].[Question] AS CompulsoryQuestion_QuestionId
        FROM [dbo].[CompulsoryQuestion] [cq]";

    string IQuestionConfigurationSqlQueries.SetCompulsoryQuestion =>
      @"INSERT INTO [dbo].[CompulsoryQuestion] (Question)
        VALUES (@QuestionId)";

    string IQuestionConfigurationSqlQueries.ClearCompulsoryQuestion =>
      @"DELETE FROM [dbo].[CompulsoryQuestion]
        WHERE [Question] = @QuestionId";

    string IQuestionConfigurationSqlQueries.GetCompulsorySupplierMandatedQuestions =>
      @"SELECT
            [csq].[SupplierOrganisation] AS CompulsorySupplierMandatedQuestion_SupplierOrganisationId,
	        [csq].[Question] AS CompulsorySupplierMandatedQuestion_QuestionId        
        FROM [dbo].[CompulsorySupplierQuestion] [csq]
        WHERE [csq].[SupplierOrganisation] = @SupplierOrganisationId";

    string IQuestionConfigurationSqlQueries.SetCompulsorySupplierMandatedQuestion =>
      @"INSERT INTO [dbo].[CompulsorySupplierQuestion] (SupplierOrganisation, Question)
        VALUES (@SupplierOrganisationId, @QuestionId)";

    string IQuestionConfigurationSqlQueries.ClearCompulsorySupplierMandatedQuestion =>
      @"DELETE FROM [dbo].[CompulsorySupplierQuestion]
        WHERE   [SupplierOrganisation] = @SupplierOrganisationId
	        AND [Question] = @QuestionId";
}