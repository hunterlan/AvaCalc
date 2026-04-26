namespace AvaCalc.Core.Commands;

/// <summary>Represents the % key press — converts the current value to a percentage.</summary>
public sealed class PercentCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
