CREATE TABLE [dbo].[ResponseFormatFreeForm]
(
	[Id] NVARCHAR(32) NOT NULL FOREIGN KEY (Id) REFERENCES [ResponseFormatType](Id) PRIMARY KEY,

	[InputType] AS 'FreeForm' PERSISTED
)
