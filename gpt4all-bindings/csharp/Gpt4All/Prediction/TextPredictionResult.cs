using System.Text;

namespace Gpt4All;

/// <inheritdoc/>
public record TextPredictionResult : ITextPredictionResult
{
    private readonly StringBuilder _result;

    /// <summary>
    /// true if the generation request is successfull, false otherwise
    /// </summary>
    public bool Success { get; internal set; } = true;

    /// <summary>
    /// The error surfaced by the model, if present
    /// </summary>
    public string? ErrorMessage { get; internal set; }

    internal TextPredictionResult()
    {
        _result = new StringBuilder();
    }

    internal void Append(string token)
    {
        _result.Append(token);
    }

    /// <inheritdoc/>
    public Task<string> GetPredictionAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_result.ToString());
    }
}
