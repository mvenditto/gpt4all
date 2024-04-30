namespace Gpt4All.Bindings;

/// <summary>
/// Represents the interface exposed by the universal wrapper for GPT4All language models built around llmodel C-API.
/// </summary>
public interface ILLModel : IDisposable
{
    /// <summary>
    /// Get the size of the internal state of the model.
    /// </summary>
    /// <remarks>
    /// This state data is specific to the type of model you have created.
    /// </remarks>
    /// <returns>the size in bytes of the internal state of the model</returns>
    ulong GetStateSizeBytes();

    /// <summary>
    /// Get  the number of threads used by the model.
    /// </summary>
    /// <returns>the number of threads used by the model</returns>
    int GetThreadCount();

    /// <summary>
    /// Set the number of threads to be used by the model.
    /// </summary>
    /// <param name="threadCount">The new thread count</param>
    void SetThreadCount(int threadCount);

    /// <summary>
    /// Check if the model is loaded.
    /// </summary>
    /// <returns>true if the model was loaded successfully, false otherwise.</returns>
    bool IsLoaded();

    /// <summary>
    /// Load the model from a file.
    /// </summary>
    /// <param name="modelPath">The path to the model file.</param>
    /// <param name="maxContextSize">Maximum size of context window</param>
    /// <param name="numGpuLayers">Number of GPU layers to use (Vulkan)</param>
    /// <returns>true if the model was loaded successfully, false otherwise.</returns>
    bool Load(string modelPath, int maxContextSize, int numGpuLayers);

    /// <summary>
    /// Generate a response using the model
    /// </summary>
    /// <param name="prompt">The input promp</param>
    /// <param name="promptTemplate">The promp template</param>
    /// <param name="context">The context</param>
    /// <param name="promptCallback">A callback function for handling the processing of prompt</param>
    /// <param name="responseCallback">A callback function for handling the generated response</param>
    /// <param name="recalculateCallback">A callback function for handling recalculation requests</param>
    /// <param name="cancellationToken"></param>
    void Prompt(
        string prompt,
        string promptTemplate,
        LLModelPromptContext context,
        Func<ModelPromptEventArgs, bool>? promptCallback = null,
        Func<ModelResponseEventArgs, bool>? responseCallback = null,
        Func<ModelRecalculatingEventArgs, bool>? recalculateCallback = null,
        bool special = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate an embedding using the model
    /// </summary>
    /// <returns>A pointer to an array of floating point values passed to the calling method which then will
    /// be responsible for lifetime of this memory.NULL if an error occurred.
    /// </returns>
    public unsafe float* Embed(
        ReadOnlyMemory<string?> texts,
        out nuint embeddingsSize,
        out nuint tokenCount,
        int dimensionality = -1,
        string? prefix = null,
        bool atlas = false,
        bool doMean = false,
        Func<ModelEmbedCancellationEventArgs, bool>? cancellationCallback = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores the internal state of the model using data from the specified address.
    /// </summary>
    /// <param name="destination">A pointer to destination</param>
    /// <returns>the number of bytes read</returns>
    unsafe ulong RestoreStateData(byte* destination);

    /// <summary>
    /// Saves the internal state of the model to the specified destination address.
    /// </summary>
    /// <param name="source">A pointer to the src</param>
    /// <returns>The number of bytes copied</returns>
    unsafe ulong SaveStateData(byte* source);

    /// <summary>
    /// Estimated RAM requirement for a model file
    /// </summary>
    /// <param name="modelPath">The path to the model file</param>
    /// <param name="maxContextSize">Maximum size of context window</param>
    /// <param name="numGpuLayers">Number of GPU layers to use (Vulkan)</param>
    /// <returns>The estimated RAM requirement for a model file</returns>
    nuint GetRequiredMemory(string modelPath, int maxContextSize, int numGpuLayers);

    /// <summary>
    /// Get the name of the backend used by the model
    /// </summary>
    string GetBackendName();

    /// <summary>
    /// Get the name of the device used by the model
    /// </summary>
    string GetDeviceName();

    /// <summary>
    /// Get whatever the model is running on a GPU or not
    /// </summary>
    bool HasGpuDevice();
}
