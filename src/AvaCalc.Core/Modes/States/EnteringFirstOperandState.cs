using System.Globalization;
using AvaCalc.Core.Engine;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Modes.States;

/// <summary>
/// The initial calculator state where the user is entering the first operand.
/// Transitions to <see cref="OperatorSelectedState"/> when an operator is pressed.
/// </summary>
internal sealed class EnteringFirstOperandState : CalculatorStateBase
{
    /// <inheritdoc/>
    public override void HandleDigit(CalculatorContext context, char digit)
    {
        context.CurrentInput = context.CurrentInput is "0" or "Error"
            ? digit.ToString()
            : context.CurrentInput + digit;
    }

    /// <inheritdoc/>
    public override void HandleDecimalPoint(CalculatorContext context)
    {
        if (!context.CurrentInput.Contains('.'))
            context.CurrentInput += ".";
    }

    /// <inheritdoc/>
    public override void HandleOperator(CalculatorContext context, CalculatorOperator op)
    {
        if (decimal.TryParse(context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            context.FirstOperand = value;

        context.PendingOperator = op;
        context.TransitionTo(new OperatorSelectedState());
    }

    /// <inheritdoc/>
    public override void HandleClear(CalculatorContext context)
    {
        context.CurrentInput = "0";
    }

    /// <inheritdoc/>
    public override void HandleAllClear(CalculatorContext context)
    {
        context.CurrentInput = "0";
        context.FirstOperand = null;
        context.PendingOperator = null;
        context.LastResult = null;
    }

    /// <inheritdoc/>
    public override void HandleBackspace(CalculatorContext context)
    {
        context.CurrentInput = context.CurrentInput.Length > 1
            ? context.CurrentInput[..^1]
            : "0";
    }

    /// <inheritdoc/>
    public override void HandleSignToggle(CalculatorContext context)
    {
        if (context.CurrentInput == "0")
            return;

        context.CurrentInput = context.CurrentInput.StartsWith('-')
            ? context.CurrentInput[1..]
            : '-' + context.CurrentInput;
    }

    /// <inheritdoc/>
    public override void HandlePercent(CalculatorContext context)
    {
        if (decimal.TryParse(context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            context.CurrentInput = (value / 100m).ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public override string GetDisplayValue(CalculatorContext context) => context.CurrentInput;
}
