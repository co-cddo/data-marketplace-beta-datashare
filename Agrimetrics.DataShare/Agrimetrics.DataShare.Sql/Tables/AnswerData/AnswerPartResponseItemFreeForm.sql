CREATE TABLE [dbo].[AnswerPartResponseItemFreeForm]
(
	[Id] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [AnswerPartResponseItem](Id) PRIMARY KEY,

	[EnteredValue] NVARCHAR(MAX) NOT NULL,
	[ValueEntryDeclined] BIT NOT NULL
)
