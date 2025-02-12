using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.QuestionConfiguration;

[TestFixture]
public class QuestionConfigurationModelDataFactoryTests
{
    #region CreateCompulsoryQuestionSet() Tests
    [Test]
    public void GivenANullCollectionOfCompulsoryQuestionModelData_WhenICreateCompulsoryQuestionSet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationModelDataFactory.CreateCompulsoryQuestionSet(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("compulsoryQuestionModelDatas"));
    }

    [Test]
    public void GivenACollectionOfCompulsoryQuestionModelDataContainingANullItem_WhenICreateCompulsoryQuestionSet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        testCompulsoryQuestionModelDatas.Insert(0, null!);

        Assert.That(() => testItems.QuestionConfigurationModelDataFactory.CreateCompulsoryQuestionSet(testCompulsoryQuestionModelDatas),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("compulsoryQuestionModelData"));
    }

    [Test]
    public void GivenACollectionOfCompulsoryQuestionModelData_WhenICreateCompulsoryQuestionSet_ThenACompulsoryQuestionSetIsCreatedContainingEachQuestionId()
    {
        var testItems = CreateTestItems();

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        var result = testItems.QuestionConfigurationModelDataFactory.CreateCompulsoryQuestionSet(testCompulsoryQuestionModelDatas).CompulsoryQuestions;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testCompulsoryQuestionModelDatas.Count).Items);

            foreach (var testCompulsoryQuestionModelData in testCompulsoryQuestionModelDatas)
            {
                Assert.That(result.FirstOrDefault(x => x.QuestionId == testCompulsoryQuestionModelData.CompulsoryQuestion_QuestionId),
                    Is.Not.Null);
            }
        });
    }
    #endregion

    #region CreateCompulsorySupplierMandatedQuestionSet() Tests
    [Test]
    public void GivenANullCollectionOfCompulsorySupplierMandatedQuestionModelData_WhenICreateCompulsorySupplierMandatedQuestionSet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationModelDataFactory.CreateCompulsorySupplierMandatedQuestionSet(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("compulsorySupplierMandatedQuestionModelDatas"));
    }

    [Test]
    public void GivenACollectionOfCompulsorySupplierMandatedQuestionModelDataContainingANullItem_WhenICreateCompulsorySupplierMandatedQuestionSet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        testCompulsorySupplierMandatedQuestionModelDatas.Insert(0, null!);

        Assert.That(() => testItems.QuestionConfigurationModelDataFactory.CreateCompulsorySupplierMandatedQuestionSet(testCompulsorySupplierMandatedQuestionModelDatas),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("compulsorySupplierMandatedQuestionModelData"));
    }

    [Test]
    public void GivenACollectionOfCompulsoryQuestionModelData_WhenICreateCompulsorySupplierMandatedQuestionSet_ThenACompulsoryQuestionSetIsCreatedContainingEachQuestionId()
    {
        var testItems = CreateTestItems();

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        var result = testItems.QuestionConfigurationModelDataFactory.CreateCompulsorySupplierMandatedQuestionSet(testCompulsorySupplierMandatedQuestionModelDatas).CompulsorySupplierMandatedQuestions;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testCompulsorySupplierMandatedQuestionModelDatas.Count).Items);

            foreach (var testCompulsorySupplierMandatedQuestionModelData in testCompulsorySupplierMandatedQuestionModelDatas)
            {
                Assert.That(result.FirstOrDefault(x =>
                        x.SupplierOrganisationId == testCompulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_SupplierOrganisationId &&
                        x.QuestionId == testCompulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_QuestionId),
                    Is.Not.Null);
            }
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var questionConfigurationModelDataFactory = new QuestionConfigurationModelDataFactory();

        return new TestItems(
            fixture,
            questionConfigurationModelDataFactory);
    }

    private class TestItems(
        IFixture fixture,
        IQuestionConfigurationModelDataFactory questionConfigurationModelDataFactory)
    {
        public IFixture Fixture { get; } = fixture;

        public IQuestionConfigurationModelDataFactory QuestionConfigurationModelDataFactory { get; } = questionConfigurationModelDataFactory;
    }
    #endregion
}
