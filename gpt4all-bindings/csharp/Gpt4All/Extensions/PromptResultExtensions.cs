using Gpt4All.Bindings;

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
