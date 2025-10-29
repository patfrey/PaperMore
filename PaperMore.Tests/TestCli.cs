using PaperMore.CLI;

namespace PaperMore.Tests;

public class TestCli
{
    public record CliTestSet(string[] Args, CmdArgs? Result, int ReturnCode, string TestCase);

    private CmdParser _parser;
    private TextWriter _consoleOut;

    [SetUp]
    public void Setup()
    {
        _parser = new CmdParser();
        // Save Stdout
        _consoleOut = Console.Out;
        // Ignore all the help output
        Console.SetOut(TextWriter.Null);
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_consoleOut);
    }

    [Test]
    [TestCaseSource(nameof(GetSuccessArgTests))]
    public void TestParseShouldSucceed(CliTestSet testData)
    {
        CmdArgs? cmdArgs = null;
        int returnCode = _parser.TryParse(testData.Args, result => cmdArgs = result);

        Assert.That(returnCode, Is.EqualTo(testData.ReturnCode));
        Assert.That(cmdArgs, Is.Not.Null);
        Assert.That(cmdArgs.Url, Is.EqualTo(testData.Result!.Url));
        Assert.That(cmdArgs.Token, Is.EqualTo(testData.Result.Token));
        Assert.That(cmdArgs.Format, Is.EqualTo(testData.Result.Format));
        Assert.That(cmdArgs.BatchSize, Is.EqualTo(testData.Result.BatchSize));
        Assert.That(cmdArgs.BlankLines, Is.EqualTo(testData.Result.BlankLines));
    }

    [Test]
    [TestCaseSource(nameof(GetFailArgsTests))]
    public void TestParseShouldFail(CliTestSet testData)
    {
        CmdArgs? cmdArgs = null;
        int returnCode = _parser.TryParse(testData.Args, result => cmdArgs = result);

        Assert.That(returnCode, Is.EqualTo(testData.ReturnCode), testData.TestCase);
        Assert.That(cmdArgs, Is.Null, testData.TestCase);
    }

    private static IEnumerable<CliTestSet> GetSuccessArgTests()
    {
        yield return new CliTestSet(
            [
                "--url", "http://localhost:8080", "--token", "123456789", "--format", "pdf", "--path",
                "/home/test/index.pdf", "--blanklines", "25", "--batch-size", "100"
            ],
            new CmdArgs("http://localhost:8080", "123456789", FormatType.Pdf, "/home/test/index.pdf", 25, 100, null,
                null),
            0,
            "Long Params");
        yield return new CliTestSet(
            [
                "-u", "http://localhost:8080", "-t", "123456789", "-f", "pdf", "-p", "/home/test/index.pdf", "-b", "25",
                "-B", "100"
            ],
            new CmdArgs("http://localhost:8080", "123456789", FormatType.Pdf, "/home/test/index.pdf", 25, 100, null,
                null),
            0,
            "Short Params");
    }

    private static IEnumerable<CliTestSet> GetFailArgsTests()
    {
        yield return new CliTestSet(
            ["-t", "123456789", "-p", "/home/test/index.csv"],
            null,
            1,
            "no url provided"
        );
        yield return new CliTestSet(
            ["-u", "http://localhost:8080", "-p", "/home/test/index.csv"],
            null,
            1,
            "no token provided");
        yield return new CliTestSet(
            ["-u", "http://localhost:8080", "-t", "123456789"],
            null,
            1,
            "no path provided");
        yield return new CliTestSet(
            ["-u", "http://localhost:8080", "-t", "123456789", "-f", "csv", "-p", "/home/test/index.csv", "-b", "25"],
            null,
            1,
            "--blank-lines parameter only in pdf"
        );
        yield return new CliTestSet(
            ["-u", "http://localhost:8080", "-t", "123456789", "-f", "csv", "-p", "/home/test/index.csv", "-B", "-1"],
            null,
            1,
            "--batch-size less than 0"
        );
        yield return new CliTestSet(
            ["-u", "http://localhost:8080", "-t", "123456789", "-f", "csv", "-p", "/home/test/index.csv", "-b", "-1"],
            null,
            1,
            "--blank-lines less than 0"
        );
        yield return new CliTestSet(
            [
                "-u", "http://localhost:8080", "-t", "123456789", "-f", "csv", "-p", "/home/test/index.csv",
                "--asn-from", "100", "--asn-to", "50"
            ],
            null,
            1,
            "--asn-from higher than --asn-to"
        );
        yield return new CliTestSet(
            [
                "-u", "http://localhost:8080", "-t", "123456789", "-f", "csv", "-p", "/home/test/index.csv", "--asn-to",
                "-1"
            ],
            null,
            1,
            "--asn-to less than 0"
        );
    }
}