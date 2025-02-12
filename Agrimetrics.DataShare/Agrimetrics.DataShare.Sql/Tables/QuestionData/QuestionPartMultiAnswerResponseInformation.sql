CREATE TABLE [dbo].[QuestionPartMultiAnswerResponseInformation]
(
	[QuestionPart] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [QuestionPart](Id) PRIMARY KEY,

	[ItemDescription] NVARCHAR(128) NOT NULL,
	[CollectionDescription] NVARCHAR(128) NOT NULL
)
