
PRINT 'Migrating countries question to country type if required'

DECLARE @QuestionSetId AS UNIQUEIDENTIFIER
DECLARE @QuestionId AS UNIQUEIDENTIFIER
DECLARE @QuestionPartId AS UNIQUEIDENTIFIER

SELECT
	@QuestionSetId = [qs].[Id],
	@QuestionId = [q].[Id],
	@QuestionPartId = [qp].[Id]
FROM [dbo].[QuestionSet] [qs]
	JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]
	JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSet] = [qs].[Id] AND [qsq].[QuestionSetSection] = [qss].[Id]
	JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id]
	JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]
WHERE
	[qs].[SupplierDomain] IS NULL AND
	[qs].[SupplierOrganisation] IS NULL AND
	[qs].[Esda] IS NULL AND
	[qss].[SectionNumber] = 3 AND
	[qsq].[QuestionOrder] = 5 AND
	[qps].[QuestionPartOrder] = 1

DECLARE @CurrentResponseFormatType AS NVARCHAR(32)

SELECT
	@CurrentResponseFormatType = [qp].[ResponseFormatType]
FROM [dbo].[QuestionPart] [qp] WHERE [qp].[Id] = @QuestionPartId

IF @CurrentResponseFormatType != 'Text'
BEGIN
	PRINT 'Countries question is not of Text format, no need to migrate'
END
ELSE
BEGIN

	PRINT 'Countries question is of Text format, will be migrated'

	-- PRINT 'Question Set: ' + CAST(@QuestionSetId AS NVARCHAR(100))
	-- PRINT 'Question: ' + CAST(@QuestionId AS NVARCHAR(100))
	-- PRINT 'Question Part: ' + CAST(@QuestionPartId AS NVARCHAR(100))

	SELECT * FROM [dbo].[QuestionPart] WHERE [Id] = @QuestionPartId

	DECLARE @TextEmptyValidationRule AS UNIQUEIDENTIFIER

	SELECT
		@TextEmptyValidationRule = [qpavr].[Id]
	FROM [dbo].[QuestionPartAnswerValidationRule] [qpavr]
	WHERE ValidationRuleId = 'FreeForm_Text_NoValueSupplied'

	-- PRINT 'Text Validation Rule: ' + CAST(@TextEmptyValidationRule AS NVARCHAR(100))

	DECLARE @CountryQuestionPartValidationId AS UNIQUEIDENTIFIER;

	SELECT
		@CountryQuestionPartValidationId = [qsqpav].[Id]
	FROM [dbo].[QuestionSetQuestionPartAnswerValidation] [qsqpav]
	WHERE
		[qsqpav].[QuestionSet] = @QuestionSetId AND
		[qsqpav].[QuestionPart] = @QuestionPartId AND
		[qsqpav].[ValidationRule] = @TextEmptyValidationRule

	-- PRINT 'Country Question Part Validation Rule: ' + CAST(@CountryQuestionPartValidationId AS NVARCHAR(100))

	DECLARE @CountryValidationRuleId AS UNIQUEIDENTIFIER;

	SELECT
		@CountryValidationRuleId = [qpavr].[Id]
	FROM [dbo].[QuestionPartAnswerValidationRule] [qpavr]
	WHERE [qpavr].[ValidationRuleId] = 'FreeForm_Country_NoValueSupplied'

	-- PRINT 'Country Validation Rule: ' + CAST(@CountryValidationRuleId AS NVARCHAR(100))

	-- -------------------------

	UPDATE [dbo].[QuestionSetQuestionPartAnswerValidation]
	SET
		[ValidationRule] = @CountryValidationRuleId,
		[ErrorText] = 'Select a country'
	WHERE [Id] = @CountryQuestionPartValidationId

	-- -------------------------

	UPDATE [dbo].[QuestionPart]
	SET [ResponseFormatType] = 'Country'
	WHERE [Id] = @QuestionPartId

	-- -------------------------
END
