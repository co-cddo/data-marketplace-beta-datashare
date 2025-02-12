CREATE TABLE [dbo].[AnswerSet]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[DataShareRequest] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [DataShareRequest](Id),

	CONSTRAINT AnswerSetDataShareRequestUniqueConstraint UNIQUE (DataShareRequest)

)
