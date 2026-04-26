using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Commands;

/// <summary>Represents a binary operator key press (+, −, ×, ÷).</summary>
/// <param name="Operator">The operator to apply.</param>
public sealed record ApplyOperatorCommand(CalculatorOperator Operator) : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
