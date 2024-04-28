namespace Gpt4All;

public interface IGpt4AllModel : ITextPrediction, IDisposable
{
    /// <summary>
    /// The prompt formatter used to format the prompt before
    /// feeding it to the model, if null no transformation is applied
    /// </summary>
    [Obsolete("Use PredictRequestOptions.PromptTemplate instead")]
    IPromptFormatter? PromptFormatter { get; set; }

    /// <summary>
    /// The name of the backend the model is running on
    /// </summary>
    string BackendName { get; }

    /// <summary>
    /// The name of the device the model is running on
    /// </summary>
    string DeviceName { get; }

    /// <summary>
    /// Gets whether the model is running on a GPU
    /// </summary>
    bool HasGpuDevice { get; }
}
