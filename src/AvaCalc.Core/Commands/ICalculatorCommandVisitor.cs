namespace AvaCalc.Core.Commands;

/// <summary>
/// Visitor interface for exhaustive, compile-time-checked dispatch of all
/// <see cref="ICalculatorCommand"/> implementations.
/// </summary>
/// <remarks>
/// Implement this interface in any class that processes calculator commands
/// (e.g., a concrete <c>ICalculatorMode</c> implementation) to handle every
/// command type without runtime type-switch expressions.
/// Each command calls back the appropriate overload via its <c>Accept</c> method.
/// </remarks>
public interface ICalculatorCommandVisitor
{
    /// <summary>Handles an <see cref="AppendDigitCommand"/>.</summary>
    void Visit(AppendDigitCommand command);

    /// <summary>Handles an <see cref="AppendDecimalPointCommand"/>.</summary>
    void Visit(AppendDecimalPointCommand command);

    /// <summary>Handles an <see cref="ApplyOperatorCommand"/>.</summary>
    void Visit(ApplyOperatorCommand command);

    /// <summary>Handles an <see cref="EvaluateCommand"/>.</summary>
    void Visit(EvaluateCommand command);

    /// <summary>Handles a <see cref="ClearCommand"/>.</summary>
    void Visit(ClearCommand command);

    /// <summary>Handles an <see cref="AllClearCommand"/>.</summary>
    void Visit(AllClearCommand command);

    /// <summary>Handles a <see cref="BackspaceCommand"/>.</summary>
    void Visit(BackspaceCommand command);

    /// <summary>Handles a <see cref="SignToggleCommand"/>.</summary>
    void Visit(SignToggleCommand command);

    /// <summary>Handles a <see cref="PercentCommand"/>.</summary>
    void Visit(PercentCommand command);
}
