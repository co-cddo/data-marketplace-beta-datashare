CREATE TABLE [dbo].[QuestionSetSelectionOptionQuestionHighlight]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[QuestionSet] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionSet](Id),
	[SelectionOption] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[SelectionOption](Id),
	[HighlightCondition] NVARCHAR(64) NOT NULL,
	[ReasonHighlighted] NVARCHAR(512) NOT NULL
)
