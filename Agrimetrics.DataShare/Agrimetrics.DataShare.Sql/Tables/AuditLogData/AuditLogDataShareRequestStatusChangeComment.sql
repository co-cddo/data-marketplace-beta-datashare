CREATE TABLE [dbo].[AuditLogDataShareRequestStatusChangeComment]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[StatusChange] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[AuditLogDataShareRequestStatusChange](Id),
	[Comment] NVARCHAR(MAX) NOT NULL,
	[CommentOrder] INTEGER NOT NULL
)
