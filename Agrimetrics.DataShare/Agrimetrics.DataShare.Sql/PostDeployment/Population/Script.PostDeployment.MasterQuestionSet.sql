
-- ###############################################################################################

-- Firstly check whether a Master question set exists.
-- If it does then go no further, and assume that what is live is correct

DECLARE @MasterQuestionSetExists AS BIT;

SET @MasterQuestionSetExists =
	CASE
		WHEN EXISTS (
			SELECT
				[qs].[Id]
			FROM [dbo].[QuestionSet] [qs]
			WHERE
				[qs].[Esda] IS NULL AND
				[qs].[SupplierOrganisation] IS NULL)
		THEN 1
		ELSE 0
	END

IF @MasterQuestionSetExists = 1
BEGIN
	PRINT 'Master Question Set Already Exists'
	RETURN
END

PRINT 'Master Question Set Does Not Exist, Creating ...'

-- ########################################################################################################
-- Questions

-- Question 'Data Type'
DECLARE @Question_DataTypes TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes(Id)
VALUES ('Data type')

-- Question 'Data Subjects'
DECLARE @Question_DataSubjects TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataSubjects(Id)
VALUES ('Data subjects')

-- Question 'Project Aims'
DECLARE @Question_ProjectAims TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_ProjectAims(Id)
VALUES ('Project aims')

-- Question 'Data Required'
DECLARE @Question_DataRequired TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataRequired(Id)
VALUES ('Data required')

-- Question 'Benefit To Public'
DECLARE @Question_BenefitToPublic TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic(Id)
VALUES ('Benefit to public')

-- Question 'Other Organisations'
DECLARE @Question_OtherOrganisations TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_OtherOrganisations(Id)
VALUES ('Other organisations accessing data')

-- Question 'Other Organisations - Organisation Selection'
DECLARE @Question_OtherOrganisationsOrganisationSelection TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_OtherOrganisationsOrganisationSelection(Id)
VALUES ('Other organisations accessing data - Organisation Selection')

-- Question 'Impact If Data Not Given'
DECLARE @Question_ImpactIfDataNotGiven TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_ImpactIfDataNotGiven(Id)
VALUES ('Impact if data not given')

-- Question 'Date Required'
DECLARE @Question_DateRequired TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DateRequired(Id)
VALUES ('Date required')

-- Question 'Legal Power'
DECLARE @Question_LegalPower TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_LegalPower(Id)
VALUES ('Legal power')

-- Question 'Legal Advice'
DECLARE @Question_LegalAdvice TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_LegalAdvice(Id)
VALUES ('Legal advice')

-- Question 'Legal Review'
DECLARE @Question_LegalReview TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_LegalReview(Id)
VALUES ('Legal review')

-- Question 'Lawful Basis For Personal Data'
DECLARE @Question_LawfulBasisForPersonalData TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData(Id)
VALUES ('Lawful basis for personal data')

-- Question 'Lawful Basis For Special Category Data'
DECLARE @Question_LawfulBasisForSpecialCategoryData TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData(Id)
VALUES ('Lawful basis for special category data')

-- Question 'Substantial Public Interest For Special Category Data'
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData(Id)
VALUES ('Substantial public interest for special category data')

-- Question 'Data Travel Outside UK'
DECLARE @Question_DataTravelOutsideUk TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUk(Id)
VALUES ('Data travel outside UK')

-- Question 'Data Travel Outside UK - Country Selection'
DECLARE @Question_DataTravelOutsideUkCountrySelection TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUkCountrySelection(Id)
VALUES ('Data travel outside UK - Country Selection')

-- Question 'Role Of Organisation'
DECLARE @Question_RoleOfOrganisation TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation(Id)
VALUES ('Role of organisation')

-- Question 'Data Protection Review'
DECLARE @Question_DataProtectionReview TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataProtectionReview(Id)
VALUES ('Data protection review')

-- Question 'Disposal Of Data'
DECLARE @Question_DisposalOfData TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DisposalOfData(Id)
VALUES ('Disposal of data')

-- Question 'Data Security Review'
DECLARE @Question_DataSecurityReview TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[Question] (Header)
	OUTPUT [inserted].[Id] INTO @Question_DataSecurityReview(Id)
VALUES ('Data security review')

-- ########################################################################################################
-- Question Parts

DECLARE @ResponseFormatType_Text NVARCHAR(32) = 'Text';
DECLARE @ResponseFormatType_Date NVARCHAR(32) = 'Date';
DECLARE @ResponseFormatType_Country NVARCHAR(32) = 'Country';
DECLARE @ResponseFormatType_SelectSingle NVARCHAR(32) = 'SelectSingle';
DECLARE @ResponseFormatType_SelectMulti NVARCHAR(32) = 'SelectMulti';
DECLARE @ResponseFormatType_ReadOnly NVARCHAR(32) = 'ReadOnly';

-- Question 'Data Type' - Part 1
DECLARE @Question_DataTypes_Part1 TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Part1(Id)
VALUES (@ResponseFormatType_SelectMulti,
        'What type of data do you need?',
		NULL,
		0)

-- Question 'Data Subjects' - Part 1
DECLARE @Question_DataSubjects_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataSubjects_Part1(Id)
VALUES (@ResponseFormatType_Text,
		'Who are the data subjects?',
		'<div id="data-subjects-hint" class="govuk-hint">' + 
           'Be as specific as possible.' + 
           '<details class="govuk-details" data-module="govuk-details">' + 
             '<summary class="govuk-details__summary">' + 
               '<span class="govuk-details__summary-text">' + 
                 'Data subject definition and examples' + 
               '</span>' + 
             '</summary>' + 
             '<div class="govuk-details__text">' + 
               '<p class="govuk-body">Data subjects are the individuals whose data you''re requesting as part of this dataset.</p>' + 
			   '<p class="govuk-body">The data subjects you identify should be specific to the purpose of your data request.</p>' + 
               '<p class="govuk-body">For example:</p>' + 
               '<ul class="govuk-list govuk-list--bullet">' + 
                 '<li><p>small business owners aged between 45 and 55</p></li>' + 
                 '<li><p>all people using Universal Credit and have children</p></li>' + 
               '</ul>' + 
             '</div>' + 
           '</details>' + 
         '</div>',
		0)

-- Question 'Project Aims' - Part 1
DECLARE @Question_ProjectAims_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_ProjectAims_Part1(Id)
VALUES (@ResponseFormatType_Text,
		'What are the aims of your project?',
		NULL,
		0)

-- Question 'Project Aims' - Part 2
DECLARE @Question_ProjectAims_Part2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_ProjectAims_Part2(Id)
VALUES (@ResponseFormatType_Text,
		'How will the data you want help achieve this?',
		NULL,
		0)

-- Question 'Data Required' - Part 1
DECLARE @Question_DataRequired_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataRequired_Part1(Id)
VALUES (@ResponseFormatType_Text,
		'What data from [[<<EsdaName>>]] do you need?',
		NULL,
		0)

-- Question 'Benefit To Public' - Part 1
DECLARE @Question_BenefitToPublic_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1(Id)
VALUES (@ResponseFormatType_SelectMulti,
		'What public benefit will your project provide?',
		'<div id="benefits-hint" class="govuk-hint">' +
           'Select all relevant options.' +
         '</div>',
		0)

-- Question 'Benefit To Public' - Part 1, Option 'Something Else' Supplementary Part
DECLARE @Question_BenefitToPublic_Part1_OptionSomethingElse_SupplementaryPart TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_OptionSomethingElse_SupplementaryPart(Id)
VALUES (@ResponseFormatType_Text,
		'How will your project do this?',
		NULL,
		0)

