using Microsoft.Extensions.Time.Testing;
using PaperMore.Reports;

namespace PaperMore.Tests;

[TestFixture]
public class TestPdfGenerator
{
    [Test]
    public void TestGeneration()
    {
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

        string tempFileName = Path.GetTempFileName();
        try
        {
            using Stream stream = File.OpenWrite(tempFileName);
            PdfGenerator generator = new(timeProvider);

            generator.Generate(testData, Defaults.DefaultSorting, Defaults.DefaultFilter(null, null, false), stream);

            FileInfo testFileInfo = new(tempFileName);
            Assert.IsTrue(testFileInfo.Exists);
            Assert.That(testFileInfo.Length, Is.GreaterThan(0));
        }
        finally
        {
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);
        }
    }
}