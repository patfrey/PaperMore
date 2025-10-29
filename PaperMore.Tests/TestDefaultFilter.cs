using PaperMore.Reports;

namespace PaperMore.Tests;

public class TestDefaultFilter
{
    public record FilterTestSet(
        List<DocumentReportData> InputDocs,
        long? AsnUpperBound,
        long? AsnLowerBound,
        List<DocumentReportData> ExpectedDocs,
        string TestCase);


    [Test]
    [TestCaseSource(nameof(DefaultFilterTestData))]
    public void TestFilter(FilterTestSet testSet)
    {
        Func<DocumentReportData, bool> filter = Defaults.DefaultFilter(testSet.AsnLowerBound, testSet.AsnUpperBound);
        List<DocumentReportData> actualList = testSet.InputDocs.Where(filter).ToList();

        Assert.That(actualList, Is.EquivalentTo(testSet.ExpectedDocs), testSet.TestCase);
    }

    private static IEnumerable<FilterTestSet> DefaultFilterTestData()
    {
        yield return new FilterTestSet(
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
            null,
            null,
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
            "no asn filter applied"
        );

        yield return new FilterTestSet(
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
            3,
            1,
            [
                new("Reflections on Trusting Trust", 1, "Ken Thompson", DateTimeOffset.Parse("1984-08-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new("How to share a secret", 2, "Adi Shamir", DateTimeOffset.Parse("1979-11-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new(
                    "Dutch courage? Effects of acute alcohol consumption on self-ratings and observer ratings of foreign language skills",
                    3, "Fritz Renner, Inge Kersbergen, Matt Dield, Jessica Werthmann",
                    DateTimeOffset.Parse("2017-10-18"), DateTimeOffset.Parse("2025-01-01"))
            ],
            "filter from 1 to 3 - no null ASNs"
        );

        yield return new FilterTestSet(
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
            2,
            null,
            [
                new("Reflections on Trusting Trust", 1, "Ken Thompson", DateTimeOffset.Parse("1984-08-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new("How to share a secret", 2, "Adi Shamir", DateTimeOffset.Parse("1979-11-01"),
                    DateTimeOffset.Parse("2025-01-01")),
            ],
            "asn upper limit 2"
        );

        yield return new FilterTestSet(
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
            null,
            2,
            [
                new("How to share a secret", 2, "Adi Shamir", DateTimeOffset.Parse("1979-11-01"),
                    DateTimeOffset.Parse("2025-01-01")),
                new(
                    "Dutch courage? Effects of acute alcohol consumption on self-ratings and observer ratings of foreign language skills",
                    3, "Fritz Renner, Inge Kersbergen, Matt Dield, Jessica Werthmann",
                    DateTimeOffset.Parse("2017-10-18"), DateTimeOffset.Parse("2025-01-01"))
            ],
            "asn lower limit 2"
        );
    }
}