-- Question 'Other Organisations' - Part 1
DECLARE @Question_OtherOrganisations_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_OtherOrganisations_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'Will other organisations need access to this data?',
		'<div id="data-access-hint" class="govuk-hint">' +
           'For example, if this is a joint project with another government department.' +
         '</div>',
		0)

-- Question 'Other Organisations - OrganisationSelection' - Part 1
DECLARE @Question_OtherOrganisationsOrganisationSelection_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_OtherOrganisationsOrganisationSelection_Part1(Id)
VALUES (@ResponseFormatType_Text,
		'Which other organisations will access the data?',
		'<div class="govuk-hint">' +
		   'Add each organisation to the list below.' +
		 '</div>',
		1)

-- Question 'Impact If Data Not Given' - Part 1
DECLARE @Question_ImpactIfDataNotGiven_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_ImpactIfDataNotGiven_Part1(Id)
VALUES (@ResponseFormatType_Text,
		'How will it impact your project if you don''t get this data?',
		NULL,
		0)

-- Question 'Date Required' - Part 1
DECLARE @Question_DateRequired_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DateRequired_Part1(Id)
VALUES (@ResponseFormatType_Date,
		'By when do you need this data?',
		'<p class="govuk-body">' +
           'Providing a date can help prioritise requests. The data supplier will let you know whether they can meet this date.' +
         '</p>' +
		 '<legend class="govuk-fieldset__legend govuk-fieldset__legend--m">' +
		   '<h1 class="govuk-fieldset__heading">' +
		     'Enter date' +
		  '</h1>' +
         '</legend>' +
		 '<div id="date-hint" class="govuk-hint">' +
           'For example, 02 03 2025' +
         '</div>',
		0)

-- Question 'Legal Power' - Part 1
DECLARE @Question_LegalPower_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_LegalPower_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'Are you requesting this data under a legal power?',
		'<div id="legal-power-hint" class="govuk-hint">' +
           'For example, through common law, specific legislation (eg the Digital Economy Act) or royal prerogative.' +
         '</div>',
		0)

-- Question 'Legal Power' - Part 1, Option 'Yes' Supplementary Part
DECLARE @Question_LegalPower_Part1_OptionYes_SupplementaryPart TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_LegalPower_Part1_OptionYes_SupplementaryPart(Id)
VALUES (@ResponseFormatType_Text,
		'Which legal power will you use?',
		NULL,
		0)

-- Question 'Legal Advice' - Part 1
DECLARE @Question_LegalAdvice_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_LegalAdvice_Part1(Id)
VALUES (@ResponseFormatType_ReadOnly,
		'Get legal advice',
		'<p class="govuk-body">' +
		   'Contact a lawyer, or someone with legal expertise, to discuss your legal power for requesting this data.' +
		 '</p>' +
		 '<p class="govuk-body">' +
		   'Once you can confirm your legal power, return to this service and continue.' +
		 '</p>',
		0)

-- Question 'Legal Review' - Part 1
DECLARE @Question_LegalReview_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_LegalReview_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'Have your answers been reviewed by someone with legal knowledge?',
		'<div id="legal-review-hint" class="govuk-hint">' +
           'It''s important to confirm that someone with specialist knowledge has checked your answers. If you''re unable to do this, it may slow down the process of getting this data share request approved.' +
         '</div>',
		0)

-- Question 'Lawful Basis For Personal Data' - Part 1
DECLARE @Question_LawfulBasisForPersonalData_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1(Id)
VALUES (@ResponseFormatType_SelectMulti,
		'What is your lawful basis for requesting personal data under UK GDPR?',
		'<div id="lawful-basis-personal-hint" class="govuk-hint">' +
           '<div>' +
             '<p class="govuk-body">Find out more about having a ' +
               '<a target="_blank" class="govuk-link" href="https://ico.org.uk/for-organisations/uk-gdpr-guidance-and-resources/lawful-basis/a-guide-to-lawful-basis/">' +
			     'lawful basis for processing personal data (opens in new tab).' +
			   '</a>' +
             '</p>' +
			 '<p class="govuk-caption">' +
			   'You may need help from a data protection specialist.' +
			 '</p>' +
           '</div>' +
         '</div>',
		0)

-- Question 'Lawful Basis For Special Category Data' - Part 1
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1(Id)
VALUES (@ResponseFormatType_SelectMulti,
		'What is your lawful basis for requesting special category data under UK GDPR?',
		'<div id="lawful-basis-special-hint" class="govuk-hint">' +
           '<div>' +
             '<p class="govuk-body">Find out more about having a ' +
               '<a target="_blank" class="govuk-link" href="https://ico.org.uk/for-organisations/uk-gdpr-guidance-and-resources/lawful-basis/a-guide-to-lawful-basis/">' +
                 'lawful basis for processing special category data (opens in new tab).' +
			   '</a>' +
             '</p>' +
			 '<p class="govuk-body">' +
			   'You may need help from a data protection specialist.' +
		     '</p>' +
           '</div>' +
         '</div>',
		0)

-- Question 'Substantial Public Interest For Special Category Data' - Part 1
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1(Id)
VALUES (@ResponseFormatType_SelectMulti,
		'What is the substantial public interest for requesting special category data under UK GDPR?',
		'<div id="lawful-basis-special-public-interest-hint" class="govuk-hint">' +
           'You may need help from a data protection specialist.' +
         '</div>',
		0)

-- Question 'Data Travel Outside UK' - Part 1
DECLARE @Question_DataTravelOutsideUk_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUk_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'Will any of this data leave the UK?',
		'<div id="data-travel-hint" class="govuk-hint">' +
		   '<p class="govuk-body">' +
		     'This includes third-party software that may host or transfer the data using international servers.' +
		   '</p>' +
		   '<p class="govuk-body">Find out more about ' +
               '<a target="_blank" class="govuk-link" href="https://ico.org.uk/for-organisations/uk-gdpr-guidance-and-resources/international-transfers/international-transfers-a-guide/">' +
                 'international transfers of personal data' +
			   '</a>.' +
             '</p>' +
         '</div>',
		0);

-- Question 'Data Travel Outside UK - Country Selection' - Part 1
DECLARE @Question_DataTravelOutsideUkCountrySelection_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUkCountrySelection_Part1(Id)
VALUES (@ResponseFormatType_Country,
		'What countries will the data travel through?',
		NULL,
		1)

-- Question 'Role Of Organisation' - Part 1
DECLARE @Question_RoleOfOrganisation_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'What will be the role of your organisation under UK GDPR?',
		NULL,
		0)

-- Question 'Data Protection Review' - Part 1
DECLARE @Question_DataProtectionReview_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataProtectionReview_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'Have your answers been reviewed by someone with data protection knowledge?',
		'<div id="protection-review-hint" class="govuk-hint">' +
           'It''s important to confirm that someone with specialist knowledge has checked your answers. If you''re unable to do this, it may slow down the process of getting this data share request approved.' +
         '</div>',
		0)

-- Question 'Disposal Of Data' - Part 1
DECLARE @Question_DisposalOfData_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DisposalOfData_Part1(Id)
VALUES (@ResponseFormatType_Text,
		'How will you ensure that this data is deleted when your project finishes?',
		'<div id="disposal-of-data-hint" class="govuk-hint">' +
           	'This includes any copies of the data within your organisation.' +
         '</div>',
		0)

-- Question 'Data Security Review' - Part 1
DECLARE @Question_DataSecurityReview_Part1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionPart] (ResponseFormatType, QuestionText, HintText, AllowMultipleAnswerItems)
	OUTPUT [inserted].[Id] INTO @Question_DataSecurityReview_Part1(Id)
VALUES (@ResponseFormatType_SelectSingle,
		'Have your answers been reviewed by someone with knowledge of data governance and security?',
		'<div id="protection-review-hint" class="govuk-hint">' +
           'It''s important to confirm that someone with specialist knowledge has checked your answers. If you''re unable to do this, it may slow down the process of getting this data share request approved.' +
         '</div>',
		0)

