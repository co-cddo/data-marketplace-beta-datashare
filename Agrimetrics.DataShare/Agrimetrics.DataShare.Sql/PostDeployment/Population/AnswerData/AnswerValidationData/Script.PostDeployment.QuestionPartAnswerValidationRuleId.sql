
MERGE [dbo].[QuestionPartAnswerValidationRuleId] AS TGT USING (
	VALUES
	  ('FreeForm_Text_NoValueSupplied'),

	  ('FreeForm_Number_NoValueSupplied'),
	  ('FreeForm_Number_NotAValidNumber'),

	  ('FreeForm_Date_NoValueSupplied'),
	  ('FreeForm_Date_NotAValidDate'),
	  ('FreeForm_Date_DateCannotBeInThePast'),
	  ('FreeForm_Date_DateCannotBeInTheFuture'),
	  
	  ('FreeForm_Time_NoValueSupplied'),
	  ('FreeForm_Time_NotAValidTime'),

	  ('FreeForm_DateTime_NoValueSupplied'),
	  ('FreeForm_DateTime_NotAValidDateTime'),
	  ('FreeForm_DateTime_DateTimeCannotBeInThePast'),
	  ('FreeForm_DateTime_DateTimeCannotBeInTheFuture'),

	  ('OptionSelection_SelectSingle_NoOptionIsSelected'),

	  ('OptionSelection_SelectMulti_NoOptionIsSelected'),

	  ('FreeForm_Country_NoValueSupplied')

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
