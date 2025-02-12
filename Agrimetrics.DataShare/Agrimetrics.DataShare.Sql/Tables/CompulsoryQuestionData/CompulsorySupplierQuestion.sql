CREATE TABLE [dbo].[CompulsorySupplierQuestion]
(
	[SupplierOrganisation] INTEGER NOT NULL,
	[Question] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [Question](Id),

	PRIMARY KEY ([SupplierOrganisation], [Question])
)
