// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using HyperVExtension.Common.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Windows.DevHome.SDK;

namespace HyperVExtension;

[ComVisible(true)]
[Guid("F8B26528-976A-488C-9B40-7198FB425C9E")]
[ComDefaultInterface(typeof(IExtension))]
public sealed class HyperVExtension : IExtension, IDisposable
{
    private readonly IHost _host;
    private bool _disposed;

    public HyperVExtension(IHost host)
    {
        _host = host;
    }

    /// <summary>
    /// Gets the synchronization object that is used to prevent the main program from exiting
    /// until the extension is disposed.
    /// </summary>
    public ManualResetEvent ExtensionDisposedEvent { get; } = new(false);

    /// <summary>
    /// Gets provider object for the specified provider type.
    /// </summary>
    /// <param name="providerType">
    /// The provider type that the Hyper-V extension may support. This is used to query the Hyper-V
    /// extension for whether it supports the provider type.
    /// </param>
    /// <returns>
    /// When the extension supports the ProviderType the object returned will not be null. However,
    /// when the extension does not support the ProviderType the returned object will be null.
    /// </returns>
    public object? GetProvider(ProviderType providerType)
    {
        object? provider = null;
        try
        {
            switch (providerType)
            {
                case ProviderType.ComputeSystem:
                    provider = _host.GetService<IComputeSystemProvider>();
                    break;
                default:
                    Providers.Logging.Logger()?.ReportInfo($"Unsupported provider: {providerType}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Providers.Logging.Logger()?.ReportError($"Failed to get provider for provider type {providerType}", ex);
        }

        return provider;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                ExtensionDisposedEvent.Set();
            }

            _disposed = true;
        }
    }
}