-- ########################################################################################################
-- Question Part Sets

DECLARE @QuestionPartType_MainQuestionPart NVARCHAR(32) = 'MainQuestionPart';
DECLARE @QuestionPartType_SupplementaryQuestionPart NVARCHAR(32) = 'SupplementaryQuestionPart';

-- Question 'Data Type' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataTypes [q], @Question_DataTypes_Part1 [qp]

-- Question 'Data Subjects' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataSubjects [q], @Question_DataSubjects_Part1 [qp]

-- Question 'Project Aims' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_ProjectAims [q], @Question_ProjectAims_Part1 [qp]

-- Question 'Project Aims' - Part 2
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 2
FROM @Question_ProjectAims [q], @Question_ProjectAims_Part2 [qp]

-- Question 'Data Required' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataRequired [q], @Question_DataRequired_Part1 [qp]

-- Question 'Benefit To Public' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_BenefitToPublic [q], @Question_BenefitToPublic_Part1 [qp]

-- Question 'Benefit To Public' - Part 1, Option 'Something Else' Supplementary Part
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_SupplementaryQuestionPart, 1
FROM @Question_BenefitToPublic [q], @Question_BenefitToPublic_Part1_OptionSomethingElse_SupplementaryPart [qp]

-- Question 'Other Organisations' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_OtherOrganisations [q], @Question_OtherOrganisations_Part1 [qp]

-- Question 'Other Organisations - OrganisationSelection' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_OtherOrganisationsOrganisationSelection [q], @Question_OtherOrganisationsOrganisationSelection_Part1 [qp]

-- Question 'Impact If Data Not Given' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_ImpactIfDataNotGiven [q], @Question_ImpactIfDataNotGiven_Part1 [qp]

-- Question 'Date Required' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DateRequired [q], @Question_DateRequired_Part1 [qp]

-- Question 'Legal Power' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_LegalPower [q], @Question_LegalPower_Part1 [qp]

-- Question 'Legal Power' - Part 1, Option 'Yes' Supplementary Part
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_SupplementaryQuestionPart, 1
FROM @Question_LegalPower [q], @Question_LegalPower_Part1_OptionYes_SupplementaryPart [qp]

-- Question 'Legal Advice' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_LegalAdvice [q], @Question_LegalAdvice_Part1 [qp]

-- Question 'Legal Review' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_LegalReview [q], @Question_LegalReview_Part1 [qp]

-- Question 'Lawful Basis For Personal Data' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_LawfulBasisForPersonalData [q], @Question_LawfulBasisForPersonalData_Part1 [qp]

-- Question 'Lawful Basis For Special Category Data' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_LawfulBasisForSpecialCategoryData [q], @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]

-- Question 'Substantial Public Interest For Special Category Data' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_SubstantialPublicInterestForSpecialCategoryData [q], @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]

-- Question 'Data Travel Outside UK' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataTravelOutsideUk [q], @Question_DataTravelOutsideUk_Part1 [qp]

-- Question 'Data Travel Outside UK - Country Selection' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataTravelOutsideUkCountrySelection [q], @Question_DataTravelOutsideUkCountrySelection_Part1 [qp]

-- Question 'Role Of Organisation' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_RoleOfOrganisation [q], @Question_RoleOfOrganisation_Part1 [qp]

-- Question 'Data Protection Review' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataProtectionReview [q], @Question_DataProtectionReview_Part1 [qp]

-- Question 'Disposal Of Data' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DisposalOfData [q], @Question_DisposalOfData_Part1 [qp]

-- Question 'Data Security Review' - Part 1
INSERT INTO [dbo].[QuestionPartSet] (Question, QuestionPart, QuestionPartType, QuestionPartOrder)
SELECT [q].[Id], [qp].[Id], @QuestionPartType_MainQuestionPart, 1
FROM @Question_DataSecurityReview [q], @Question_DataSecurityReview_Part1 [qp]

-- ########################################################################################################
-- Question Part Multi Answer Response Information

INSERT INTO [dbo].[QuestionPartMultiAnswerResponseInformation] (QuestionPart, ItemDescription, CollectionDescription)
SELECT [qp].[Id], 'organisation', 'organisations'
FROM @Question_OtherOrganisationsOrganisationSelection_Part1 [qp]

INSERT INTO [dbo].[QuestionPartMultiAnswerResponseInformation] (QuestionPart, ItemDescription, CollectionDescription)
SELECT [qp].[Id], 'country', 'countries'
FROM @Question_DataTravelOutsideUkCountrySelection_Part1 [qp]

-- ########################################################################################################
-- Question Part Response Free Form Options

-- Question 'Data Subjects' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_DataSubjects_Part1 [qp]

-- Question 'Project Aims' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_ProjectAims_Part1 [qp]

-- Question 'Project Aims' - Part 2
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_ProjectAims_Part2 [qp]

-- Question 'Data Required' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_DataRequired_Part1 [qp]

-- Question 'Benefit To Public' - Part 1, Option 'Something Else' Supplementary Part
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_BenefitToPublic_Part1_OptionSomethingElse_SupplementaryPart [qp]

-- Question 'Other Organisations - OrganisationSelection' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_OtherOrganisationsOrganisationSelection_Part1 [qp]

-- Question 'Impact If Data Not Given' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_ImpactIfDataNotGiven_Part1 [qp]

-- Question 'Date Required' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_DateRequired_Part1 [qp]

-- Question 'Legal Power' - Part 1, Option 'Yes' Supplementary Part
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_LegalPower_Part1_OptionYes_SupplementaryPart [qp]

-- Question 'Data Travel Outside UK - Country Selection' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_DataTravelOutsideUkCountrySelection_Part1 [qp]

-- Question 'Data Travel Outside UK - Country Selection' - Part 1
INSERT INTO [dbo].[FreeFormOptions] (QuestionPart, ValueEntryMayBeDeclined)
SELECT [qp].[Id], 0
FROM @Question_DisposalOfData_Part1 [qp]

-- ########################################################################################################
-- Selection Options

DECLARE @OptionType_MultiSelectOption NVARCHAR(32) = 'MultiSelectOption';
DECLARE @OptionType_SingleSelectOption NVARCHAR(32) = 'SingleSelectOption';

-- Question 'Data Type' - Part 1
-- Option 1
DECLARE @Question_DataTypes_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Personal data', 'For example, names or addresses.', NULL, 1
FROM @Question_DataTypes_Part1 [qp]
-- Option 2
DECLARE @Question_DataTypes_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Special category data', 'For example, data about health or religious beliefs.', NULL, 2
FROM @Question_DataTypes_Part1 [qp]
-- Option
DECLARE @Question_DataTypes_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'None of the above', 'For example, anonymised data.', NULL, 3
FROM @Question_DataTypes_Part1 [qp]


