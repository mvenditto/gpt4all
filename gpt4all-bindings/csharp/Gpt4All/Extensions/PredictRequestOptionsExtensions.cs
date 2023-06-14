using Gpt4All.Bindings;

namespace Gpt4All;

/// <summary>
/// <see cref="PredictRequestOptions"/> extension methods
/// </summary>
public static class PredictRequestOptionsExtensions
{
    /// <summary>
    /// Extension method to convert a <see cref="PredictRequestOptions"/> to an <see cref="LLModelPromptContext"/>
    /// </summary>
    /// <param name="opts">the input <see cref="PredictRequestOptions"/></param>
    /// <returns>an <see cref="LLModelPromptContext"/> parametrized from the input <see cref="PredictRequestOptions"/></returns>
    public static LLModelPromptContext ToPromptContext(this PredictRequestOptions opts)
    {
        return new LLModelPromptContext
        {
            LogitsSize = opts.LogitsSize,
            TokensSize = opts.TokensSize,
            TopK = opts.TopK,
            TopP = opts.TopP,
            PastNum = opts.PastConversationTokensNum,
            RepeatPenalty = opts.RepeatPenalty,
            Temperature = opts.Temperature,
            RepeatLastN = opts.RepeatLastN,
            Batches = opts.Batches,
            ContextErase = opts.ContextErase,
            ContextSize = opts.ContextSize,
            TokensToPredict = opts.TokensToPredict
        };
    }
}
