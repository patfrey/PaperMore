using PaperMore.CLI;

namespace PaperMore.Tests;

public class TestCli
{
    private CmdParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new CmdParser();
    }

    [Test]
    [TestCaseSource(nameof(GetSuccessArgTests))]
    public void TestParseShouldSucceed((string[], CmdArgs, int) testData)
    {
        CmdArgs? cmdArgs = null;
        int returnCode = _parser.TryParse(testData.Item1, result => cmdArgs = result);
        
        Assert.That(returnCode, Is.EqualTo(testData.Item3));
        Assert.That(cmdArgs, Is.Not.Null);
        Assert.That(cmdArgs.Url, Is.EqualTo(testData.Item2.Url));
        Assert.That(cmdArgs.Token, Is.EqualTo(testData.Item2.Token));
        Assert.That(cmdArgs.Format, Is.EqualTo(testData.Item2.Format));
        Assert.That(cmdArgs.BatchSize, Is.EqualTo(testData.Item2.BatchSize));
        Assert.That(cmdArgs.BlankLines, Is.EqualTo(testData.Item2.BlankLines));
    }
    
    [Test]
    [TestCaseSource(nameof(GetFailArgsTests))]
    public void TestParseShouldFail((string[], CmdArgs, int) testData)
    {
        CmdArgs? cmdArgs = null;
        int returnCode = _parser.TryParse(testData.Item1, result => cmdArgs = result);
        
        Assert.That(returnCode, Is.EqualTo(testData.Item3));
        Assert.That(cmdArgs, Is.Null);
    }
        
    private static IEnumerable<(string[], CmdArgs, int)> GetSuccessArgTests()
    {
        yield return (new[] { "--url", "http://localhost:8080", "--token", "123456789", "--format", "pdf", "--output", "/home/test/index.pdf", "--blanklines", "25", "--batch-size", "100" }, new CmdArgs("http://localhost:8080", "123456789", FormatType.Pdf, "/home/test/index.pdf", 25, 100, null, null), 0);
        yield return (new[] { "-u", "http://localhost:8080", "-t", "123456789", "-f", "pdf", "-o", "/home/test/index.pdf", "-b", "25", "-B", "100" }, new CmdArgs("http://localhost:8080", "123456789", FormatType.Pdf, "/home/test/index.pdf", 25, 100, null, null), 0);
        
    }

    private static IEnumerable<(string[], CmdArgs, int)> GetFailArgsTests()
    {
        yield return (new[] { "-u", "http://localhost:8080", "-t", "123456789", "-f", "csv", "-o", "/home/test/index.pdf", "-b", "25", "-B", "100" }, new CmdArgs("http://localhost:8080", "123456789", FormatType.Csv, "/home/test/index.pdf", 25, 100, null, null), 1);
    }
}