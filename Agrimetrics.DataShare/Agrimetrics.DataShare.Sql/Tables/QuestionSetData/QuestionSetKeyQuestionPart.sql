﻿CREATE TABLE [dbo].[QuestionSetKeyQuestionPart]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[QuestionSet] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionSet](Id),
	[QuestionPartKey]  NVARCHAR(64) NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionPartKeyType](Value),
	[QuestionPart] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[QuestionPart](Id)
)
