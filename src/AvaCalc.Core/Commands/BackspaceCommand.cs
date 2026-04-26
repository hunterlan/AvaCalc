namespace AvaCalc.Core.Commands;

/// <summary>Represents the backspace key press — deletes the last entered digit.</summary>
public sealed class BackspaceCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
