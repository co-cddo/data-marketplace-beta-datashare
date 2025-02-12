CREATE TABLE [dbo].[QuestionPartSet]
(
	Question UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [Question](Id),
	QuestionPart UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [QuestionPart](Id),
	
	QuestionPartType NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES [QuestionPartType](Value),
	QuestionPartOrder INTEGER NOT NULL,

	PRIMARY KEY (Question, QuestionPart, QuestionPartOrder)
)
