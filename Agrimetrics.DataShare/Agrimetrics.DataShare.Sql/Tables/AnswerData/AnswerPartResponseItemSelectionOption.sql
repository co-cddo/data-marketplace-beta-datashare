CREATE TABLE [dbo].[AnswerPartResponseItemSelectionOption]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[AnswerPartResponseItem] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [AnswerPartResponseItem](Id),
	[SelectionOption] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [SelectionOption](Id),

	[SupplementaryQuestionPartAnswer] UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES [AnswerPart](Id),

	CONSTRAINT AnswerPartResponseItemSelectionOptionAnswerPartResponseItemSelectionOptionUniqueConstraint UNIQUE (AnswerPartResponseItem, SelectionOption)
)
