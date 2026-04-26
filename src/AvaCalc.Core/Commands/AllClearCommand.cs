namespace AvaCalc.Core.Commands;

/// <summary>Represents the AC (All Clear) key press — resets the entire calculator state.</summary>
public sealed class AllClearCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
