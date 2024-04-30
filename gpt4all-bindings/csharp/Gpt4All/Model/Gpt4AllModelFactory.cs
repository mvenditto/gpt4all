using System.Reflection;
using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Gpt4All.Bindings;
using Gpt4All.LibraryLoader;

namespace Gpt4All;

public class Gpt4AllModelFactory : IGpt4AllModelFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;
    private static bool bypassLoading;
    private static string? libraryPath;

    private static readonly Lazy<LoadResult> libraryLoaded = new(() =>
    {
        return NativeLibraryLoader.LoadNativeLibrary(Gpt4AllModelFactory.libraryPath, Gpt4AllModelFactory.bypassLoading);
    }, true);

    /// <summary>
    /// Create a models factory.
    /// </summary>
    /// <param name="libraryPath">Set a specific path where the libllmodel library is located.</param>
    /// <param name="bypassLoading">If true, the libllmodel library loading is left to the runtime or the user</param>
    /// <param name="autoSetImplementatioSearchPath">if true, the impl. search path will be set where the executing assembly is located.</param>
    /// <param name="loggerFactory">a logger factory</param>
    public Gpt4AllModelFactory(string? libraryPath = default, bool bypassLoading = true, bool autoSetImplementatioSearchPath = false, ILoggerFactory? loggerFactory = null)
    {
        _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
        _logger = _loggerFactory.CreateLogger<Gpt4AllModelFactory>();
        Gpt4AllModelFactory.libraryPath = libraryPath;
        Gpt4AllModelFactory.bypassLoading = bypassLoading;

        if (!libraryLoaded.Value.IsSuccess)
        {
            throw new Exception($"Failed to load native gpt4all library. Error: {libraryLoaded.Value.ErrorMessage}");
        }

        if (autoSetImplementatioSearchPath)
        {
            try
            {
                var assemblyPath = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location);

                if (!string.IsNullOrEmpty(assemblyPath))
                {
                    Helpers.SetImplementationSearchPath(assemblyPath);
                    _logger.LogDebug("Set implementation search path to: {ProcessPath}", assemblyPath);
                }
                else
                {
                    _logger.LogWarning("Unable to determine the assembly path. Implementation search path not set.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set the implementation search path.");
            }
        }
    }

    private void InitGpuDevice(LLModel model, string modelPath, string device = "cpu", int maxContextSize = 2048, int numGpuLayers = 100)
    {
        ArgumentNullException.ThrowIfNull(model);

        var requiredMemory = model.GetRequiredMemory(modelPath, maxContextSize, numGpuLayers);

        _logger.LogDebug("Initializing GPU device device={Device} requiredMemory={RequiredMemory}", device, requiredMemory);

        if (NativeMethods.llmodel_gpu_init_gpu_device_by_string(model.Handle, requiredMemory, device))
        {
            _logger.LogDebug("GPU '{Device}' initialized succesfully.", device);
            return;
        }

        var allGpus = Helpers.GetAvailableGpuDevices().Select(x => x.Name);
        var availableGpus = Helpers.GetAvailableGpuDevices(requiredMemory).Select(x => x.Name);
        var unavailableGpus = allGpus.Except(availableGpus);

        var errMessage = $"Unable to initialize model on GPU: {device}";
        errMessage += $"\nAvailable GPUs: {availableGpus.Join()}";
        errMessage += $"\nUnavailable GPUs due to insufficient memory or features: {unavailableGpus.Join()}";

        throw new GpuDeviceInitializationException(errMessage);
    }

    private Gpt4All CreateModel(string modelPath, string device, int maxContextSize, int numGpuLayers)
    {
        if (!File.Exists(modelPath))
        {
            throw new ModelLoadException(
                "Model file not found",
                innerException: new FileNotFoundException("Model file not found", modelPath));
        }

        _logger.LogInformation("Creating model path={ModelPath}", modelPath);

        IntPtr error;

        var handle = NativeMethods.llmodel_model_create2(modelPath, "auto", out error);

        if (error != IntPtr.Zero)
        {
            throw new Exception(Marshal.PtrToStringAnsi(error));
        }

        var logger = _loggerFactory.CreateLogger<LLModel>();

        var underlyingModel = LLModel.Create(handle, logger: logger);

        _logger.LogDebug("Model created handle=0x{ModelHandle:X8}", handle);

        _logger.LogInformation("Model loading started");

        if (device != null && device != "cpu")
        {
            InitGpuDevice(underlyingModel, modelPath, device, maxContextSize, numGpuLayers);
        }

        var loadedSuccessfully = underlyingModel.Load(modelPath, maxContextSize, numGpuLayers);

        _logger.LogInformation("Model loading completed success={ModelLoadSuccess}", loadedSuccessfully);

        if (!loadedSuccessfully)
        {
            try
            {
                underlyingModel.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to dispose the model on load failure.");
            }

            throw new ModelLoadException($"Failed to load model: '{modelPath}'");
        }

        Debug.Assert(underlyingModel.IsLoaded());

        return new Gpt4All(underlyingModel, logger: logger);
    }

    public IGpt4AllModel LoadModel(string modelPath, string device = "cpu", int maxContextSize = 2048, int numGpuLayers = 100) =>
            CreateModel(modelPath, device, maxContextSize, numGpuLayers);
}
