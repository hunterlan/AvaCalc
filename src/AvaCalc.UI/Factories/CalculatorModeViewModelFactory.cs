using System;
using System.Collections.Generic;
using AvaCalc.Core.Shared;
using AvaCalc.UI.ViewModels;

namespace AvaCalc.UI.Factories;

/// <summary>
/// Resolves calculator mode ViewModels via pre-registered delegate factories.
/// </summary>
/// <remarks>
/// Each mode is mapped to a <see cref="Func{ViewModelBase}"/> delegate at composition root,
/// avoiding runtime <c>Type</c>-based DI resolution and making this class fully
/// Native AOT and trim-safe.
/// Add a new entry in <see cref="App"/> for every new mode/ViewModel pair.
/// </remarks>
public sealed class CalculatorModeViewModelFactory : ICalculatorModeViewModelFactory
{
    private readonly Dictionary<CalculatorMode, Func<ViewModelBase>> _modeFactories;

    /// <inheritdoc/>
    public IReadOnlyList<CalculatorMode> AvailableModes { get; }

    /// <summary>
    /// Initialises the factory with a mapping of modes to ViewModel factory delegates.
    /// </summary>
    /// <param name="modeFactories">
    /// Mapping from <see cref="CalculatorMode"/> to a factory delegate that creates
    /// the corresponding <see cref="ViewModelBase"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="modeFactories"/> is null.</exception>
    public CalculatorModeViewModelFactory(Dictionary<CalculatorMode, Func<ViewModelBase>> modeFactories)
    {
        ArgumentNullException.ThrowIfNull(modeFactories, nameof(modeFactories));
        _modeFactories = modeFactories;
        AvailableModes = new List<CalculatorMode>(_modeFactories.Keys);
    }

    /// <inheritdoc/>
    public ViewModelBase Create(CalculatorMode mode)
    {
        if (!_modeFactories.TryGetValue(mode, out var factory))
            throw new InvalidOperationException($"No ViewModel registered for mode '{mode}'.");

        return factory();
    }
}

