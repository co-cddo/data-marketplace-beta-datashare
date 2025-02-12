using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionContentAsFileTests
{
    [Test]
    public void GivenASubmissionContentAsFile_WhenISetContent_ThenContentIsSet()
    {
        var testContent = new byte[]{0, 1, 2};

        var testSubmissionContentAsFile = new SubmissionContentAsFile
        {
            Content = testContent,
            ContentType = "_",
            FileName = "_"
        };

        var result = testSubmissionContentAsFile.Content;

        Assert.That(result, Is.EqualTo(testContent));
    }

    [Test]
    public void GivenASubmissionContentAsFile_WhenISetContentType_ThenContentTypeIsSet(
        [Values("", "  ", "abc")] string testContentType)
    {
        var testSubmissionContentAsFile = new SubmissionContentAsFile
        {
            Content = [],
            ContentType = testContentType,
            FileName = "_"
        };

        var result = testSubmissionContentAsFile.ContentType;

        Assert.That(result, Is.EqualTo(testContentType));
    }

    [Test]
    public void GivenASubmissionContentAsFile_WhenISetFileName_ThenFileNameIsSet(
        [Values("", "  ", "abc")] string testFileName)
    {
        var testSubmissionContentAsFile = new SubmissionContentAsFile
        {
            Content = [],
            ContentType = "_",
            FileName = testFileName
        };

        var result = testSubmissionContentAsFile.FileName;

        Assert.That(result, Is.EqualTo(testFileName));
    }
}