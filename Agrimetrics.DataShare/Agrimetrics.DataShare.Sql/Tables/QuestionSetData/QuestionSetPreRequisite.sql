CREATE TABLE [dbo].[QuestionSetPreRequisite]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[QuestionSet] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [QuestionSet](Id),
	[Question] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [Question](Id),
	[PreRequisiteQuestion] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [Question](Id),

	CONSTRAINT QuestionSetQuestionPreRequisiteQuestionUniqueConstraint UNIQUE (QuestionSet,Question,PreRequisiteQuestion)
)
