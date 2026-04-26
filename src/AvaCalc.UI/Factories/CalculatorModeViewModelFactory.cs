using System;
using System.Collections.Generic;
using AvaCalc.Core.Shared;
using AvaCalc.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaCalc.UI.Factories;

/// <summary>
/// Resolves calculator mode ViewModels from the DI container.
/// </summary>
/// <remarks>
/// Mode ViewModels must be registered in the DI container before this factory can resolve them.
/// The <see cref="AvailableModes"/> list is populated at construction time from <paramref name="modeViewModelTypes"/>.
/// </remarks>
public sealed class CalculatorModeViewModelFactory : ICalculatorModeViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<CalculatorMode, Type> _modeViewModelTypes;

    /// <inheritdoc/>
    public IReadOnlyList<CalculatorMode> AvailableModes { get; }

    /// <summary>
    /// Initialises the factory with the DI service provider and a mapping of modes to ViewModel types.
    /// </summary>
    /// <param name="serviceProvider">The application DI service provider.</param>
    /// <param name="modeViewModelTypes">Mapping from <see cref="CalculatorMode"/> to the concrete ViewModel type.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public CalculatorModeViewModelFactory(
        IServiceProvider serviceProvider,
        Dictionary<CalculatorMode, Type> modeViewModelTypes)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));
        ArgumentNullException.ThrowIfNull(modeViewModelTypes, nameof(modeViewModelTypes));

        _serviceProvider = serviceProvider;
        _modeViewModelTypes = modeViewModelTypes;
        AvailableModes = new List<CalculatorMode>(_modeViewModelTypes.Keys);
    }

    /// <inheritdoc/>
    public ViewModelBase Create(CalculatorMode mode)
    {
        if (!_modeViewModelTypes.TryGetValue(mode, out var viewModelType))
            throw new InvalidOperationException($"No ViewModel registered for mode '{mode}'.");

        return (ViewModelBase)_serviceProvider.GetRequiredService(viewModelType);
    }
}
