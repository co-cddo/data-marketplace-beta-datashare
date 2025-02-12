CREATE TABLE [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[QuestionSet] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionSet](Id),
	[ControllingSelectionOption] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[SelectionOption](Id),
	[ControlledQuestionSetQuestion] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionSetQuestion](Id),

	[ControlledQuestionApplicabilityCondition] NVARCHAR(64) NOT NULL FOREIGN KEY REFERENCES [QuestionSetSelectionOptionQuestionApplicabilityCondition](Value)
)
