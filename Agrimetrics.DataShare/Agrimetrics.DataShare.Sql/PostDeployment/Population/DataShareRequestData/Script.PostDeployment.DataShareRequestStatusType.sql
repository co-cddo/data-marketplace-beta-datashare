
MERGE [dbo].[DataShareRequestStatusType] AS TGT USING (
	VALUES
	  ('None'),
	  ('Draft'),
	  ('Submitted'),
	  ('Accepted'),
	  ('Rejected'),
	  ('Returned'),
	  ('Cancelled'),
	  ('InReview'),
	  ('Deleted')
	) AS SRC([Value])
	ON TGT.[Value] = SRC.[Value]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [Value])
	VALUES (SRC.[Value])

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;

GO
