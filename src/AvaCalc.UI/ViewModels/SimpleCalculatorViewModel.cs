using System;
using AvaCalc.Core.Commands;
using AvaCalc.Core.Modes;
using AvaCalc.Core.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaCalc.UI.ViewModels;

/// <summary>
/// ViewModel for the Simple calculator mode.
/// Binds the display and all button commands to an <see cref="ICalculatorMode"/> back-end.
/// </summary>
public sealed partial class SimpleCalculatorViewModel : ViewModelBase
{
    private readonly ICalculatorMode _mode;

    [ObservableProperty]
    private string _displayValue = "0";

    /// <summary>
    /// Initialises the ViewModel with the given calculator mode.
    /// </summary>
    /// <param name="mode">The calculator mode that processes all user input.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mode"/> is null.</exception>
    public SimpleCalculatorViewModel(ICalculatorMode mode)
    {
        ArgumentNullException.ThrowIfNull(mode, nameof(mode));
        _mode = mode;
    }

    [RelayCommand]
    private void Digit(string digit) => Apply(new AppendDigitCommand(digit[0]));

    [RelayCommand]
    private void DecimalPoint() => Apply(new AppendDecimalPointCommand());

    [RelayCommand]
    private void Operator(CalculatorOperator op) => Apply(new ApplyOperatorCommand(op));

    [RelayCommand]
    private void Equals() => Apply(new EvaluateCommand());

    [RelayCommand]
    private void Clear() => Apply(new ClearCommand());

    [RelayCommand]
    private void AllClear() => Apply(new AllClearCommand());

    [RelayCommand]
    private void Backspace() => Apply(new BackspaceCommand());

    [RelayCommand]
    private void SignToggle() => Apply(new SignToggleCommand());

    [RelayCommand]
    private void Percent() => Apply(new PercentCommand());

    [RelayCommand]
    private void SquareRoot() => Apply(new SquareRootCommand());

    private void Apply(ICalculatorCommand command)
    {
        var result = _mode.Execute(command);
        DisplayValue = result.DisplayString;
    }
}
