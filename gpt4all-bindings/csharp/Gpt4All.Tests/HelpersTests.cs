using Xunit;

namespace Gpt4All.Tests;

public class HelpersTests
{
    [Fact]
    public void ImplementationSearchPathIsSet()
    {
        const string expectedPath = @"C:\my\implementation\path";

        Helpers.SetImplementationSearchPath(expectedPath);

        var actualPath = Helpers.GetImplementationSearchPath();

        Assert.Equal(expectedPath, actualPath);
    }
}
