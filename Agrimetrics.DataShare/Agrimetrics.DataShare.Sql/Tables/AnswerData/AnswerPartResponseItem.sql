﻿CREATE TABLE [dbo].[AnswerPartResponseItem]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[AnswerPartResponse] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [AnswerPartResponse](Id)
)
