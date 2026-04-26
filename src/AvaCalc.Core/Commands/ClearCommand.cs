namespace AvaCalc.Core.Commands;

/// <summary>Represents the C (Clear) key press — clears the current entry only.</summary>
public sealed class ClearCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
