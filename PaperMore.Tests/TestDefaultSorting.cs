using PaperMore.Reports;

namespace PaperMore.Tests;

public class TestDefaultSorting
{
    public record SortingTestSet(
        List<DocumentReportData> InputDocs,
        List<DocumentReportData> ExpectedDocs,
        string TestCase);

    [Test]
    [TestCaseSource(nameof(DefaultSortingTestData))]
    public void TestSorting(SortingTestSet testSet)
    {
        List<DocumentReportData> actualDocs = testSet.InputDocs;
        actualDocs.Sort(Defaults.DefaultSorting);

        Assert.That(actualDocs, Is.EquivalentTo(testSet.ExpectedDocs), testSet.TestCase);
    }

    private static IEnumerable<SortingTestSet> DefaultSortingTestData()
    {
        yield return new SortingTestSet(
            [
                new("Reflections on Trusting Trust", 1, "Ken Thompson", DateTimeOffset.Parse("1984-08-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new("Hyper Text Coffee Pot Control Protocol", null, "L. Masinter", DateTimeOffset.Parse("1998-04-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new("How to share a secret", 2, "Adi Shamir", DateTimeOffset.Parse("1979-11-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new(
                    "Dutch courage? Effects of acute alcohol consumption on self-ratings and observer ratings of foreign language skills",
                    3, "Fritz Renner, Inge Kersbergen, Matt Dield, Jessica Werthmann",
                    DateTimeOffset.Parse("2017-10-18"), DateTimeOffset.Parse("2025-01-01"))
            ],
            [
                new("How to share a secret", 2, "Adi Shamir", DateTimeOffset.Parse("1979-11-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new(
                    "Dutch courage? Effects of acute alcohol consumption on self-ratings and observer ratings of foreign language skills",
                    3, "Fritz Renner, Inge Kersbergen, Matt Dield, Jessica Werthmann",
                    DateTimeOffset.Parse("2017-10-18"), DateTimeOffset.Parse("2025-01-01")),
                new("Reflections on Trusting Trust", 1, "Ken Thompson", DateTimeOffset.Parse("1984-08-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new("Hyper Text Coffee Pot Control Protocol", null, "L. Masinter", DateTimeOffset.Parse("1998-04-01"),
                    DateTimeOffset.Parse("2025-01-01")),
            ],
            "default correspondent filter"
        );
    }
}