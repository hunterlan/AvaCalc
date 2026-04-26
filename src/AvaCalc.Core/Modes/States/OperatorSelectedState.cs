using AvaCalc.Core.Engine;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Modes.States;

/// <summary>
/// State entered immediately after the user presses an operator.
/// Displays the first operand and waits for the user to start entering the second.
/// Transitions to <see cref="EnteringSecondOperandState"/> on the first digit or decimal point.
/// </summary>
internal sealed class OperatorSelectedState : CalculatorStateBase
{
    /// <inheritdoc/>
    public override void HandleDigit(CalculatorContext context, char digit)
    {
        context.CurrentInput = digit.ToString();
        context.TransitionTo(new EnteringSecondOperandState());
    }

    /// <inheritdoc/>
    public override void HandleDecimalPoint(CalculatorContext context)
    {
        context.CurrentInput = "0.";
        context.TransitionTo(new EnteringSecondOperandState());
    }

    /// <inheritdoc/>
    public override void HandleOperator(CalculatorContext context, CalculatorOperator op)
    {
        context.PendingOperator = op;
    }

    /// <inheritdoc/>
    public override void HandleAllClear(CalculatorContext context)
    {
        context.CurrentInput = "0";
        context.FirstOperand = null;
        context.PendingOperator = null;
        context.LastResult = null;
        context.TransitionTo(new EnteringFirstOperandState());
    }

    /// <inheritdoc/>
    public override string GetDisplayValue(CalculatorContext context) => context.CurrentInput;
}
