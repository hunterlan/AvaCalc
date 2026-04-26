using System;
using System.Collections.Generic;
using AvaCalc.Core.Shared;
using AvaCalc.UI.Services;
using CommunityToolkit.Mvvm.Input;

namespace AvaCalc.UI.ViewModels;

/// <summary>
/// ViewModel for the main application window.
/// Delegates mode switching to <see cref="IModeNavigationService"/> and exposes
/// only the data the View needs: the active mode ViewModel and the list of available modes.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IModeNavigationService _navigationService;

    /// <summary>Gets the ViewModel for the currently displayed calculator mode.</summary>
    public ViewModelBase ActiveModeViewModel => _navigationService.ActiveModeViewModel;

    /// <summary>Gets the list of all available calculator modes for mode-selector UI.</summary>
    public IReadOnlyList<CalculatorMode> AvailableModes => _navigationService.AvailableModes;

    /// <summary>
    /// Initialises the ViewModel with the navigation service.
    /// </summary>
    /// <param name="navigationService">The service responsible for mode navigation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="navigationService"/> is null.</exception>
    public MainWindowViewModel(IModeNavigationService navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        _navigationService = navigationService;

        // Forward ModeChanged from the navigation service so the View re-renders and
        // command CanExecute re-evaluates on mode switch
        _navigationService.ModeChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(ActiveModeViewModel));
            SelectModeCommand.NotifyCanExecuteChanged();
        };
    }

    /// <summary>
    /// Command to switch the active calculator mode.
    /// </summary>
    /// <param name="mode">The mode to navigate to.</param>
    [RelayCommand(CanExecute = nameof(CanSelectMode))]
    private void SelectMode(CalculatorMode mode) => _navigationService.NavigateTo(mode);

    private bool CanSelectMode(CalculatorMode mode) => _navigationService.CanNavigateTo(mode);
}