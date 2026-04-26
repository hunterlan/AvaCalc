namespace AvaCalc.Core.Commands;

/// <summary>
/// Contract for all calculator input commands.
/// Commands are data carriers representing a user action (e.g., pressing a digit or operator).
/// They are dispatched to <see cref="AvaCalc.Core.Modes.ICalculatorMode.Execute"/> for processing.
/// </summary>
/// <remarks>
/// Implement <see cref="Accept"/> using the Visitor pattern to enable exhaustive,
/// compile-time-checked dispatch without runtime type-switch expressions:
/// <code>
/// public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
/// </code>
/// </remarks>
public interface ICalculatorCommand
{
    /// <summary>
    /// Accepts a <see cref="ICalculatorCommandVisitor"/> and dispatches to the appropriate overload.
    /// </summary>
    /// <param name="visitor">The visitor that will handle this command.</param>
    void Accept(ICalculatorCommandVisitor visitor);
}