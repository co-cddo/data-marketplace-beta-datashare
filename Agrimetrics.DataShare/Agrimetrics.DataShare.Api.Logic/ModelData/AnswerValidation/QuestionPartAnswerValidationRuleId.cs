

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

/// <remarks>
/// Values in this enum must match those in the database table 'QuestionPartAnswerValidationRuleId'
/// </remarks>
public enum QuestionPartAnswerValidationRuleId
{
    FreeForm_Text_NoValueSupplied,
    FreeForm_Number_NoValueSupplied,
    FreeForm_Number_NotAValidNumber,

    FreeForm_Date_NoValueSupplied,
    FreeForm_Date_NotAValidDate,
    FreeForm_Date_DateCannotBeInThePast,
    FreeForm_Date_DateCannotBeInTheFuture,
	  
    FreeForm_Time_NoValueSupplied,
    FreeForm_Time_NotAValidTime,

    FreeForm_DateTime_NoValueSupplied,
    FreeForm_DateTime_NotAValidDateTime,
    FreeForm_DateTime_DateTimeCannotBeInThePast,
    FreeForm_DateTime_DateTimeCannotBeInTheFuture,

    OptionSelection_SelectSingle_NoOptionIsSelected,

    OptionSelection_SelectMulti_NoOptionIsSelected,

    FreeForm_Country_NoValueSupplied
}