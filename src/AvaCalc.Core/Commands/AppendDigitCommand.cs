namespace AvaCalc.Core.Commands;

/// <summary>Represents a digit key press (0–9).</summary>
/// <param name="Digit">The digit character pressed.</param>
public sealed record AppendDigitCommand(char Digit) : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
