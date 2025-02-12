
MERGE [dbo].[QuestionPartAnswerValidationRule] AS TGT USING (
	VALUES
	  ('FreeForm_Text_NoValueSupplied', 'Text', 'No value has been entered'),

	  ('FreeForm_Number_NoValueSupplied', 'Numeric', 'No value has been entered'),
	  ('FreeForm_Number_NotAValidNumber', 'Numeric', 'Value is not a valid number'),

	  ('FreeForm_Date_NoValueSupplied', 'Date', 'No value has been entered'),
	  ('FreeForm_Date_NotAValidDate', 'Date', 'Value is not a valid date'),
	  ('FreeForm_Date_DateCannotBeInThePast', 'Date', 'Date cannot be in the past'),
	  ('FreeForm_Date_DateCannotBeInTheFuture', 'Date', 'Date cannot be in the future'),
	  
	  ('FreeForm_Time_NoValueSupplied', 'Time', 'No value has been entered'),
	  ('FreeForm_Time_NotAValidTime', 'Time', 'Value is not a valid time'),

	  ('FreeForm_DateTime_NoValueSupplied', 'DateTime', 'No value has been entered'),
	  ('FreeForm_DateTime_NotAValidDateTime', 'DateTime', 'Value is not a valid date & time'),
	  ('FreeForm_DateTime_DateTimeCannotBeInThePast', 'DateTime', 'Date & Time cannot be in the past'),
	  ('FreeForm_DateTime_DateTimeCannotBeInTheFuture', 'DateTime', 'Date & Time cannot be in the future'),

	  ('OptionSelection_SelectSingle_NoOptionIsSelected', 'SelectSingle', 'No option has been selected'),

	  ('OptionSelection_SelectMulti_NoOptionIsSelected', 'SelectMulti', 'At least one option must be selected'),

	  ('FreeForm_Country_NoValueSupplied', 'Country', 'No country has been selected')
	) AS SRC([ValidationRuleId], [ResponseFormat], [ErrorText])
	ON TGT.[ValidationRuleId] = SRC.[ValidationRuleId]

-- Inserts
WHEN NOT MATCHED BY TARGET THEN
	INSERT (    [ValidationRuleId],     [ResponseFormat],     [ErrorText])
	VALUES (SRC.[ValidationRuleId], SRC.[ResponseFormat], SRC.[ErrorText]);

-- Updates
--   UPDATES DON'T APPLY - THIS IS TYPE REFERENCE DATA

-- Deletes
--   DELETES ARE NOT PERMITTED - THIS IS TYPE REFERENCE DATA

GO
