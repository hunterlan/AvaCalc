namespace AvaCalc.Core.Commands;

/// <summary>Represents the equals (=) key press — triggers evaluation of the current expression.</summary>
public sealed class EvaluateCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