-- Question 'Benefit To Public' - Part 1
-- Option 1
DECLARE @Question_BenefitToPublic_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Evidence for public policy decision-making', NULL, NULL, 1
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 2
DECLARE @Question_BenefitToPublic_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Evidence for public service delivery', NULL, NULL, 2
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 3
DECLARE @Question_BenefitToPublic_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Evidence for decisions which are likely to benefit people in the UK', NULL, NULL, 3
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 4
DECLARE @Question_BenefitToPublic_Part1_Option4 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option4(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Evidence for decisions about how to allocate and evaluate funding', NULL, NULL, 4
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 5
DECLARE @Question_BenefitToPublic_Part1_Option5 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option5(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Help understand social or economic trends', NULL, NULL, 5
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 6
DECLARE @Question_BenefitToPublic_Part1_Option6 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option6(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Help understand the needs of the public', NULL, NULL, 6
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 7
DECLARE @Question_BenefitToPublic_Part1_Option7 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option7(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Improve existing statistical information', NULL, NULL, 7
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 8
DECLARE @Question_BenefitToPublic_Part1_Option8 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option8(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Challenge or validate existing research or statistics', NULL, NULL, 8
FROM @Question_BenefitToPublic_Part1 [qp]
-- Option 9
DECLARE @Question_BenefitToPublic_Part1_Option9 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_BenefitToPublic_Part1_Option9(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Something else', NULL, [supp_qp].[Id], 9
FROM @Question_BenefitToPublic_Part1 [qp], @Question_BenefitToPublic_Part1_OptionSomethingElse_SupplementaryPart [supp_qp]


-- Question 'Other Organisations' - Part 1
-- Option 1
DECLARE @Question_OtherOrganisations_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_OtherOrganisations_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'No, only my organisation needs access', NULL, NULL, 1
FROM @Question_OtherOrganisations_Part1 [qp]
-- Option 2
DECLARE @Question_OtherOrganisations_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_OtherOrganisations_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Yes, we are collaborating with other organisations', NULL, NULL, 2
FROM @Question_OtherOrganisations_Part1 [qp]


-- Question 'Legal Power' - Part 1
-- Option 1
DECLARE @Question_LegalPower_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LegalPower_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Yes, we believe we have the legal power to do this', NULL, [supp_qp].[Id], 1
FROM @Question_LegalPower_Part1 [qp], @Question_LegalPower_Part1_OptionYes_SupplementaryPart [supp_qp]
-- Option 2
DECLARE @Question_LegalPower_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LegalPower_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'No, we don''t have the legal power to request this', NULL, NULL, 2
FROM @Question_LegalPower_Part1 [qp]
-- Option 3
DECLARE @Question_LegalPower_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LegalPower_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'We don''t know', NULL, NULL, 3
FROM @Question_LegalPower_Part1 [qp]


-- Question 'Legal Review' - Part 1
-- Option 1
DECLARE @Question_LegalReview_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LegalReview_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Yes', NULL, NULL, 1
FROM @Question_LegalReview_Part1 [qp]
-- Option 2
DECLARE @Question_LegalReview_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LegalReview_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'No', NULL, NULL, 2
FROM @Question_LegalReview_Part1 [qp]


-- Question 'Lawful Basis For Personal Data' - Part 1
-- Option 1
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Public Task', 'The processing is necessary for you to perform a task in the public interest or for your official functions, and the task or function has a clear basis in law.', NULL, 1
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]
-- Option 2
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Legal obligation', 'The processing is necessary for the Controller to comply with the law (not including contractual obligations).', NULL, 2
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]
-- Option 3
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Contract', 'The processing is necessary for a contract the Controller has with the individual, or because they have asked you to take specific steps before entering into a contract.', NULL, 3
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]
-- Option 4
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option4 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option4(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Legitimate interests', 'The processing is necessary for your legitimate interests or the legitimate interests of a third party unless there is a good reason to protect the individual’s personal data which overrides those legitimate interests.', NULL, 4
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]
-- Option 5
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option5 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option5(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Consent', 'The individual has given clear consent for you to process their personal data for a specific purpose.', NULL, 5
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]
-- Option 6
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option6 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option6(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Vital interests', 'The processing is necessary to protect someone’s life.', NULL, 6
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]
-- Option 7
DECLARE @Question_LawfulBasisForPersonalData_Part1_Option7 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForPersonalData_Part1_Option7(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Law enforcement', 'The data processing is under Part 3 DPA18 and either by consent or is necessary for the performance of a task carried out for that purpose by a competent authority.', NULL, 7
FROM @Question_LawfulBasisForPersonalData_Part1 [qp]


-- Question 'Lawful Basis For Special Category Data'
-- Option 1
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Reasons of substantial public interest (with a basis in law)', NULL, NULL, 1
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 2
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Legal claims or judicial acts', NULL, NULL, 2
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 3
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Public health (with a basis in law)', NULL, NULL, 3
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 4
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option4 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option4(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Health or social care (with a basis in law)', NULL, NULL, 4
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 5
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option5 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option5(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Employment, social security and social protection (if authorised by law)', NULL, NULL, 5
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 6
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option6 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option6(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Vital interests', NULL, NULL, 6
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 7
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option7 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option7(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Explicit consent', NULL, NULL, 7
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 8
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option8 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option8(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Made public by the data subject', NULL, NULL, 8
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 9
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option9 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option9(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Archiving, research and statistics (with a basis in law)', NULL, NULL, 9
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]
-- Option 10
DECLARE @Question_LawfulBasisForSpecialCategoryData_Part1_Option10 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_LawfulBasisForSpecialCategoryData_Part1_Option10(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Not-for-profit bodies', NULL, NULL, 10
FROM @Question_LawfulBasisForSpecialCategoryData_Part1 [qp]


-- Question 'Substantial Public Interest For Special Category Data'
-- Option 1
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Statutory and government purposes', NULL, NULL, 1
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 2
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Administration of justice and parliamentary purposes', NULL, NULL, 2
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 3
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Equality of opportunity or treatment', NULL, NULL, 3
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 4
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option4 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option4(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Preventing or detecting unlawful acts', NULL, NULL, 4
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 5
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option5 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option5(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Protecting the public', NULL, NULL, 5
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 6
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option6 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option6(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Regulatory requirements', NULL, NULL, 6
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 7
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option7 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option7(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Journalism, academia, art and literature', NULL, NULL, 7
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 8
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option8 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option8(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Preventing fraud', NULL, NULL, 8
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 9
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option9 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option9(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Suspicion of terrorist financing or money laundering', NULL, NULL, 9
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 10
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option10 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option10(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Support for individuals with a particular disability or medical condition', NULL, NULL, 10
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 11
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option11 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option11(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Counselling', NULL, NULL, 11
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 12
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option12 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option12(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Safeguarding of children and individuals at risk', NULL, NULL, 12
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 13
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option13 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option13(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Safeguarding of economic well-being of certain individuals', NULL, NULL, 13
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 14
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option14 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option14(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Insurance', NULL, NULL, 14
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 15
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option15 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option15(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Occupational pensions', NULL, NULL, 15
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 16
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option16 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option16(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Political parties', NULL, NULL, 16
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 17
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option17 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option17(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Elected representatives responding to requests', NULL, NULL, 17
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 18
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option18 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option18(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Disclosure to elected representatives', NULL, NULL, 18
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 19
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option19 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option19(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Informing elected representatives about prisoners', NULL, NULL, 19
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 20
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option20 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option20(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Publication of legal judgments', NULL, NULL, 20
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 21
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option21 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option21(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Anti-doping in sport', NULL, NULL, 21
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]
-- Option 22
DECLARE @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option22 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option22(Id)
SELECT [qp].[Id], @OptionType_MultiSelectOption, 'Standards of behaviour in sport', NULL, NULL, 22
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp]


-- Question 'Data Travel Outside UK' - Part 1
-- Option 1
DECLARE @Question_DataTravelOutsideUk_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUk_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'No, the data will stay in the UK', NULL, NULL, 1
FROM @Question_DataTravelOutsideUk_Part1 [qp]
-- Option 2
DECLARE @Question_DataTravelOutsideUk_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUk_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Yes, it will leave the UK', NULL, NULL, 2
FROM @Question_DataTravelOutsideUk_Part1 [qp]


-- Question 'Role Of Organisation' - Part 1
-- Option 1
DECLARE @Question_RoleOfOrganisation_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataTravelOutsideUk_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Controller / Independent controller', 'We want to exercise overall control over why and how this data is processed.', NULL, 1
FROM @Question_RoleOfOrganisation_Part1 [qp]
-- Option 2
DECLARE @Question_RoleOfOrganisation_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Joint controller', 'We want to share responsibility with another department.', NULL, 2
FROM @Question_RoleOfOrganisation_Part1 [qp]
-- Option 3
DECLARE @Question_RoleOfOrganisation_Part1_Option3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation_Part1_Option3(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Processor', 'We will follow instructions from another department about using this data.', NULL, 3
FROM @Question_RoleOfOrganisation_Part1 [qp]
-- Option 4
DECLARE @Question_RoleOfOrganisation_Part1_Option4 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation_Part1_Option4(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'I don''t know', NULL, NULL, 4
FROM @Question_RoleOfOrganisation_Part1 [qp]


-- Question 'Data Protection Review'
-- Option 1
DECLARE @Question_DataProtectionReview_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataProtectionReview_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Yes', NULL, NULL, 1
FROM @Question_DataProtectionReview_Part1 [qp]
-- Option 2
DECLARE @Question_DataProtectionReview_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataProtectionReview_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'No', NULL, NULL, 2
FROM @Question_DataProtectionReview_Part1 [qp]


-- Question 'Data Security Review'
-- Option 1
DECLARE @Question_DataSecurityReview_Part1_Option1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataSecurityReview_Part1_Option1(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'Yes', NULL, NULL, 1
FROM @Question_DataSecurityReview_Part1 [qp]
-- Option 2
DECLARE @Question_DataSecurityReview_Part1_Option2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[SelectionOption] (QuestionPart, OptionType, OptionValueText, OptionHintText, SupplementaryQuestionPart, OptionOrder)
	OUTPUT [inserted].[Id] INTO @Question_DataSecurityReview_Part1_Option2(Id)
SELECT [qp].[Id], @OptionType_SingleSelectOption, 'No', NULL, NULL, 2
FROM @Question_DataSecurityReview_Part1 [qp]


-- ########################################################################################################
-- Selection Options - Multi Value

-- Question 'Data Type' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_DataTypes_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_DataTypes_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 1
FROM @Question_DataTypes_Part1_Option3 [qpo]


-- Question 'Benefit To Public' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option3 [qpo]
-- Option 4
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option4 [qpo]
-- Option 5
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option5 [qpo]
-- Option 6
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option6 [qpo]
-- Option 7
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option7 [qpo]
-- Option 8
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option8 [qpo]
-- Option 9
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_BenefitToPublic_Part1_Option9 [qpo]


-- Question 'Lawful Basis For Personal Data' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option3 [qpo]
-- Option 4
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option4 [qpo]
-- Option 5
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option5 [qpo]
-- Option 6
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option6 [qpo]
-- Option 7
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForPersonalData_Part1_Option7 [qpo]


-- Question 'Lawful Basis For Special Category Data'
-- Option 1
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option3 [qpo]
-- Option 4
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option4 [qpo]
-- Option 5
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option5 [qpo]
-- Option 6
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option6 [qpo]
-- Option 7
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option7 [qpo]
-- Option 8
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option8 [qpo]
-- Option 9
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option9 [qpo]
-- Option 10
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_LawfulBasisForSpecialCategoryData_Part1_Option10 [qpo]


-- Question 'Substantial Public Interest For Special Category Data'
-- Option 1
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option3 [qpo]
-- Option 4
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option4 [qpo]
-- Option 5
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option5 [qpo]
-- Option 6
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option6 [qpo]
-- Option 7
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option7 [qpo]
-- Option 8
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option8 [qpo]
-- Option 9
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option9 [qpo]
-- Option 10
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option10 [qpo]
-- Option 11
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option11 [qpo]
-- Option 12
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option12 [qpo]
-- Option 13
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option13 [qpo]
-- Option 14
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option14 [qpo]
-- Option 15
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option15 [qpo]
-- Option 16
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option16 [qpo]
-- Option 17
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option17 [qpo]
-- Option 18
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option18 [qpo]
-- Option 19
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option19 [qpo]
-- Option 20
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option20 [qpo]
-- Option 21
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option21 [qpo]
-- Option 22
INSERT INTO [dbo].[SelectionOptionMultiValue] (Id, IsMaster)
SELECT [qpo].[Id], 0
FROM @Question_SubstantialPublicInterestForSpecialCategoryData_Part1_Option22 [qpo]


-- ########################################################################################################
-- Selection Options - Single Value

-- Question 'Other Organisations' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_OtherOrganisations_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_OtherOrganisations_Part1_Option2 [qpo]


-- Question 'Legal Power' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_LegalPower_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_LegalPower_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_LegalPower_Part1_Option3 [qpo]


-- Question 'Legal Review' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_LegalReview_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_LegalReview_Part1_Option2 [qpo]


-- Question 'Data Travel Outside UK' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_DataTravelOutsideUk_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_DataTravelOutsideUk_Part1_Option2 [qpo]


-- Question 'Role Of Organisation' - Part 1
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_RoleOfOrganisation_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_RoleOfOrganisation_Part1_Option2 [qpo]
-- Option 3
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_RoleOfOrganisation_Part1_Option3 [qpo]
-- Option 4
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 1
FROM @Question_RoleOfOrganisation_Part1_Option4 [qpo]


-- Question 'Data Protection Review'
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_DataProtectionReview_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_DataProtectionReview_Part1_Option2 [qpo]


-- Question 'Data Security Review'
-- Option 1
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_DataSecurityReview_Part1_Option1 [qpo]
-- Option 2
INSERT INTO [dbo].[SelectionOptionSingleValue] (Id, IsAlternativeAnswer)
SELECT [qpo].[Id], 0
FROM @Question_DataSecurityReview_Part1_Option2 [qpo]


-- ########################################################################################################
-- Question Footers

-- Question 'Data Type'
DECLARE @Question_DataTypes_Footer TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionFooter] (Question, Header)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Footer(Id)
SELECT [q].[Id], 'More information'
FROM @Question_DataTypes [q]

-- Question 'Role Of Organisation'
DECLARE @Question_RoleOfOrganisation_Footer TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionFooter] (Question, Header)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation_Footer(Id)
SELECT [q].[Id], NULL
FROM @Question_RoleOfOrganisation [q]

-- ########################################################################################################
-- Question Footer Items

-- Question 'Data Type' Footer
-- Item 1
DECLARE @Question_DataTypes_Footer_Item1 TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionFooterItem] (QuestionFooter, [Text], OrderWithinFooter)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Footer_Item1(Id)
SELECT [f].[Id], '<a href="https://ico.org.uk/for-organisations/uk-gdpr-guidance-and-resources/personal-information-what-is-it/what-is-personal-data/what-is-personal-data/" target="_blank">What is personal data? (opens in new tab)</a>', 1
FROM @Question_DataTypes_Footer [f]
-- Item 2
DECLARE @Question_DataTypes_Footer_Item2 TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionFooterItem] (QuestionFooter, [Text], OrderWithinFooter)
	OUTPUT [inserted].[Id] INTO @Question_DataTypes_Footer_Item2(Id)
SELECT [f].[Id], '<a href="https://ico.org.uk/for-organisations/uk-gdpr-guidance-and-resources/lawful-basis/special-category-data/what-is-special-category-data/" target="_blank">What is special category data? (opens in new tab)</a>', 2
FROM @Question_DataTypes_Footer [f]

-- Question 'Role Of Organisation' Footer
-- Item 1
DECLARE @Question_RoleOfOrganisation_Footer_Item1 TABLE(Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionFooterItem] (QuestionFooter, [Text], OrderWithinFooter)
	OUTPUT [inserted].[Id] INTO @Question_RoleOfOrganisation_Footer_Item1(Id)
SELECT [f].[Id], '<p>' +
                    'Find out more about ' +
                    '<a target="_blank" href="https://ico.org.uk/for-organisations/direct-marketing-and-privacy-and-electronic-communications/guidance-for-the-use-of-personal-data-in-political-campaigning-1/controllers-joint-controllers-and-processors/#:~:text=If%20two%20or%20more%20controllers,same%20data%20for%20different%20purposes">' +
                      'controllers, joint controllers and processors' +
                    '</a>.' +
                  '</p>', 1
FROM @Question_RoleOfOrganisation_Footer [f]

-- ########################################################################################################
-- Question Set

DECLARE @MasterQuestionSet TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionSet] (SupplierDomain, SupplierOrganisation, Esda)
	OUTPUT [inserted].[Id] INTO @MasterQuestionSet(Id)
VALUES (NULL, NULL, NULL)


-- ########################################################################################################
-- Question Set Sections

-- Section 1
DECLARE @Section1 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionSetSection] (QuestionSet, SectionNumber, SectionHeader)
	OUTPUT [inserted].[Id] INTO @Section1(Id)
SELECT [qs].[Id], 1, 'Purpose of the data share'
FROM @MasterQuestionSet [qs]

-- Section 2
DECLARE @Section2 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionSetSection] (QuestionSet, SectionNumber, SectionHeader)
	OUTPUT [inserted].[Id] INTO @Section2(Id)
SELECT [qs].[Id], 2, 'Legal power and gateway'
FROM @MasterQuestionSet [qs]

-- Section 3
DECLARE @Section3 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionSetSection] (QuestionSet, SectionNumber, SectionHeader)
	OUTPUT [inserted].[Id] INTO @Section3(Id)
SELECT [qs].[Id], 3, 'Data protection'
FROM @MasterQuestionSet [qs]

-- Section 4
DECLARE @Section4 TABLE (Id UNIQUEIDENTIFIER);
INSERT INTO [dbo].[QuestionSetSection] (QuestionSet, SectionNumber, SectionHeader)
	OUTPUT [inserted].[Id] INTO @Section4(Id)
SELECT [qs].[Id], 4, 'Data governance, security and technology'
FROM @MasterQuestionSet [qs]


-- ########################################################################################################
-- Question Set Questions

-- Section 1, Question 1 - 'Data Types'
DECLARE @Section1_Question1 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question1(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 1, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_DataTypes [q]

-- Section 1, Question 2 - 'Data Subjects'
DECLARE @Section1_Question2 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question2(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 2, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_DataSubjects [q]

-- Section 1, Question 3 - 'Project Aims'
DECLARE @Section1_Question3 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question3(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 3, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_ProjectAims [q]

-- Section 1, Question 4 - 'Data Required'
DECLARE @Section1_Question4 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question4(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 4, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_DataRequired [q]

-- Section 1, Question 5 - 'Benefit To Public'
DECLARE @Section1_Question5 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question5(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 5, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_BenefitToPublic [q]

-- Section 1, Question 6 - 'Other Organisations'
DECLARE @Section1_Question6 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question6(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 6, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_OtherOrganisations [q]

-- Section 1, Question 7 - 'Other Organisations - Organisation Selection'
DECLARE @Section1_Question7 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question7(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 7, 0, 0, [fq].[Id]
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_OtherOrganisationsOrganisationSelection [q], @Question_OtherOrganisations [fq]

-- Section 1, Question 8 - 'Impact If Data Not Given'
DECLARE @Section1_Question8 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question8(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 8, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_ImpactIfDataNotGiven [q]

-- Section 1, Question 9 - 'Date Required'
DECLARE @Section1_Question9 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question9(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 9, 1, 1, NULL
FROM @MasterQuestionSet [qs], @Section1 [qss], @Question_DateRequired [q]


-- Section 2, Question 1 - 'Legal Power'
DECLARE @Section2_Question1 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section1_Question1(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 1, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section2 [qss], @Question_LegalPower [q]

-- Section 2, Question 2 - 'Legal Avice'
DECLARE @Section2_Question2 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section2_Question2(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 2, 0, 0, NULL
FROM @MasterQuestionSet [qs], @Section2 [qss], @Question_LegalAdvice [q]

-- Section 2, Question 3 - 'Legal Review'
DECLARE @Section2_Question3 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section2_Question3(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 3, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section2 [qss], @Question_LegalReview [q]


-- Section 3, Question 1 - 'Lawful Basis For Personal Data'
DECLARE @Section3_Question1 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question1(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 1, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_LawfulBasisForPersonalData [q]

-- Section 3, Question 2 - 'Lawful Basis For Special Category Data'
DECLARE @Section3_Question2 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question2(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 2, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_LawfulBasisForSpecialCategoryData [q]

-- Section 3, Question 3 - 'Substantial Public Interest For Special Category Data'
DECLARE @Section3_Question3 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question3(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 3, 0, 0, NULL
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_SubstantialPublicInterestForSpecialCategoryData [q]

-- Section 3, Question 4 - 'Data Travel Outside UK'
DECLARE @Section3_Question4 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question4(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 4, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_DataTravelOutsideUk [q]

-- Section 3, Question 5 - 'Data Travel Outside UK - Country Selection'
DECLARE @Section3_Question5 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question5(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 5, 0, 0, [fq].[Id]
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_DataTravelOutsideUkCountrySelection [q], @Question_DataTravelOutsideUk [fq]

-- Section 3, Question 6 - 'Role Of Organisation'
DECLARE @Section3_Question6 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question6(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 6, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_RoleOfOrganisation [q]

-- Section 3, Question 7 - 'Data Protection Review'
DECLARE @Section3_Question7 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section3_Question7(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 7, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section3 [qss], @Question_DataProtectionReview [q]


-- Section 4, Question 1 - 'Disposal Of Data'
DECLARE @Section4_Question1 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section4_Question1(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 1, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section4 [qss], @Question_DisposalOfData [q]

-- Section 4, Question 2 - 'Data Security Review'
DECLARE @Section4_Question2 TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO [dbo].[QuestionSetQuestion] (QuestionSetSection, QuestionSet, Question, QuestionOrder, ShowOnQuestionsPage, IsOptional, FrontingQuestion)
	OUTPUT [inserted].[Id] INTO @Section4_Question2(Id)
SELECT [qss].[Id], [qs].[Id], [q].[Id], 2, 1, 0, NULL
FROM @MasterQuestionSet [qs], @Section4 [qss], @Question_DataSecurityReview [q]


-- ########################################################################################################
-- Question Part Keys

-- Section 1, Question 1 - 'Data Types', Part 1
INSERT INTO [dbo].[QuestionSetKeyQuestionPart] (QuestionSet, QuestionPartKey, QuestionPart)
SELECT [qs].[Id], 'DataTypes', [qp].[Id]
FROM @MasterQuestionSet [qs], @Question_DataTypes_Part1 [qp]

-- Section 1, Question 3 - 'Project Aims', Part 1
INSERT INTO [dbo].[QuestionSetKeyQuestionPart] (QuestionSet, QuestionPartKey, QuestionPart)
SELECT [qs].[Id], 'ProjectAims', [qp].[Id]
FROM @MasterQuestionSet [qs], @Question_ProjectAims_Part1 [qp]

-- Section 1, Question 9 - 'Date Required', Part 1
INSERT INTO [dbo].[QuestionSetKeyQuestionPart] (QuestionSet, QuestionPartKey, QuestionPart)
SELECT [qs].[Id], 'DateRequired', [qp].[Id]
FROM @MasterQuestionSet [qs], @Question_DateRequired_Part1 [qp]


-- ########################################################################################################
-- Question Set Pre-Requisites

-- Section 1, Question 2 - 'Data Subjects' requires the completion of Section 1, Question 1 - 'Data Types'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_DataSubjects [q], @Question_DataTypes [pr]

-- Section 2, Question 3 - 'Legal Review' requires the completion of Section 2, Question 1 - 'Legal Power'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_LegalReview [q], @Question_LegalPower [pr]

-- Section 3, Question 1 - 'Lawful Basis For Personal Data' requires the completion of Section 1, Question 1 - 'Data Types'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_LawfulBasisForPersonalData [q], @Question_DataTypes [pr]

-- Section 3, Question 2 - 'Lawful Basis For Special Category Data' requires the completion of Section 1, Question 1 - 'Data Types'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_LawfulBasisForSpecialCategoryData [q], @Question_DataTypes [pr]

-- Section 3, Question 7 - 'Data Protection Review' requires the completion of Section 3, Question 1 - 'Lawful Basis For Personal Data'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_DataProtectionReview [q], @Question_LawfulBasisForPersonalData [pr]

-- Section 3, Question 7 - 'Data Protection Review' requires the completion of Section 3, Question 2 - 'Lawful Basis For Special Category Data'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_DataProtectionReview [q], @Question_LawfulBasisForSpecialCategoryData [pr]

-- Section 3, Question 7 - 'Data Protection Review' requires the completion of Section 3, Question 4 - 'Data Travel Outside UK'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_DataProtectionReview [q], @Question_DataTravelOutsideUk [pr]

-- Section 3, Question 7 - 'Data Protection Review' requires the completion of Section 3, Question 6 - 'Role Of Organisation'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_DataProtectionReview [q], @Question_RoleOfOrganisation [pr]

-- Section 4, Question 2 - 'Data Security Review' requires the completion of Section 4, Question 1 - 'Disposal Of Data'
INSERT INTO [dbo].[QuestionSetPreRequisite] (QuestionSet, Question, PreRequisiteQuestion)
SELECT [qs].[Id], [q].[Id], [pr].[Id]
FROM @MasterQuestionSet [qs], @Question_DataSecurityReview [q], @Question_DisposalOfData [pr]


-- ########################################################################################################
-- Question Set Selection Option Question Applicability Overrides

DECLARE @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsSelected NVARCHAR(64) = 'QuestionIsNotApplicableIfOptionIsSelected';
DECLARE @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsNotSelected NVARCHAR(64) = 'QuestionIsNotApplicableIfOptionIsNotSelected';

-- Section 1, Question 2 - 'Data Subjects' - is not applicable if Section 1, Question 1 - 'Data Types' - Option 3 - 'None Of The Above' IS selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsSelected
FROM @MasterQuestionSet [qs], @Section1_Question2 [controlled_q], @Question_DataTypes_Part1_Option3 [controlling_so]

-- Section 1, Question 7 - 'Other Organisations - Organisation Selection' - is not applicable if Section 1, Question 6 - 'Other Organisations' - Option 1 - 'No' - IS selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsSelected
FROM @MasterQuestionSet [qs], @Section1_Question7 [controlled_q], @Question_OtherOrganisations_Part1_Option1 [controlling_so]

--Section 2, Question 2 - 'Legal Advice' - is not applicable if Section 2, Question 1 - 'Legal Power' - Option 1 - 'Yes' - IS selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsSelected
FROM @MasterQuestionSet [qs], @Section2_Question2 [controlled_q], @Question_LegalPower_Part1_Option1 [controlling_so]

-- Section 3, Question 1 - 'Lawful Basis For Personal Data' - is not applicable if Section 1, Question 1 - 'Data Types' - Option 1 - 'Personal data' - is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsNotSelected
FROM @MasterQuestionSet [qs], @Section3_Question1 [controlled_q], @Question_DataTypes_Part1_Option1 [controlling_so]

-- Section 3, Question 2 - 'Lawful Basis For Special Category Data' - is not applicable if Section 1, Question 1 - 'Data Types' - Option 1 - 'Special Category data' - is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsNotSelected
FROM @MasterQuestionSet [qs], @Section3_Question2 [controlled_q], @Question_DataTypes_Part1_Option2 [controlling_so]

-- Section 3, Question 3 - 'Substantial Public Interest For Special Category Data' - is not applicable if Section 3, Question 2 - 'Lawful Basis For Special Category Data' - Option 1 - 'Reasons of substantial public interest' - is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsNotSelected
FROM @MasterQuestionSet [qs], @Section3_Question3 [controlled_q], @Question_LawfulBasisForSpecialCategoryData_Part1_Option1 [controlling_so]

-- Section 3, Question 5 - 'Data Travel Outside UK - Country Selection' - is not applicable if Section 3, Question 4 - 'Data Travel Outside UK' - Option 1 - 'No' - IS selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] (QuestionSet, ControlledQuestionSetQuestion, ControllingSelectionOption, ControlledQuestionApplicabilityCondition)
SELECT [qs].[Id], [controlled_q].[Id], [controlling_so].Id, @ControlledQuestionApplicabilityCondition_QuestionIsNotApplicableIfOptionIsSelected
FROM @MasterQuestionSet [qs], @Section3_Question5 [controlled_q], @Question_DataTravelOutsideUk_Part1_Option1 [controlling_so]


-- ########################################################################################################
-- Question Set Selection Option Question Highlights

DECLARE @HighlightCondition_QuestionIsHighlightedIfOptionIsSelected NVARCHAR(64) = 'QuestionIsHighlightedIfOptionIsSelected';
DECLARE @HighlightCondition_QuestionIsHighlightedIfOptionIsNotSelected NVARCHAR(64) = 'QuestionIsHighlightedIfOptionIsNotSelected';

-- Question 'Other Organisations' Is Highlighted if Option 2 'Yes, we are collaborating with other organisations' IS selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionHighlight] (QuestionSet, SelectionOption, HighlightCondition, ReasonHighlighted)
SELECT [qs].[Id], [so].[Id], @HighlightCondition_QuestionIsHighlightedIfOptionIsSelected, 'Data will be accessed by other organisations.'
FROM  @MasterQuestionSet [qs], @Question_OtherOrganisations_Part1_Option2 [so]

-- Question 'Legal Power' Is Highlighted if Option 1 'Yes, we believe we have the legal power to do this' is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionHighlight] (QuestionSet, SelectionOption, HighlightCondition, ReasonHighlighted)
SELECT [qs].[Id], [so].[Id], @HighlightCondition_QuestionIsHighlightedIfOptionIsNotSelected, 'They do not have the legal power to request this data.'
FROM  @MasterQuestionSet [qs], @Question_LegalPower_Part1_Option1 [so]

-- Question 'Legal Review' Is Highlighted if Option 1 'Yes' is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionHighlight] (QuestionSet, SelectionOption, HighlightCondition, ReasonHighlighted)
SELECT [qs].[Id], [so].[Id], @HighlightCondition_QuestionIsHighlightedIfOptionIsNotSelected, 'Request has not been reviewed by someone with legal knowledge.'
FROM  @MasterQuestionSet [qs], @Question_LegalReview_Part1_Option1 [so]

-- Question 'Data Travel Outside UK' Is Highlighted if Option 2 'Yes, it will leave the UK' IS selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionHighlight] (QuestionSet, SelectionOption, HighlightCondition, ReasonHighlighted)
SELECT [qs].[Id], [so].[Id], @HighlightCondition_QuestionIsHighlightedIfOptionIsSelected, 'Data will travel outside of the UK.'
FROM  @MasterQuestionSet [qs], @Question_DataTravelOutsideUk_Part1_Option2 [so]

-- Question 'Data Protection Review' Is Highlighted if Option 1 'Yes' is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionHighlight] (QuestionSet, SelectionOption, HighlightCondition, ReasonHighlighted)
SELECT [qs].[Id], [so].[Id], @HighlightCondition_QuestionIsHighlightedIfOptionIsNotSelected, 'Request has not been reviewed by someone with knowledge of data protection.'
FROM  @MasterQuestionSet [qs], @Question_DataProtectionReview_Part1_Option1 [so]

-- Question 'Data Security Review' Is Highlighted if Option 1 'Yes' is NOT selected
INSERT INTO [dbo].[QuestionSetSelectionOptionQuestionHighlight] (QuestionSet, SelectionOption, HighlightCondition, ReasonHighlighted)
SELECT [qs].[Id], [so].[Id], @HighlightCondition_QuestionIsHighlightedIfOptionIsNotSelected, 'Request has not been reviewed by someone with knowledge of data governance and security.'
FROM  @MasterQuestionSet [qs], @Question_DataSecurityReview_Part1_Option1 [so]


-- ########################################################################################################
-- Question Set Question Part Answer Validation Rules

DECLARE @ValidationRuleId_FreeForm_Text_NoValueSupplied NVARCHAR(128) = 'FreeForm_Text_NoValueSupplied';
DECLARE @ValidationRuleId_FreeForm_Date_NoValueSupplied NVARCHAR(128) = 'FreeForm_Date_NoValueSupplied';
DECLARE @ValidationRuleId_FreeForm_Date_NotAValidDate NVARCHAR(128) = 'FreeForm_Date_NotAValidDate';
DECLARE @ValidationRuleId_FreeForm_Date_DateCannotBeInThePast NVARCHAR(128) = 'FreeForm_Date_DateCannotBeInThePast';
DECLARE @ValidationRuleId_FreeForm_Country_NoValueSupplied NVARCHAR(128) = 'FreeForm_Country_NoValueSupplied';
DECLARE @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected NVARCHAR(128) = 'OptionSelection_SelectSingle_NoOptionIsSelected';
DECLARE @ValidationRuleId_OptionSelection_SelectMulti_NoOptionIsSelected NVARCHAR(128) = 'OptionSelection_SelectMulti_NoOptionIsSelected';

-- Section 1, Question 1 - 'Data Types' - Part 1
-- At least one option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select a data type or none of the above'
FROM @MasterQuestionSet [qs], @Question_DataTypes_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectMulti_NoOptionIsSelected;

-- Section 1, Question 2 - 'Data Subjects' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter a description of the data subjects'
FROM @MasterQuestionSet [qs], @Question_DataSubjects_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 3 - 'Project Aims' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter aim of your project'
FROM @MasterQuestionSet [qs], @Question_ProjectAims_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 3 - 'Project Aims' - Part 2
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter how the data will help you achieve your aims'
FROM @MasterQuestionSet [qs], @Question_ProjectAims_Part2 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 4 - 'Data Required' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter description of data needed'
FROM @MasterQuestionSet [qs], @Question_DataRequired_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 5 - 'Benefit To Public' - Part 1
-- At least one option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select one or more benefits'
FROM @MasterQuestionSet [qs], @Question_BenefitToPublic_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectMulti_NoOptionIsSelected;

-- Section 1, Question 5 - 'Benefit To Public' - Part 1 - Option 'Something Else' Supplementary Part
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter how your project will provide this public benefit'
FROM @MasterQuestionSet [qs], @Question_BenefitToPublic_Part1_OptionSomethingElse_SupplementaryPart [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 6 - 'Other Organisations' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select No or Yes'
FROM @MasterQuestionSet [qs], @Question_OtherOrganisations_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;

-- Section 1, Question 7 - 'Other Organisations - OrganisationSelection' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter the name of an organisation'
FROM @MasterQuestionSet [qs], @Question_OtherOrganisationsOrganisationSelection_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 8 - 'Impact If Data Not Given' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter the impact of project if you do not receive the data'
FROM @MasterQuestionSet [qs], @Question_ImpactIfDataNotGiven_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 1, Question 9 - 'Date Required' - Part 1
-- Date must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter the latest date you need this data'
FROM @MasterQuestionSet [qs], @Question_DateRequired_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Date_NoValueSupplied;
-- Entered date is invalid
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Date is invalid'
FROM @MasterQuestionSet [qs], @Question_DateRequired_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Date_NotAValidDate;
-- Entered date is in the past
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Date must be in the future'
FROM @MasterQuestionSet [qs], @Question_DateRequired_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Date_DateCannotBeInThePast;


-- Section 2, Question 1 - 'Legal Power' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select Yes, No or we don’t know'
FROM @MasterQuestionSet [qs], @Question_LegalPower_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;

-- Section 2, Question 1 - 'Legal Power' - Part 1 - Option 'Yes' Supplementary Part
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter the legal power'
FROM @MasterQuestionSet [qs], @Question_LegalPower_Part1_OptionYes_SupplementaryPart [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 2, Question 3 - 'Legal Review' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select Yes or No'
FROM @MasterQuestionSet [qs], @Question_LegalReview_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;


-- Section 3, Question 1 - 'Lawful Basis For Personal Data' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select one or more options'
FROM @MasterQuestionSet [qs], @Question_LawfulBasisForPersonalData_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectMulti_NoOptionIsSelected;

-- Section 3, Question 2 - 'Lawful Basis For Special Category Data' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select one or more options'
FROM @MasterQuestionSet [qs], @Question_LawfulBasisForSpecialCategoryData_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectMulti_NoOptionIsSelected;

-- Section 3, Question 3 - 'Substantial Public Interest For Special Category Data' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select one or more options'
FROM @MasterQuestionSet [qs], @Question_SubstantialPublicInterestForSpecialCategoryData_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectMulti_NoOptionIsSelected;

-- Section 3, Question 4 - 'Data Travel Outside UK' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select No or Yes'
FROM @MasterQuestionSet [qs], @Question_DataTravelOutsideUk_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;

-- Section 3, Question 5 - 'Data Travel Outside UK - Country Selection' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select a country'
FROM @MasterQuestionSet [qs], @Question_DataTravelOutsideUkCountrySelection_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Country_NoValueSupplied;

-- Section 3, Question 6 - 'Role Of Organisation' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select ‘Controller’, ‘Joint Controller’, ‘Processor’ or ‘I don’t know’'
FROM @MasterQuestionSet [qs], @Question_RoleOfOrganisation_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;

-- Section 3, Question 7 - 'Data Protection Review' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select Yes or No'
FROM @MasterQuestionSet [qs], @Question_DataProtectionReview_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;


-- Section 4, Question 1 - 'Disposal Of Data' - Part 1
-- Text must be entered
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Enter how will you dispose of data'
FROM @MasterQuestionSet [qs], @Question_DisposalOfData_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_FreeForm_Text_NoValueSupplied;

-- Section 4, Question 2 - 'Data Security Review' - Part 1
-- An option must be selected
INSERT INTO [dbo].[QuestionSetQuestionPartAnswerValidation] (QuestionSet, QuestionPart, ValidationRule, ErrorText)
SELECT [qs].[Id], [qp].[Id], [avr].[Id], 'Select Yes or No'
FROM @MasterQuestionSet [qs], @Question_DataSecurityReview_Part1 [qp], [dbo].[QuestionPartAnswerValidationRule] [avr]
WHERE [avr].[ValidationRuleId] = @ValidationRuleId_OptionSelection_SelectSingle_NoOptionIsSelected;


-- ########################################################################################################
-- Compulsory Questions

INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataTypes [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataSubjects [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_ProjectAims [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataRequired [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_BenefitToPublic [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_OtherOrganisations [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_OtherOrganisationsOrganisationSelection [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_ImpactIfDataNotGiven [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DateRequired [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_LegalPower [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_LegalAdvice [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_LegalReview [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_LawfulBasisForPersonalData [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_LawfulBasisForSpecialCategoryData [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_SubstantialPublicInterestForSpecialCategoryData [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataTravelOutsideUk [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataTravelOutsideUkCountrySelection [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_RoleOfOrganisation [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataProtectionReview [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DisposalOfData [q]
INSERT INTO [dbo].[CompulsoryQuestion] SELECT [q].[Id] FROM @Question_DataSecurityReview [q]

