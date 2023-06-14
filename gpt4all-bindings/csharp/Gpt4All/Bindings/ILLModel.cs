namespace Gpt4All.Bindings;

/// <summary>
/// Represents the interface exposed by the universal wrapper for GPT4All language models built around llmodel C-API.
/// </summary>
public interface ILLModel : IDisposable
{
    /// <summary>
    /// this model type
    /// </summary>
    ModelType ModelType { get; }

    /// <summary>
    /// Gets the size of the model state
    /// </summary>
    /// <returns>the size of the model state in bytes</returns>
    ulong GetStateSizeBytes();

    /// <summary>
    /// Get the read count the model is using
    /// </summary>
    /// <returns>the threads count</returns>
    int GetThreadCount();

    /// <summary>
    /// Set the thread count the model will use
    /// </summary>
    /// <param name="threadCount">The thread count to set</param>
    void SetThreadCount(int threadCount);

    /// <summary>
    /// Check if the model is loaded
    /// </summary>
    /// <returns>true if the model is loaded, false othrwise</returns>
    bool IsLoaded();

    /// <summary>
    /// Load the model
    /// </summary>
    /// <param name="modelPath">The path to the model file to load</param>
    /// <returns>true if the model loaded successfully, false otherwise</returns>
    bool Load(string modelPath);

    /// <summary>
    /// Request the model for a text generation
    /// </summary>
    /// <param name="text">The prompt</param>
    /// <param name="context">The prompt context</param>
    /// <param name="promptCallback">The prompt callback</param>
    /// <param name="responseCallback">The response callback</param>
    /// <param name="recalculateCallback">The recalculate callback</param>
    /// <param name="cancellationToken">The cancellation token to stop the generation</param>
    void Prompt(
        string text,
        LLModelPromptContext context,
        Func<ModelPromptEventArgs, bool>? promptCallback = null,
        Func<ModelResponseEventArgs, bool>? responseCallback = null,
        Func<ModelRecalculatingEventArgs, bool>? recalculateCallback = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Restore the state of the model
    /// </summary>
    /// <param name="source">The source buffer</param>
    /// <returns></returns>
    unsafe ulong RestoreStateData(byte* source);

    /// <summary>
    /// Save the current model state
    /// </summary>
    /// <param name="destination">The destination buffer</param>
    /// <returns></returns>
    unsafe ulong SaveStateData(byte* destination);
}
