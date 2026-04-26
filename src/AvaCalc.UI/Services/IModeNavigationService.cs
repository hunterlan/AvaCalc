using System;
using System.Collections.Generic;
using AvaCalc.Core.Shared;
using AvaCalc.UI.ViewModels;

namespace AvaCalc.UI.Services;

/// <summary>
/// Manages navigation between calculator modes and exposes the currently active mode ViewModel.
/// </summary>
/// <remarks>
/// Bind <see cref="ActiveModeViewModel"/> to the main content area in the View.
/// Call <see cref="NavigateTo"/> to switch modes programmatically or in response to user input.
/// Subscribe to <see cref="ModeChanged"/> to react to mode transitions.
/// </remarks>
public interface IModeNavigationService
{
    /// <summary>
    /// Raised after the active mode has changed.
    /// The event argument is the newly active <see cref="CalculatorMode"/>.
    /// </summary>
    event EventHandler<CalculatorMode>? ModeChanged;

    /// <summary>Gets the list of all available calculator modes.</summary>
    IReadOnlyList<CalculatorMode> AvailableModes { get; }

    /// <summary>Gets the currently active calculator mode identifier.</summary>
    CalculatorMode ActiveMode { get; }

    /// <summary>Gets the ViewModel for the currently active mode.</summary>
    ViewModelBase ActiveModeViewModel { get; }

    /// <summary>
    /// Navigates to the specified calculator mode, updating <see cref="ActiveModeViewModel"/>.
    /// </summary>
    /// <param name="mode">The mode to switch to.</param>
    void NavigateTo(CalculatorMode mode);

    /// <summary>
    /// Determines whether navigation to the specified mode is currently allowed.
    /// </summary>
    /// <param name="mode">The mode to check.</param>
    /// <returns><see langword="true"/> if navigation is allowed; otherwise <see langword="false"/>.</returns>
    bool CanNavigateTo(CalculatorMode mode);
}
