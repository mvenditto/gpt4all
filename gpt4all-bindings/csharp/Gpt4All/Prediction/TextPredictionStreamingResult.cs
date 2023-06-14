using System.Text;
using System.Threading.Channels;

namespace Gpt4All;

/// <inheritdoc/>
public record TextPredictionStreamingResult : ITextPredictionStreamingResult
{
    private readonly Channel<string> _channel;

    /// <summary>
    /// true if the generation request is successfull, false otherwise
    /// </summary>
    public bool Success { get; internal set; } = true;

    /// <summary>
    /// The error surfaced by the model, if any
    /// </summary>
    public string? ErrorMessage { get; internal set; }

    /// <summary>
    /// Gets a Task that completes when there are no more tokens incoming
    /// </summary>
    public Task Completion => _channel.Reader.Completion;

    internal TextPredictionStreamingResult()
    {
        _channel = Channel.CreateUnbounded<string>();
    }

    internal bool Append(string token)
    {
        return _channel.Writer.TryWrite(token);
    }

    internal void Complete()
    {
        _channel.Writer.Complete();
    }

    /// <inheritdoc/>
    public async Task<string> GetPredictionAsync(CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();

        var tokens = GetPredictionStreamingAsync(cancellationToken).ConfigureAwait(false);

        await foreach (var token in tokens)
        {
            sb.Append(token);
        }

        return sb.ToString();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<string> GetPredictionStreamingAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
