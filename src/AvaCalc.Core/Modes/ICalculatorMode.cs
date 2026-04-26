using AvaCalc.Core.Commands;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Modes;

/// <summary>
/// Defines the contract for a calculator mode.
/// Each mode (Simple, Science, Statistics, NumeralSystems) implements this interface
/// and handles the full set of user input commands.
/// </summary>
/// <remarks>
/// Implement this interface to add a new calculator mode.
/// Register the implementation with the DI container and add a corresponding
/// <see cref="CalculatorMode"/> enum value.
/// </remarks>
public interface ICalculatorMode
{
    /// <summary>Gets the mode identifier for this calculator implementation.</summary>
    CalculatorMode Mode { get; }

    /// <summary>
    /// Processes a user input command and returns the updated display result.
    /// </summary>
    /// <param name="command">The command representing the user action (digit, operator, evaluate, etc.).</param>
    /// <returns>
    /// A <see cref="CalculationResult"/> describing the new display state after the command.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = calculator.Execute(new AppendDigitCommand('5'));
    /// Console.WriteLine(result.DisplayString); // "5"
    /// </code>
    /// </example>
    CalculationResult Execute(ICalculatorCommand command);
}
