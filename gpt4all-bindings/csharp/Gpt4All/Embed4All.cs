using Gpt4All.Bindings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gpt4All;

public class Embed4All : Gpt4AllModelBase, IGpt4AllEmbeddingModel
{
    private readonly ILogger<Embed4All> _logger;

    public Embed4All(ILLModel model, ILogger<Embed4All>? logger = null) : base(model, logger)
    {
        _logger = logger ?? NullLogger<Embed4All>.Instance;
    }

    /// <inheritdoc/>
    /// <exception cref="EmbeddingsGenerationException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public Task<IEmbedResult> GetEmbeddingsAsync(
        string[] texts,
        EmbedRequestOptions opts,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            unsafe
            {
                var embeddingsPtr = _model.Embed(
                    texts,
                    out var totalValues,
                    out var tokenCount,
                    dimensionality: opts.Dimensionality,
                    prefix: opts.Prefix,
                    atlas: opts.EnforceAtlasApiCompatibility,
                    doMean: opts.DoMean,
                    cancellationCallback: null,
                    cancellationToken: cancellationToken
                );

                var result = new EmbedResult(
                    embeddingsPtr: embeddingsPtr,
                    numSegments: texts.Length,
                    embeddingSize: (int)totalValues);

                return (IEmbedResult)result;
            }
        }, cancellationToken);
    }
}
