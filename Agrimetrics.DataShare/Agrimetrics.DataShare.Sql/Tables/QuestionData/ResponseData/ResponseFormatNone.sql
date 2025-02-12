CREATE TABLE [dbo].[ResponseFormatNone]
(
	[Id] NVARCHAR(32) NOT NULL FOREIGN KEY (Id) REFERENCES [ResponseFormatType](Id) PRIMARY KEY,

	[InputType] AS 'None' PERSISTED
)
