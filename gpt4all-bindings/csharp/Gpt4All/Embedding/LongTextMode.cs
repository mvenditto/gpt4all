namespace Gpt4All;

/// <summary>
/// Modes to instruct the model how to handle texts longer than it can accept
/// </summary>
public enum LongTextMode
{
    /// <summary>
    /// Average multiple embeddings
    /// </summary>
    Mean = 1,
    /// <summary>
    /// Truncate
    /// </summary>
    Truncate = 2
}
