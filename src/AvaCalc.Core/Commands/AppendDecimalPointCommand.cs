namespace AvaCalc.Core.Commands;

/// <summary>Represents the decimal point (.) key press.</summary>
public sealed class AppendDecimalPointCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
