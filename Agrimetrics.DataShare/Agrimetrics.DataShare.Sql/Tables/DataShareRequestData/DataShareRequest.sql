CREATE TABLE [dbo].[DataShareRequest]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

	[AcquirerUser] INTEGER NOT NULL,
	[AcquirerDomain] INTEGER NOT NULL,
	[AcquirerOrganisation] INTEGER NOT NULL,
	[Esda] UNIQUEIDENTIFIER NOT NULL,
	[EsdaName] NVARCHAR(128) NOT NULL,
	[SupplierDomain] INTEGER NOT NULL,
	[SupplierOrganisation] INTEGER NOT NULL,
	[QuestionSet] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [QuestionSet](Id),

	[RequestId] NVARCHAR(32) NOT NULL,
	[RequestStatus] NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES [DataShareRequestStatusType](Value) DEFAULT 'Draft',
	[QuestionsRemainThatRequireAResponse] BIT NOT NULL DEFAULT 1
)
