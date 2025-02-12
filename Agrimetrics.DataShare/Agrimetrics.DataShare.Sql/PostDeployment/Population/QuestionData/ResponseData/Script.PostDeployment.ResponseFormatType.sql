
-- ==========================================================================
-- Enter all Question Format values in to the Base ResponseFormatType table first

MERGE [dbo].[ResponseFormatType] AS TGT USING (
	VALUES
	  ('Text', 'FreeForm'),
	  ('Numeric', 'FreeForm'),
	  ('Date', 'FreeForm'),
	  ('Time', 'FreeForm'),
	  ('DateTime', 'FreeForm'),
	  ('SelectSingle', 'OptionSelection'),
	  ('SelectMulti', 'OptionSelection'),
	  ('ReadOnly', 'None'),
	  ('Country', 'FreeForm')

	) AS SRC([Id], [InputType])
	ON TGT.[Id] = SRC.[Id]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [Id],     [InputType])
	VALUES (SRC.[Id], SRC.[InputType]);

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
--   DELETES ARE NOT PERMITTED - THIS IS TYPE REFERENCE DATA

GO


-- ===============================================================================
-- Now add all the Simple format values in to the ResponseFormatFreeForm child table

MERGE [dbo].[ResponseFormatFreeForm] AS TGT USING (
	VALUES
	  ('Text'),
	  ('Numeric'),
	  ('Date'),
	  ('Time'),
	  ('DateTime'),
	  ('Country')
	) AS SRC([Id])
	ON TGT.[Id] = SRC.[Id]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [Id])
	VALUES (SRC.[Id]);

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
--   DELETES ARE NOT PERMITTED - THIS IS TYPE REFERENCE DATA

GO


-- ===============================================================================
-- Now add all the Simple format values in to the ResponseFormatOptionSelection child table

MERGE [dbo].[ResponseFormatOptionSelection] AS TGT USING (
	VALUES
	  ('SelectSingle'),
	  ('SelectMulti')
	) AS SRC([Id])
	ON TGT.[Id] = SRC.[Id]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [Id])
	VALUES (SRC.[Id]);

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
--   DELETES ARE NOT PERMITTED - THIS IS TYPE REFERENCE DATA

GO

-- ===============================================================================
-- Now add all the Simple format values in to the ResponseFormatNone child table

MERGE [dbo].[ResponseFormatNone] AS TGT USING (
	VALUES
	  ('ReadOnly')
	) AS SRC([Id])
	ON TGT.[Id] = SRC.[Id]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [Id])
	VALUES (SRC.[Id]);

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
--   DELETES ARE NOT PERMITTED - THIS IS TYPE REFERENCE DATA

GO
