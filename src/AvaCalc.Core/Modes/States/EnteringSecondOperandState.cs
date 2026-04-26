using System.Globalization;
using AvaCalc.Core.Engine;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Modes.States;

/// <summary>
/// State where the user is entering the second operand of a binary operation.
/// Evaluates and transitions to <see cref="ResultState"/> when <c>=</c> is pressed,
/// or chains to <see cref="OperatorSelectedState"/> when a new operator is pressed.
/// </summary>
internal sealed class EnteringSecondOperandState : CalculatorStateBase
{
    /// <inheritdoc/>
    public override void HandleDigit(CalculatorContext context, char digit)
    {
        context.CurrentInput = context.CurrentInput == "0"
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
        Evaluate(context);

        if (context.LastResult?.IsSuccess is false)
            return;

        context.FirstOperand = decimal.TryParse(context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var v)
            ? v
            : context.FirstOperand;

        context.PendingOperator = op;
        context.LastResult = null;
        context.TransitionTo(new OperatorSelectedState());
    }

    /// <inheritdoc/>
    public override void HandleEquals(CalculatorContext context)
    {
        Evaluate(context);

        if (context.LastResult?.IsSuccess is true)
            context.TransitionTo(new ResultState());
        else
            context.TransitionTo(new EnteringFirstOperandState());
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
        context.TransitionTo(new EnteringFirstOperandState());
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

    private static void Evaluate(CalculatorContext context)
    {
        if (!context.FirstOperand.HasValue || !context.PendingOperator.HasValue)
            return;

        if (!decimal.TryParse(context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var second))
            return;

        var first = context.FirstOperand.Value;
        var op = context.PendingOperator.Value;

        if (op is CalculatorOperator.Divide && second == 0m)
        {
            context.LastResult = CalculationResult.Failure("Division by zero");
            context.CurrentInput = "Error";
            context.FirstOperand = null;
            context.PendingOperator = null;

            return;
        }

        var result = op switch
        {
            CalculatorOperator.Add => first + second,
            CalculatorOperator.Subtract => first - second,
            CalculatorOperator.Multiply => first * second,
            CalculatorOperator.Divide => first / second,
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };

        context.LastResult = CalculationResult.Success(result);
        context.CurrentInput = context.LastResult.DisplayString;
        context.FirstOperand = null;
        context.PendingOperator = null;
    }
}
