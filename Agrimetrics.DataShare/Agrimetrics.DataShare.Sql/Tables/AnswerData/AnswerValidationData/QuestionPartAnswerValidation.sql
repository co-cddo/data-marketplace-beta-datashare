CREATE TABLE [dbo].QuestionSetQuestionPartAnswerValidation
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[QuestionSet] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [QuestionSet](Id),
	[QuestionPart] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionPart](Id),
	[ValidationRule] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionPartAnswerValidationRule](Id),
	[ErrorText] NVARCHAR(128) NOT NULL,

	CONSTRAINT QuestionSetQuestionPartAnswerValidationUniqueConstraint UNIQUE ([QuestionSet], [QuestionPart], [ValidationRule])
)
