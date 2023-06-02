using Gpt4All.Bindings;
using Gpt4All.Prediction;

namespace Gpt4All;

public static class PromptResultExtensions
{
    public static PredictionInfo ToPredictionInfo(this PromptResult promptResult)
    {
        return new PredictionInfo
        {
            PromptTokens = promptResult.PromptTokens,
            CompletionTokens = promptResult.CompletionTokens,
        };
    }
}
