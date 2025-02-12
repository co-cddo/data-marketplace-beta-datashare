CREATE TABLE [dbo].[AuditLogDataShareRequestStatusChange]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[DataShareRequest] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[DataShareRequest](Id),

	[FromStatus] NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES [dbo].[DataShareRequestStatusType](Value),
	[ToStatus] NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES [dbo].[DataShareRequestStatusType](Value),

	[ChangedByUser] INTEGER NOT NULL,
	[ChangedByUserDomain] INTEGER NOT NULL,
	[ChangedByUserOrganisation] INTEGER NOT NULL,

	[ChangedAtUtc] DATETIME2 NOT NULL
)
