﻿
MERGE [dbo].[QuestionStatusType] AS TGT USING (
	VALUES
	  ('NotSet'),
	  ('NoResponseNeeded'),
	  ('NotStarted'),
	  ('CannotStartYet'),
	  ('Completed'),
	  ('NotApplicable')
	) AS SRC([Value])
	ON TGT.[Value] = SRC.[Value]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [Value])
	VALUES (SRC.[Value]);

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
--   DELETES ARE NOT PERMITTED - THIS IS TYPE REFERENCE DATA

GO
