using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gpt4All.Bindings;

/// <summary>
/// Arguments for the response processing callback
/// </summary>
/// <param name="TokenId">The token id of the response</param>
/// <param name="Response"> The response string. NOTE: a token_id of -1 indicates the string is an error string</param>
/// <return>
/// A bool indicating whether the model should keep generating
/// </return>
public record ModelResponseEventArgs(int TokenId, string Response)
{
    public bool IsError => TokenId == -1;
}

/// <summary>
/// Arguments for the prompt processing callback
/// </summary>
/// <param name="TokenId">The token id of the prompt</param>
/// <return>
/// A bool indicating whether the model should keep processing
/// </return>
public record ModelPromptEventArgs(int TokenId)
{
}

/// <summary>
/// Arguments for the recalculating callback
/// </summary>
/// <param name="IsRecalculating"> whether the model is recalculating the context.</param>
/// <return>
/// A bool indicating whether the model should keep generating
/// </return>
public record ModelRecalculatingEventArgs(bool IsRecalculating);

/// <summary>
/// Arguments for embedding cancellation callback
/// </summary>
/// <param name="NumTokensPerBatch"> The number of tokens in each batch that will be embedded</param>
/// <param name="NumBatches">The number of batches that will be embedded</param>
/// <param name="Backend">The backend that will be used for embedding. One of "cpu", "kompute", or "metal"</param>
public record ModelEmbedCancellationEventArgs(uint NumTokensPerBatch, uint NumBatches, string Backend);

/// <summary>
/// Base class and universal wrapper for GPT4All language models built around llmodel C-API.
/// </summary>
public class LLModel : ILLModel
{
    protected readonly IntPtr _handle;
    private readonly ILogger _logger;
    private bool _disposed;

    internal LLModel(IntPtr handle, ILogger? logger = null)
    {
        _handle = handle;
        _logger = logger ?? NullLogger.Instance;
    }

    internal IntPtr Handle => _handle;

    /// <summary>
    /// Create a new model from a pointer
    /// </summary>
    /// <param name="handle">Pointer to underlying model</param>
    public static LLModel Create(IntPtr handle, ILogger? logger = null)
    {
        return new LLModel(handle, logger: logger);
    }

    /// <inheritdoc/>
    public nuint GetRequiredMemory(string modelPath, int maxContextSize, int numGpuLayers)
    {
        return NativeMethods.llmodel_required_mem(_handle, modelPath, maxContextSize, numGpuLayers);
    }

