using System.Diagnostics.CodeAnalysis;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using Moq;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations
{
    [TestFixture]
    public class DataShareRequestQuestionStatusesDeterminationTests
    {
        #region DetermineQuestionStatuses() Tests
        #region Input Parameter Validation
        [Test]
        public void GivenANullQuestionStatusInformationSet_WhenIDetermineQuestionStatuses_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestionStatusInformationSetModelData"));
        }
        #endregion

        #region Questions Remain That Require A Response
        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Multiple enumeration in mock verification")]
        public void GivenQuestionsRemainThatRequireAResponse_WhenIDetermineQuestionStatuses_ThenQuestionsRemainThatRequireAResponseIsTrue()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("56B7D96B-E0A6-4B66-87CF-D2EC68843E58")),
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("67E81EB1-C77E-4FAA-9577-4AE8911EEB29")),
                ]
            };

            testItems.MockDataShareRequestQuestionSetCompletedDetermination.Setup(x => x.DetermineDataShareRequestQuestionSetCompleteness(
                 It.Is<IEnumerable<IDataShareRequestQuestionSetQuestionStatusDataModel>>(questionStatuses =>
                     questionStatuses.Count() == 2 &&
                     questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("56B7D96B-E0A6-4B66-87CF-D2EC68843E58")) &&
                     questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("67E81EB1-C77E-4FAA-9577-4AE8911EEB29")))))
                .Returns(() =>
                {
                    var questionRequiringAResponse = testItems.Fixture.Build<DataShareRequestQuestionSetQuestionStatusDataModel>()
                        .With(x => x.QuestionId, Guid.Parse("67E81EB1-C77E-4FAA-9577-4AE8911EEB29"))
                        .Create();

                    var questionSetCompletenessDeterminationResult = testItems.Fixture.Build<DataShareRequestQuestionSetCompletenessDeterminationResult>()
                        .With(x => x.QuestionsRequiringAResponse, [questionRequiringAResponse]);

                    return questionSetCompletenessDeterminationResult.Create();
                });

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionsRemainThatRequireAResponse, Is.True);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Multiple enumeration in mock verification")]
        public void GivenQuestionDoNotRemainThatRequireAResponse_WhenIDetermineQuestionStatuses_ThenQuestionsRemainThatRequireAResponseIsFalse()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("56B7D96B-E0A6-4B66-87CF-D2EC68843E58")),
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("67E81EB1-C77E-4FAA-9577-4AE8911EEB29")),
                ]
            };

            testItems.MockDataShareRequestQuestionSetCompletedDetermination.Setup(x => x.DetermineDataShareRequestQuestionSetCompleteness(
                 It.Is<IEnumerable<IDataShareRequestQuestionSetQuestionStatusDataModel>>(questionStatuses =>
                     questionStatuses.Count() == 3 &&
                     questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("56B7D96B-E0A6-4B66-87CF-D2EC68843E58")) &&
                     questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("67E81EB1-C77E-4FAA-9577-4AE8911EEB29")))))
                .Returns(() =>
                {
                    var questionSetCompletenessDeterminationResult = testItems.Fixture.Build<DataShareRequestQuestionSetCompletenessDeterminationResult>()
                        .With(x => x.QuestionsRequiringAResponse, []);

                    return questionSetCompletenessDeterminationResult.Create();
                });

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionsRemainThatRequireAResponse, Is.False);
        }
        #endregion

        #region Question Statuses
        #region Response Structure
        [Test]
        public void GivenAQuestionStatusInformationSet_WhenIDetermineQuestionStatuses_ThenAResultIsReturnedForEachQuestionInTheQuestionStatusInformationSet()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"), sectionNumber:1, questionNumberWithinSection:3),
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("B11401AD-10ED-4629-9313-C1D0C3055E3B"), sectionNumber:2, questionNumberWithinSection:1),
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"), sectionNumber:4, questionNumberWithinSection:10),
                ]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.Multiple(() =>
            {
                Assert.That(result.QuestionStatusDeterminationResults, Has.Exactly(3).Items);

                Assert.That(result.QuestionStatusDeterminationResults.Any(x => x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494") && x.QuestionSetQuestionStatusData is {SectionNumber: 1, QuestionOrderWithinSection: 3}));
                Assert.That(result.QuestionStatusDeterminationResults.Any(x => x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("B11401AD-10ED-4629-9313-C1D0C3055E3B") && x.QuestionSetQuestionStatusData is {SectionNumber: 2, QuestionOrderWithinSection: 1}));
                Assert.That(result.QuestionStatusDeterminationResults.Any(x => x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") && x.QuestionSetQuestionStatusData is {SectionNumber: 4, QuestionOrderWithinSection: 10}));
            });
        }

        [Test]
        public void GivenAQuestionStatusInformationSet_WhenIDetermineQuestionStatuses_ThenThePreviousQuestionStateOfEachQuestionIsReturned()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"), questionStatusType: QuestionStatusType.NoResponseNeeded),
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("B11401AD-10ED-4629-9313-C1D0C3055E3B"), questionStatusType: QuestionStatusType.Completed),
                    CreateTestDataShareRequestQuestionStatusInformationModelData(testItems, questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"), questionStatusType: QuestionStatusType.NotApplicable)
                ]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.Multiple(() =>
            {
                Assert.That(result.QuestionStatusDeterminationResults, Has.Exactly(3).Items);

                Assert.That(result.QuestionStatusDeterminationResults.Any(x => 
                    x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494") && 
                    x.PreviousQuestionStatus == QuestionStatusType.NoResponseNeeded));

                Assert.That(result.QuestionStatusDeterminationResults.Any(x => 
                    x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("B11401AD-10ED-4629-9313-C1D0C3055E3B") && 
                    x.PreviousQuestionStatus == QuestionStatusType.Completed));

                Assert.That(result.QuestionStatusDeterminationResults.Any(x => 
                    x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") && 
                    x.PreviousQuestionStatus == QuestionStatusType.NotApplicable));
            });
        }
        #endregion

        #region Question Set Question Applicability Override
        #region Configuration Validation
        [Test]
        public void GivenAQuestionSetQuestionHasAnApplicabilityOverrideWithAnInvalidConditionType_WhenIDetermineQuestionStatuses_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            var questionSetQuestionApplicabilityCondition = (QuestionSetQuestionApplicabilityConditionType) Enum.GetValues<QuestionSetQuestionApplicabilityConditionType>().Cast<int>().Max() + 1;

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionApplicabilityOverrides: [
                    CreateTestQuestionApplicabilityOverride(testItems,
                        controlledQuestionApplicabilityCondition: questionSetQuestionApplicabilityCondition,
                        controllingSelectionOptionIsSelected: It.IsAny<bool>())]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            Assert.That(() => testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet),
                Throws.InvalidOperationException.With.Message.EqualTo("Question has unknown Applicability Condition"));
        }
        #endregion

        #region Question Is Not Applicable If Option Is Selected
        [Test]
        public void GivenAQuestionIsNotApplicableIfARelatedOptionIsSelectedAndTheOptionIsSelected_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNotApplicable()
        {
            var testItems = CreateTestItems();

            // In reality for a question to be controlled by the selection options of another question then that other question must of course exist,
            // however by the time the information is received at this point, all of that linkage has been made and forgotten about, with the question
            // status information just including whether the option has been selected.
            // Therefore, this testing can be made using only the 'controlled' question, and not requiring the 'controlling' question
            var testControlledQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionApplicabilityOverrides: [
                    CreateTestQuestionApplicabilityOverride(testItems,
                        controlledQuestionApplicabilityCondition: QuestionSetQuestionApplicabilityConditionType.QuestionIsNotApplicableIfOptionIsSelected,
                        controllingSelectionOptionIsSelected: true)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testControlledQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.NotApplicable));
        }

        [Test]
        public void GivenAQuestionIsNotApplicableIfARelatedOptionIsSelectedAndTheOptionIsNotSelected_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNotNotApplicable()
        {
            var testItems = CreateTestItems();

            // In reality for a question to be controlled by the selection options of another question then that other question must of course exist,
            // however by the time the information is received at this point, all of that linkage has been made and forgotten about, with the question
            // status information just including whether the option has been selected.
            // Therefore, this testing can be made using only the 'controlled' question, and not requiring the 'controlling' question
            var testControlledQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionApplicabilityOverrides: [
                    CreateTestQuestionApplicabilityOverride(testItems,
                        controlledQuestionApplicabilityCondition: QuestionSetQuestionApplicabilityConditionType.QuestionIsNotApplicableIfOptionIsSelected,
                        controllingSelectionOptionIsSelected: false)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testControlledQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus != QuestionStatusType.NotApplicable));
        }
        #endregion

        #region Question Is Not Applicable If Option Is Not Selected
        [Test]
        public void GivenAQuestionIsNotApplicableIfARelatedOptionIsNotSelectedAndTheOptionIsNotSelected_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNotApplicable()
        {
            var testItems = CreateTestItems();

            // In reality for a question to be controlled by the selection options of another question then that other question must of course exist,
            // however by the time the information is received at this point, all of that linkage has been made and forgotten about, with the question
            // status information just including whether the option has been selected.
            // Therefore, this testing can be made using only the 'controlled' question, and not requiring the 'controlling' question
            var testControlledQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionApplicabilityOverrides: [
                    CreateTestQuestionApplicabilityOverride(testItems,
                        controlledQuestionApplicabilityCondition: QuestionSetQuestionApplicabilityConditionType.QuestionIsNotApplicableIfOptionIsNotSelected,
                        controllingSelectionOptionIsSelected: false)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testControlledQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.NotApplicable));
        }

        [Test]
        public void GivenAQuestionIsNotApplicableIfARelatedOptionIsNotSelectedAndTheOptionIsSelected_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNotNotApplicable()
        {
            var testItems = CreateTestItems();

            // In reality for a question to be controlled by the selection options of another question then that other question must of course exist,
            // however by the time the information is received at this point, all of that linkage has been made and forgotten about, with the question
            // status information just including whether the option has been selected.
            // Therefore, this testing can be made using only the 'controlled' question, and not requiring the 'controlling' question
            var testControlledQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionApplicabilityOverrides: [
                    CreateTestQuestionApplicabilityOverride(testItems,
                        controlledQuestionApplicabilityCondition: QuestionSetQuestionApplicabilityConditionType.QuestionIsNotApplicableIfOptionIsNotSelected,
                        controllingSelectionOptionIsSelected: true)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testControlledQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus != QuestionStatusType.NotApplicable));
        }
        #endregion
        #endregion

        #region Question Does Not Require A Response
        [Test]
        public void GivenAQuestionHasASinglePartThatHasInputTypeNone_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNoResponseNeeded()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.None)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.NoResponseNeeded));
        }

        [Test]
        public void GivenAQuestionHasAMultiplePartsThatAllHaveInputTypeNone_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNoResponseNeeded()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPartResponses: [
                    CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.None),
                    CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.None),
                    CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.None)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.NoResponseNeeded));
        }

        [Test]
        public void GivenAQuestionHasAMultiplePartsAndAtLeastOneHasInputTypeOtherThanNone_WhenIDetermineQuestionStatuses_ThenTheQuestionStatusIsNotNoResponseNeeded()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPartResponses: [
                    CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.None),
                    CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.OptionSelection),
                    CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.None)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus != QuestionStatusType.NoResponseNeeded));
        }
        #endregion

        #region Question Has A Response
        [Test]
        public void GivenAQuestionHasAResponse_WhenIDetermineQuestionStatuses_ThenQuestionStatusIsCompleted()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems,
                    responseInputType: QuestionPartResponseInputType.FreeForm,
                    answerPartResponseId: Guid.Parse("DBAAAA48-3D43-4984-9C81-68E10CDF8B41"))]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.Completed));
        }

        [Test]
        public void GivenAQuestionDoesNotHaveAResponse_WhenIDetermineQuestionStatuses_ThenQuestionStatusIsNotCompleted()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems,
                    responseInputType: QuestionPartResponseInputType.FreeForm,
                    answerPartResponseId: null)]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus != QuestionStatusType.Completed));
        }
        #endregion

        #region Question Has A Pre-Requisite
        [Test]
        public void GivenAQuestionHasAPreRequisiteThatRequiresAResponse_WhenIDetermineQuestionStatuses_ThenQuestionStatusIsCannotStartYet()
        {
            var testItems = CreateTestItems();

            var preRequisiteQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems,
                    responseInputType: QuestionPartResponseInputType.FreeForm,
                    answerPartResponseId: null)]);

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPreRequisites: [
                    CreateTestQuestionPreRequisiteDataModel(testItems,
                        preRequisiteQuestionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"))
                ]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    preRequisiteQuestionStatusInformation,
                    testQuestionStatusInformation
                ]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.CannotStartYet));
        }

        [Test]
        public void GivenAQuestionHasAPreRequisiteThatHasInputTypeNone_WhenIDetermineQuestionStatuses_ThenQuestionStatusIsNotCannotStartYet()
        {
            var testItems = CreateTestItems();

            var preRequisiteQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems,
                    responseInputType: QuestionPartResponseInputType.None)]);

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPreRequisites: [
                    CreateTestQuestionPreRequisiteDataModel(testItems,
                        preRequisiteQuestionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"))
                ]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    preRequisiteQuestionStatusInformation,
                    testQuestionStatusInformation
                ]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus != QuestionStatusType.CannotStartYet));
        }

        [Test]
        public void GivenAQuestionHasAPreRequisiteThatHasAResponse_WhenIDetermineQuestionStatuses_ThenQuestionStatusIsNotCannotStartYet()
        {
            var testItems = CreateTestItems();

            var preRequisiteQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems,
                    responseInputType: QuestionPartResponseInputType.FreeForm,
                    answerPartResponseId: Guid.Parse("6363C684-5E70-4069-B10A-F71AD3EDD62D"))]);

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPreRequisites: [
                    CreateTestQuestionPreRequisiteDataModel(testItems,
                        preRequisiteQuestionId: Guid.Parse("4BD66C98-336F-46F2-B2DD-E0E0367C8494"))
                ]);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses =
                [
                    preRequisiteQuestionStatusInformation,
                    testQuestionStatusInformation
                ]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus != QuestionStatusType.CannotStartYet));
        }
        #endregion

        #region Question Not Started
        [Test]
        public void GivenAQuestionHasNotHadAResponse_WhenIDetermineQuestionStatuses_ThenQuestionStatusIsNotStarted()
        {
            var testItems = CreateTestItems();

            var testQuestionStatusInformation = CreateTestDataShareRequestQuestionStatusInformationModelData(testItems,
                questionId: Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA"),
                questionPartResponses: [CreateTestQuestionPartResponseDataModel(testItems,
                    responseInputType: QuestionPartResponseInputType.FreeForm,
                    answerPartResponseId: null)],
                questionPreRequisites:[],
                questionApplicabilityOverrides: []);

            var testQuestionStatusInformationSet = new DataShareRequestQuestionStatusInformationSetModelData
            {
                DataShareRequestQuestionStatuses = [testQuestionStatusInformation]
            };

            var result = testItems.DataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(testQuestionStatusInformationSet);

            Assert.That(result.QuestionStatusDeterminationResults.Any(x =>
                x.QuestionSetQuestionStatusData.QuestionId == Guid.Parse("68B7552E-DA92-40B3-962D-8D808A4B61EA") &&
                x.QuestionSetQuestionStatusData.QuestionStatus == QuestionStatusType.NotStarted));
        }
        #endregion
        #endregion
        #endregion

        #region Test Data Creation
        private static QuestionPartResponseDataModel CreateTestQuestionPartResponseDataModel(
            TestItems testItems,
            QuestionPartResponseInputType? responseInputType = null,
            Guid? answerPartResponseId = null)
        {
            return testItems.Fixture.Build<QuestionPartResponseDataModel>()
                .With(x => x.QuestionPartResponse_ResponseInputType, responseInputType ?? It.IsAny<QuestionPartResponseInputType>())
                .With(x => x.QuestionPartResponse_AnswerPartResponseId, answerPartResponseId)
                .Create();
        }

        private static QuestionPreRequisiteDataModel CreateTestQuestionPreRequisiteDataModel(
            TestItems testItems,
            Guid preRequisiteQuestionId)
        {
            return testItems.Fixture.Build<QuestionPreRequisiteDataModel>()
                .With(x => x.QuestionPreRequisite_PreRequisiteQuestionId, preRequisiteQuestionId)
                .Create();
        }

        private static QuestionSetQuestionApplicabilityOverride CreateTestQuestionApplicabilityOverride(TestItems testItems,
            QuestionSetQuestionApplicabilityConditionType controlledQuestionApplicabilityCondition,
            bool controllingSelectionOptionIsSelected)
        {
            return testItems.Fixture.Build<QuestionSetQuestionApplicabilityOverride>()
                .With(x => x.QuestionSetQuestionApplicabilityOverride_ControlledQuestionApplicabilityCondition, controlledQuestionApplicabilityCondition)
                .With(x => x.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected, controllingSelectionOptionIsSelected)
                .Create();
        }

        private static DataShareRequestQuestionStatusInformationModelData CreateTestDataShareRequestQuestionStatusInformationModelData(
            TestItems testItems,
            Guid? questionId = null,
            int? sectionNumber = null,
            int? questionNumberWithinSection = null,
            QuestionStatusType? questionStatusType = null,
            List<QuestionPartResponseDataModel>? questionPartResponses = null,
            List<QuestionPreRequisiteDataModel>? questionPreRequisites = null,
            List<QuestionSetQuestionApplicabilityOverride>? questionApplicabilityOverrides = null)
        {
            var testQuestionSetQuestionInformation = testItems.Fixture.Build<QuestionSetQuestionInformationModelData>()
                .With(x => x.QuestionSet_SectionNumber, sectionNumber ?? It.IsAny<int>())
                .With(x => x.QuestionSet_QuestionOrerWithinSection, questionNumberWithinSection ?? It.IsAny<int>())
                .Create();

            var testQuestionResponseInformationDataModel = testItems.Fixture.Build<QuestionResponseInformationDataModel>()
                .With(x => x.QuestionResponseInformation_QuestionStatusType, questionStatusType ?? It.IsAny<QuestionStatusType>())
                .With(x => x.QuestionResponseInformation_QuestionPartResponses, questionPartResponses ?? [ CreateTestQuestionPartResponseDataModel(testItems, responseInputType: QuestionPartResponseInputType.FreeForm)])
                .Create();

            return testItems.Fixture.Build<DataShareRequestQuestionStatusInformationModelData>()
                .With(x => x.DataShareRequestQuestionStatus_QuestionId, questionId ?? Guid.NewGuid())
                .With(x => x.QuestionSetQuestionInformation, testQuestionSetQuestionInformation)
                .With(x => x.QuestionResponseInformation, testQuestionResponseInformationDataModel)
                .With(x => x.QuestionPreRequisites, questionPreRequisites ?? [])
                .With(x => x.SelectionOptionQuestionSetQuestionApplicabilityOverrides, questionApplicabilityOverrides ?? [])
                .Create();
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockDataShareRequestQuestionSetCompletedDetermination = Mock.Get(fixture.Create<IDataShareRequestQuestionSetCompletenessDetermination>());

            var dataShareRequestQuestionStatusesDetermination = new DataShareRequestQuestionStatusesDetermination(
                mockDataShareRequestQuestionSetCompletedDetermination.Object);

            return new TestItems(fixture,
                dataShareRequestQuestionStatusesDetermination,
                mockDataShareRequestQuestionSetCompletedDetermination);
        }

        private class TestItems(
            IFixture fixture,
            IDataShareRequestQuestionStatusesDetermination dataShareRequestQuestionStatusesDetermination,
            Mock<IDataShareRequestQuestionSetCompletenessDetermination> mockDataShareRequestQuestionSetCompletedDetermination)
        {
            public IFixture Fixture { get; } = fixture;
            public IDataShareRequestQuestionStatusesDetermination DataShareRequestQuestionStatusesDetermination { get; } = dataShareRequestQuestionStatusesDetermination;
            public Mock<IDataShareRequestQuestionSetCompletenessDetermination> MockDataShareRequestQuestionSetCompletedDetermination { get; } = mockDataShareRequestQuestionSetCompletedDetermination;
        }
        #endregion
    }
}
