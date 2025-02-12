using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class AnswerValidationSqlQueries : IAnswerValidationSqlQueries
{
    string IAnswerValidationSqlQueries.GetQuestionPartAnswerValidationRuleSet =>
      @"SELECT
	        [qp].[Id] AS QuestionPartAnswerValidationRuleSet_QuestionPartId,
	        [qsq].[IsOptional] AS QuestionPartAnswerValidationRuleSet_AnswerIsOptional,
	        [qp].[ResponseFormatType] AS QuestionPartAnswerValidationRuleSet_ResponseFormatType,

	        [qpavr].[Id] AS QuestionPartAnswerValidationRule_RuleId,
	        [qpavr].[ValidationRuleId] AS QuestionPartAnswerValidationRule_Rule,
	        [qsqpav].[ErrorText] AS QuestionPartAnswerValidationRule_QuestionErrorText,
	        [qpavr].[ErrorText] AS QuestionPartAnswerValidationRule_RuleErrorText

        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]
	        JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSetSection] = [qss].[Id]
	        JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	        JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id]
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]

	        LEFT JOIN [dbo].[QuestionSetQuestionPartAnswerValidation] [qsqpav] ON [qsqpav].[QuestionSet] = [qs].[Id] AND [qsqpav].[QuestionPart] = [qp].[Id]
	        LEFT JOIN [dbo].[QuestionPartAnswerValidationRule] [qpavr] ON [qpavr].[Id] = [qsqpav].[ValidationRule]
        WHERE   [dsr].[Id] = @DataShareRequestId
	        AND [qp].[Id] = @QuestionPartId";
}