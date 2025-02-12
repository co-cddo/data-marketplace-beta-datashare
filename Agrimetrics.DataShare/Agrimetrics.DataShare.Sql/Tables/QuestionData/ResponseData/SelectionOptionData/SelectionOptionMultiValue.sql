CREATE TABLE [dbo].[SelectionOptionMultiValue]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

	[OptionType] AS 'MultiSelectOption' PERSISTED,
	FOREIGN KEY (Id) REFERENCES [SelectionOption](Id),

	IsMaster BIT
)
