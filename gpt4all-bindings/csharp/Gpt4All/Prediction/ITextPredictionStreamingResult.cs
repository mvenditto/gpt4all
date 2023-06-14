namespace Gpt4All;

/// <summary>
/// Represents the result of a streaming text prediction request
/// </summary>
public interface ITextPredictionStreamingResult : ITextPredictionResult
{
    /// <summary>
    /// Gets an async enumerable of the tokens produced by the generatio
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to stop the generation</param>
    /// <returns>The tokens async enumerable </returns>
    IAsyncEnumerable<string> GetPredictionStreamingAsync(CancellationToken cancellationToken = default);
}
