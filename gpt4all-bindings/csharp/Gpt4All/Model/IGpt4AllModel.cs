using Gpt4All.Bindings;
using Gpt4All.Embedding;

namespace Gpt4All;

public interface IGpt4AllModel : ITextPrediction, ITextEmbedding, IDisposable
{
    /// <summary>
    /// Gets an estimate RAM requirement for a model file
    /// </summary>
    /// <returns>The estimated RAM requirement for the model</returns>
    /// <exception cref="ModelLoadException">If the model file cannot be parsed</exception>
    nuint GetRequiredMemory();

    /// <summary>
    /// The prompt formatter used to format the prompt before
    /// feeding it to the model, if null no transformation is applied
    /// </summary>
    IPromptFormatter? PromptFormatter { get; set; }

    /// <summary>
    /// The model context
    /// </summary>
    LLModelPromptContext Context { get; set; }
}
