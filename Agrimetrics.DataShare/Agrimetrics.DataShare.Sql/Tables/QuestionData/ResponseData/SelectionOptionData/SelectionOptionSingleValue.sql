CREATE TABLE [dbo].[SelectionOptionSingleValue]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

	[OptionType] AS 'SingleSelectOption' PERSISTED,
	FOREIGN KEY (Id) REFERENCES [SelectionOption](Id),

	[IsAlternativeAnswer] BIT NOT NULL
)
