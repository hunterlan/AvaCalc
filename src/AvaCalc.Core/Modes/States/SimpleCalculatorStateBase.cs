using System.Globalization;
using AvaCalc.Core.Engine;

namespace AvaCalc.Core.Modes.States;

internal abstract class SimpleCalculatorStateBase : CalculatorStateBase
{
    public override void HandleAllClear(CalculatorContext context)
    {
        context.CurrentInput = "0";
        context.FirstOperand = null;
        context.PendingOperator = null;
        context.LastResult = null;
        context.TransitionTo(new EnteringFirstOperandState());
    }

    protected static bool TryParseCurrentInput(string input, out decimal value) =>
        decimal.TryParse(
            input.TrimEnd('.'),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out value);
}
