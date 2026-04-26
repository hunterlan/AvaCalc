namespace AvaCalc.Core.Commands;

/// <summary>Represents the √ (square root) key press.</summary>
public sealed class SquareRootCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
