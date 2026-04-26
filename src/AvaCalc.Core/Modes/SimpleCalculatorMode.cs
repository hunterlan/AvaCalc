using System.Globalization;
using AvaCalc.Core.Commands;
using AvaCalc.Core.Engine;
using AvaCalc.Core.Modes.States;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Modes;

/// <summary>
/// Implements the Simple (basic arithmetic) calculator mode.
/// Processes all user input commands via the Visitor pattern and delegates
/// state transitions to the <see cref="CalculatorContext"/>.
/// </summary>
public sealed class SimpleCalculatorMode : ICalculatorMode, ICalculatorCommandVisitor
{
    private readonly CalculatorContext _context = new(new EnteringFirstOperandState());

    /// <inheritdoc/>
    public CalculatorMode Mode => CalculatorMode.Simple;

    /// <inheritdoc/>
    public CalculationResult Execute(ICalculatorCommand command)
    {
        _context.LastResult = null;
        command.Accept(this);

        return _context.LastResult ?? CalculationResult.Success(0m, _context.DisplayValue);
    }

    /// <inheritdoc/>
    public void Visit(AppendDigitCommand command) => _context.HandleDigit(command.Digit);

    /// <inheritdoc/>
    public void Visit(AppendDecimalPointCommand command) => _context.HandleDecimalPoint();

    /// <inheritdoc/>
    public void Visit(ApplyOperatorCommand command) => _context.HandleOperator(command.Operator);

    /// <inheritdoc/>
    public void Visit(EvaluateCommand command) => _context.HandleEquals();

    /// <inheritdoc/>
    public void Visit(ClearCommand command) => _context.HandleClear();

    /// <inheritdoc/>
    public void Visit(AllClearCommand command) => _context.HandleAllClear();

    /// <inheritdoc/>
    public void Visit(BackspaceCommand command) => _context.HandleBackspace();

    /// <inheritdoc/>
    public void Visit(SignToggleCommand command) => _context.HandleSignToggle();

    /// <inheritdoc/>
    public void Visit(PercentCommand command) => _context.HandlePercent();

    /// <inheritdoc/>
    public void Visit(SquareRootCommand command)
    {
        if (!decimal.TryParse(_context.CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return;

        if (value < 0m)
        {
            _context.LastResult = CalculationResult.Failure("Invalid input for square root");
            _context.CurrentInput = "Error";

            // Reset binary operation state so the error doesn't corrupt subsequent input.
            _context.FirstOperand = null;
            _context.PendingOperator = null;
            _context.TransitionTo(new EnteringFirstOperandState());

            return;
        }

        var result = (decimal)Math.Sqrt((double)value);
        _context.CurrentInput = result.ToString(CultureInfo.InvariantCulture);
        _context.LastResult = CalculationResult.Success(result, _context.CurrentInput);
    }
}
