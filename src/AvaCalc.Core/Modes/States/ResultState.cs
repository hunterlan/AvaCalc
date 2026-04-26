using System.Globalization;
using AvaCalc.Core.Engine;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Modes.States;

/// <summary>
/// State entered after <c>=</c> is pressed and a result has been computed.
/// A new digit press starts a fresh first-operand entry;
/// an operator press uses the result as the left-hand side of a new operation.
/// </summary>
internal sealed class ResultState : CalculatorStateBase
{
    /// <inheritdoc/>
    public override void HandleDigit(CalculatorContext context, char digit)
    {
        context.CurrentInput = digit.ToString();
        context.FirstOperand = null;
        context.PendingOperator = null;
        context.LastResult = null;
        context.TransitionTo(new EnteringFirstOperandState());
    }

    /// <inheritdoc/>
    public override void HandleOperator(CalculatorContext context, CalculatorOperator op)
    {
        // Read the displayed result from CurrentInput rather than LastResult,
        // because LastResult is cleared at the start of each Execute call.
        if (decimal.TryParse(context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            context.FirstOperand = value;

        context.PendingOperator = op;
        context.LastResult = null;
        context.TransitionTo(new OperatorSelectedState());
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
    public override void HandleSignToggle(CalculatorContext context)
    {
        if (context.CurrentInput == "0")
            return;

        context.CurrentInput = context.CurrentInput.StartsWith('-')
            ? context.CurrentInput[1..]
            : '-' + context.CurrentInput;

        if (context.LastResult is not null && decimal.TryParse(context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
            context.LastResult = CalculationResult.Success(v, context.CurrentInput);
    }

    /// <inheritdoc/>
    public override string GetDisplayValue(CalculatorContext context) => context.CurrentInput;
}
