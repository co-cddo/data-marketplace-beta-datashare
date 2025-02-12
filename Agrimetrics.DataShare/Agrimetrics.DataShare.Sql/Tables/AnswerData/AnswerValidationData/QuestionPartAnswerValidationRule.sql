CREATE TABLE [dbo].[QuestionPartAnswerValidationRule]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[ValidationRuleId] NVARCHAR(128) NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionPartAnswerValidationRuleId],
	[ResponseFormat] NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES [dbo].[ResponseFormatType],

	[ErrorText] NVARCHAR(128) NOT NULL
)
