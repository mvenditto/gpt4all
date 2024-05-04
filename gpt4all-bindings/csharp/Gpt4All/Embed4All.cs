using Gpt4All.Bindings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gpt4All;

public class Embed4All : Gpt4AllModelBase, IGpt4AllEmbeddingModel
{
    private readonly ILogger<Embed4All> _logger;

    private const int MinDimensionality = 64;

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
        var dimensionality = opts.Dimensionality ?? -1;

        if (opts.Dimensionality.HasValue)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(
                dimensionality, 0,
                nameof(opts.Dimensionality));
        }

        if (dimensionality < MinDimensionality)
        {
            _logger.LogWarning(
                "Dimensionality {Dimensionality} is less than the suggested minimum of {MinDimensionality},"
                + "Performance may be degraded.",
                dimensionality,
                MinDimensionality);
        }

        var mean = opts.LongTextMode == LongTextMode.Mean;

        return Task.Run(() =>
        {
            unsafe
            {
                var embeddingsPtr = _model.Embed(
                    texts,
                    out var totalValues,
                    out var tokenCount,
                    dimensionality: dimensionality,
                    prefix: opts.Prefix,
                    atlas: opts.EnforceAtlasApiCompatibility,
                    doMean: mean,
                    cancellationCallback: null,
                    cancellationToken: cancellationToken
                );

                var result = new EmbedResult(
                    embeddingsPtr: embeddingsPtr,
                    numSegments: texts.Length,
                    embeddingSize: (int)totalValues,
                    tokenCount: (int?)tokenCount);

                return (IEmbedResult)result;
            }
        }, cancellationToken);
    }
}
