using System;
using System.Collections.Generic;
using System.Linq;
using AvaCalc.Core.Shared;
using AvaCalc.UI.Factories;
using AvaCalc.UI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaCalc.UI.Services;

/// <summary>
/// Default implementation of <see cref="IModeNavigationService"/>.
/// Extends <see cref="ObservableObject"/> so that <see cref="ActiveModeViewModel"/>
/// and <see cref="ActiveMode"/> trigger Avalonia data binding updates when the mode changes.
/// </summary>
public sealed class ModeNavigationService : ObservableObject, IModeNavigationService
{
    private readonly ICalculatorModeViewModelFactory _factory;
    private CalculatorMode _activeMode;
    private ViewModelBase _activeModeViewModel = null!;

    /// <inheritdoc/>
    public event EventHandler<CalculatorMode>? ModeChanged;

    /// <inheritdoc/>
    public IReadOnlyList<CalculatorMode> AvailableModes => _factory.AvailableModes;

    /// <inheritdoc/>
    public CalculatorMode ActiveMode
    {
        get => _activeMode;
        private set => SetProperty(ref _activeMode, value);
    }

    /// <inheritdoc/>
    public ViewModelBase ActiveModeViewModel
    {
        get => _activeModeViewModel;
        private set => SetProperty(ref _activeModeViewModel, value);
    }

    /// <summary>
    /// Initialises the service and navigates to <paramref name="initialMode"/>.
    /// </summary>
    /// <param name="factory">The factory used to create mode ViewModels.</param>
    /// <param name="initialMode">The mode to display on startup.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is null.</exception>
    public ModeNavigationService(ICalculatorModeViewModelFactory factory, CalculatorMode initialMode = CalculatorMode.Simple)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));
        _factory = factory;
        _activeMode = initialMode;

        if (_factory.AvailableModes.Count > 0)
            NavigateTo(initialMode);
    }

    /// <inheritdoc/>
    public void NavigateTo(CalculatorMode mode)
    {
        if (!CanNavigateTo(mode))
            return;

        ActiveModeViewModel = _factory.Create(mode);
        ActiveMode = mode;
        ModeChanged?.Invoke(this, mode);
    }

    /// <inheritdoc/>
    public bool CanNavigateTo(CalculatorMode mode) =>
        _factory.AvailableModes.Contains(mode) && (_activeModeViewModel is null || mode != _activeMode);
}
