
namespace Gpt4All;

/// <summary>
/// Exposes functionality common to both embedding and text prediction models
/// </summary>
public interface IModel
{
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
