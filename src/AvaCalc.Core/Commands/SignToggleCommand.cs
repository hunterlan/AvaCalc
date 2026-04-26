namespace AvaCalc.Core.Commands;

/// <summary>Represents the ± key press — negates the current input value.</summary>
public sealed class SignToggleCommand : ICalculatorCommand
{
    /// <inheritdoc/>
    public void Accept(ICalculatorCommandVisitor visitor) => visitor.Visit(this);
}
