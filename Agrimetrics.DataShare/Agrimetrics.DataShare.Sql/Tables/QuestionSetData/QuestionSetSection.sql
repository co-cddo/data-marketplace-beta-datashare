﻿CREATE TABLE [dbo].[QuestionSetSection]
(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	QuestionSet UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [QuestionSet](Id),

	SectionNumber INTEGER NOT NULL,
	SectionHeader NVARCHAR(128) NOT NULL
)
