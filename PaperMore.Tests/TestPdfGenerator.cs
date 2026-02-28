using Microsoft.Extensions.Time.Testing;
using PaperMore.Reports;

namespace PaperMore.Tests;

[TestFixture]
public class TestPdfGenerator
{
    [Test]
    public void TestGeneration()
    {
        // QuestPDF will may change their internal structure
        // Update this when updating to a new version
        // For the future: find a better was to compare documents
        const string expectedDataHash = "9fa78c676bc403a5d15d0a403fdb6db9df2b681529906bd3d15399bffdd79a8d";

        FakeTimeProvider timeProvider = new();


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

        // using MemoryStream stream = new();
        using Stream stream = File.OpenWrite("E:\\test.pdf");
        PdfGenerator generator = new(timeProvider);
        generator.Generate(testData, Defaults.DefaultSorting, Defaults.DefaultFilter(null, null, false), stream);


        // byte[] actualDataHashBytes = SHA256.HashData(stream.ToArray());
        // string actualDataHash = Convert.ToHexStringLower(actualDataHashBytes);
        //
        // Assert.That(actualDataHash, Is.EqualTo(expectedDataHash));
    }
}