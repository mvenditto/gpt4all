using Gpt4All.Bindings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gpt4All;

public abstract class Gpt4AllModelBase : IModel, IDisposable
{
    protected readonly ILLModel _model;
    private readonly Lazy<string> _deviceName;
    private readonly Lazy<string> _backendName;
    private readonly Lazy<bool> _hasGpuDevice;
    private readonly ILogger _logger;

    protected Gpt4AllModelBase(ILLModel model, ILogger? logger = null)
    {
        _model = model;
        _deviceName = new(model.GetDeviceName);
        _backendName = new(model.GetBackendName);
        _hasGpuDevice = new(model.HasGpuDevice);
        _logger = logger ?? NullLogger.Instance;
    }

    /// <inheritdoc/>
    public string BackendName => _backendName.Value;

    /// <inheritdoc/>
    public string DeviceName => _deviceName.Value;

    /// <inheritdoc/>
    public bool HasGpuDevice => _hasGpuDevice.Value;

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _model.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
