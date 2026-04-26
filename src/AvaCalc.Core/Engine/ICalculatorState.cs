using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Engine;

/// <summary>
/// Defines the behaviour of the calculator in a specific input state.
/// Implementations handle user input differently depending on the current
/// phase of data entry (e.g., awaiting first operand vs. entering second operand).
/// </summary>
/// <remarks>
/// Each calculator mode defines its own concrete state classes.
/// States interact with <see cref="CalculatorContext"/> to read/write shared
/// data and to trigger transitions via <see cref="CalculatorContext.TransitionTo"/>.
/// </remarks>
public interface ICalculatorState
{
    /// <summary>Handles a digit key press (0–9).</summary>
    /// <param name="context">The shared calculator context.</param>
    /// <param name="digit">The digit character pressed.</param>
    void HandleDigit(CalculatorContext context, char digit);

    /// <summary>Handles the decimal point (.) key press.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandleDecimalPoint(CalculatorContext context);

    /// <summary>Handles a binary operator key press (+, −, ×, ÷).</summary>
    /// <param name="context">The shared calculator context.</param>
    /// <param name="op">The operator selected by the user.</param>
    void HandleOperator(CalculatorContext context, CalculatorOperator op);

    /// <summary>Handles the equals (=) key press — triggers evaluation.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandleEquals(CalculatorContext context);

    /// <summary>Handles the C (Clear) key press — clears the current entry.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandleClear(CalculatorContext context);

    /// <summary>Handles the AC (All Clear) key press — resets all state.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandleAllClear(CalculatorContext context);

    /// <summary>Handles the backspace key press — removes the last entered digit.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandleBackspace(CalculatorContext context);

    /// <summary>Handles the ± (sign toggle) key press — negates the current value.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandleSignToggle(CalculatorContext context);

    /// <summary>Handles the % key press — converts the current value to a percentage.</summary>
    /// <param name="context">The shared calculator context.</param>
    void HandlePercent(CalculatorContext context);

    /// <summary>
    /// Returns the string to show on the calculator display for the current state.
    /// </summary>
    /// <param name="context">The shared calculator context.</param>
    /// <returns>The display string (e.g., "42", "3.14", "Error").</returns>
    string GetDisplayValue(CalculatorContext context);
}