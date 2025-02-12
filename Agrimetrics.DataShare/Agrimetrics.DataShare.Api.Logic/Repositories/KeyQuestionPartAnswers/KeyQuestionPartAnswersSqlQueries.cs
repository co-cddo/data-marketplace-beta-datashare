using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class KeyQuestionPartAnswersSqlQueries : IKeyQuestionPartAnswersSqlQueries
{
    string IKeyQuestionPartAnswersSqlQueries.GetKeyQuestionPartModelData =>
        @"SELECT
	        [qs].[Id] AS KeyQuestionPartAnswer_QuestionSetId,
	        [qp].[Id] AS KeyQuestionPartAnswer_QuestionPartId,
	        [qp].[AllowMultipleAnswerItems] AS KeyQuestionPartAnswer_AllowMultipleResponses,
	        [rft].[InputType] AS KeyQuestionPartAnswer_ResponseFormatType,
	        [ap].[Id] AS KeyQuestionPartAnswer_AnswerPartId,
	        [apr].[Id] AS KeyQuestionPart_AnswerPartResponseId
        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSet] = [qs].[Id]
	        JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	        JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id] AND [qps].[QuestionPartType] = 'MainQuestionPart'
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]
	        JOIN [dbo].[ResponseFormatType] [rft] ON [rft].[Id] = [qp].[ResponseFormatType]
	        JOIN [dbo].[QuestionSetKeyQuestionPart] [qskqp] ON [qskqp].[QuestionSet] = [qs].[Id] AND [qskqp].[QuestionPart] = [qp].[Id]
	        JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
	        JOIN [dbo].[AnswerPart] [ap] ON [ap].[Answer] = [a].[Id] AND [ap].[QuestionPart] = [qp].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId
	        AND [qskqp].[QuestionPartKey] = @QuestionPartKey";

    string IKeyQuestionPartAnswersSqlQueries.GetKeyQuestionPartFreeFormResponseItemModelDatas =>
        @"SELECT
	            [apr].[Id] AS KeyQuestionPartAnswerResponse_ResponseId,
	            [apri].[Id] AS KeyQuestionPartAnswerResponseItem_ResponseItemId,
	            [apriff].[EnteredValue] AS KeyQuestionPartAnswerResponseItemFreeForm_EnteredValue
            FROM [dbo].[AnswerPartResponse] [apr]
	            LEFT JOIN [dbo].[AnswerPartResponseItem] [apri] ON [apri].[AnswerPartResponse] = [apr].[Id]
	            LEFT JOIN [dbo].[AnswerPartResponseItemFreeForm] [apriff] ON [apriff].[Id] = [apri].[Id]
            WHERE [apr].[Id] = @AnswerPartResponseId";

    string IKeyQuestionPartAnswersSqlQueries.GetKeyQuestionPartOptionSelectionResponseItemModelDatas =>
        @"SELECT
	            [apr].[Id] AS KeyQuestionPartAnswerResponse_ResponseId,
	            [apri].[Id] AS KeyQuestionPartAnswerResponseItem_ResponseItemId,
	            [apriso].[Id] AS KeyQuestionPartAnswerResponseItemOptionSelection_ResponseItemId,
	            [so].[OptionValueText] AS KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue
            FROM [dbo].[AnswerPartResponse] [apr]
	            LEFT JOIN [dbo].[AnswerPartResponseItem] [apri] ON [apri].[AnswerPartResponse] = [apr].[Id]
	            LEFT JOIN [dbo].[AnswerPartResponseItemSelectionOption] [apriso] ON [apriso].[AnswerPartResponseItem] = [apri].[Id]
	            LEFT JOIN [dbo].[SelectionOption] [so] ON [so].[Id] = [apriso].[SelectionOption]
            WHERE [apr].[Id] = @AnswerPartResponseId";
}