    /// <inheritdoc/>
    public void Prompt(
        string prompt,
        string promptTemplate,
        LLModelPromptContext context,
        Func<ModelPromptEventArgs, bool>? promptCallback = null,
        Func<ModelResponseEventArgs, bool>? responseCallback = null,
        Func<ModelRecalculatingEventArgs, bool>? recalculateCallback = null,
        bool special = false,
        CancellationToken cancellationToken = default)
    {
        GC.KeepAlive(promptCallback);
        GC.KeepAlive(responseCallback);
        GC.KeepAlive(recalculateCallback);
        GC.KeepAlive(cancellationToken);

        _logger.LogInformation("Prompt input='{Prompt}' special={Special} ctx={Context}",
            prompt,
            special,
            context.Dump());

        NativeMethods.llmodel_prompt(
            _handle,
            prompt,
            promptTemplate,
            (tokenId) =>
            {
                if (cancellationToken.IsCancellationRequested) return false;
                if (promptCallback == null) return true;
                var args = new ModelPromptEventArgs(tokenId);
                return promptCallback(args);
            },
            (tokenId, response) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("ResponseCallback evt=CancellationRequested");
                    return false;
                }

                if (responseCallback == null) return true;
                var args = new ModelResponseEventArgs(tokenId, response);
                return responseCallback(args);
            },
            (isRecalculating) =>
            {
                if (cancellationToken.IsCancellationRequested) return false;
                if (recalculateCallback == null) return true;
                var args = new ModelRecalculatingEventArgs(isRecalculating);
                return recalculateCallback(args);
            },
            ref context.UnderlyingContext,
            special: special,
            fake_reply: IntPtr.Zero
        );
    }

    /// <inheritdoc/>
    /// <exception cref="EmbeddingsGenerationException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public unsafe float* Embed(
        string[] texts,
        out nuint embeddingsSize,
        out nuint tokenCount,
        int dimensionality = -1,
        string? prefix = null,
        bool atlas = false,
        bool doMean = false,
        Func<ModelEmbedCancellationEventArgs, bool>? cancellationCallback = null,
        CancellationToken cancellationToken = default)
    {
        GC.KeepAlive(cancellationToken);
        GC.KeepAlive(cancellationCallback);

        var canceled = false;

        // manually marshal the null-terminated strings array
        // NativeMethods.llmodel_embed 'texts' parameter type could also be changed to
        // string[], but in that case, there would be the necessity to add a NULL string a the end.
        // to avoid to copy the string array passed by the user (or to bother the user adding a NULL string at the end),
        // we can marshal the string array manually.
        // writing a custom null-terminated string[] marshall could also be done.
        var ptrTexts = (IntPtr*)IntPtr.Zero;

        try
        {
            ptrTexts = (IntPtr*)NativeMemory.Alloc((nuint)(sizeof(IntPtr) * (texts.Length + 1))); // +1 for null terminator

            ptrTexts[texts.Length] = IntPtr.Zero;

            for (var i = 0; i < texts.Length; i++)
            {
                ptrTexts[i] = Marshal.StringToHGlobalAnsi(texts[i]);
            }

            var embeddingsPtr = NativeMethods.llmodel_embed(
                _handle,
                ptrTexts,
                out embeddingsSize,
                prefix,
                dimensionality,
                out tokenCount,
                doMean,
                atlas,
                (batchSizes, nBatch, backend) =>
                {
                    if (cancellationToken.IsCancellationRequested) return true;
                    if (cancellationCallback == null) return false;
                    var args = new ModelEmbedCancellationEventArgs(batchSizes, nBatch, backend);
                    canceled = cancellationCallback(args);
                    return canceled;
                },
                out var error);

            if (canceled || cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("Embeddings generation was canceled");
            }

            if (embeddingsPtr == null)
            {
                var errorMessage = error != IntPtr.Zero
                    ? Marshal.PtrToStringAnsi(error)
                    : "Unknown error";

                throw new EmbeddingsGenerationException($"Failed to generate embeddings: {errorMessage}");
            }

            return embeddingsPtr;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Embedding generation failed.");
            throw new EmbeddingsGenerationException("Embedding generation failed.", ex);
        }
        finally
        {
            if (ptrTexts != (IntPtr*)IntPtr.Zero)
            {
                for (var i = 0; i < texts.Length; i++)
                {
                    Marshal.FreeHGlobal(ptrTexts[i]);
                }
                NativeMemory.Free(ptrTexts);
            }
        }
    }

    /// <inheritdoc/>
    public void SetThreadCount(int threadCount)
    {
        NativeMethods.llmodel_setThreadCount(_handle, threadCount);
    }

    /// <inheritdoc/>
    public int GetThreadCount()
    {
        return NativeMethods.llmodel_threadCount(_handle);
    }

    /// <inheritdoc/>
    public ulong GetStateSizeBytes()
    {
        return NativeMethods.llmodel_get_state_size(_handle);
    }

    /// <inheritdoc/>
    public unsafe ulong SaveStateData(byte* source)
    {
        return NativeMethods.llmodel_save_state_data(_handle, source);
    }

    /// <inheritdoc/>
    public unsafe ulong RestoreStateData(byte* destination)
    {
        return NativeMethods.llmodel_restore_state_data(_handle, destination);
    }

    /// <inheritdoc/>
    public bool IsLoaded()
    {
        return NativeMethods.llmodel_isModelLoaded(_handle);
    }

    /// <inheritdoc/>
    public bool Load(string modelPath, int maxContextSize = 2048, int numGpuLayers = 100)
    {
        return NativeMethods.llmodel_loadModel(_handle, modelPath, maxContextSize, numGpuLayers);
    }

    /// <inheritdoc/>
    public string GetDeviceName()
    {
        var ptr = NativeMethods.llmodel_model_gpu_device_name(_handle);
        return Marshal.PtrToStringAnsi(ptr) ?? string.Empty;
    }

    /// <inheritdoc/>
    public string GetBackendName()
    {
        var ptr = NativeMethods.llmodel_model_backend_name(_handle);
        return Marshal.PtrToStringAnsi(ptr) ?? string.Empty;
    }

    /// <inheritdoc/>
    public bool HasGpuDevice()
    {
        return NativeMethods.llmodel_has_gpu_device(_handle);
    }

    /// <inheritdoc/>
    protected void Destroy()
    {
        NativeMethods.llmodel_model_destroy(_handle);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // dispose managed state
        }

        Destroy();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
