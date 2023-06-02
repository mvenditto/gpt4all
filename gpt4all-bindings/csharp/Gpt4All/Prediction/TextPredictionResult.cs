using System.Text;
using Gpt4All.Prediction;

namespace Gpt4All;

public record TextPredictionResult : ITextPredictionResult
{
    private readonly StringBuilder _result;

    public bool Success { get; internal set; } = true;

    public string? ErrorMessage { get; internal set; }

    public PredictionInfo Usage { get; internal set; }

    internal TextPredictionResult()
    {
        _result = new StringBuilder();
        Usage = PredictionInfo.Empty;
    }

    internal void Append(string token)
    {
        _result.Append(token);
    }

    public Task<string> GetPredictionAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_result.ToString());
    }
}
