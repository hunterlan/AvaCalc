using System.Collections.Generic;
using AvaCalc.Core.Shared;
using AvaCalc.UI.ViewModels;

namespace AvaCalc.UI.Factories;

/// <summary>
/// Creates and resolves <see cref="ViewModelBase"/> instances for each calculator mode.
/// </summary>
/// <remarks>
/// Register mode ViewModels with the DI container and inject this factory into
/// <see cref="AvaCalc.UI.Services.IModeNavigationService"/> to enable mode switching.
/// </remarks>
public interface ICalculatorModeViewModelFactory
{
    /// <summary>Gets the list of modes that have a registered ViewModel.</summary>
    IReadOnlyList<CalculatorMode> AvailableModes { get; }

    /// <summary>
    /// Creates or resolves the <see cref="ViewModelBase"/> for the given <paramref name="mode"/>.
    /// </summary>
    /// <param name="mode">The calculator mode to create a ViewModel for.</param>
    /// <returns>The ViewModel instance for the specified mode.</returns>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when no ViewModel is registered for <paramref name="mode"/>.
    /// </exception>
    ViewModelBase Create(CalculatorMode mode);
}
