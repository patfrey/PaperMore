using System.Security.Cryptography;
using PaperMore.Reports;

namespace PaperMore.Tests;

[TestFixture]
public class TestCsvGenerator
{
    [Test]
    public void TestGeneration()
    {
        const string expectedDataHash = "895946a332232da40cf213f0b2b785dfcc140fab53f295534da0748f5d7a0365";

        List<DocumentReportData> testData =
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
        ];

        using MemoryStream stream = new();
        CsvGenerator generator = new();
        generator.Generate(testData, Defaults.DefaultSorting, Defaults.DefaultFilter(null, null, false),
            new ExportFormatOptions("dd.MM.yyyy"), stream);

        byte[] actualDataHashBytes = SHA256.HashData(stream.ToArray());
        string actualDataHash = Convert.ToHexStringLower(actualDataHashBytes);

        Assert.That(actualDataHash, Is.EqualTo(expectedDataHash));
    }
}