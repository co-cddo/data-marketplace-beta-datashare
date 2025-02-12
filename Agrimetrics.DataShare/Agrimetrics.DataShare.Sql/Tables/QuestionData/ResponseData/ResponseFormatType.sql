CREATE TABLE [dbo].[ResponseFormatType]
(
	[Id] NVARCHAR(32) NOT NULL PRIMARY KEY,

	[InputType] NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES [dbo].[ResponseInputType](Value)
)
