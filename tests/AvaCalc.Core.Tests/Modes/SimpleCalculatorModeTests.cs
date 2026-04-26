using AvaCalc.Core.Commands;
using AvaCalc.Core.Modes;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Tests.Modes;

/// <summary>
/// Tests for <see cref="SimpleCalculatorMode"/> covering all state transitions
/// and arithmetic operations via the public <see cref="ICalculatorMode.Execute"/> API.
/// </summary>
public sealed class SimpleCalculatorModeTests
{
    // ── Initial state ─────────────────────────────────────────────────────────

    [Fact]
    public void InitialState_DisplayStringIsZero()
    {
        var mode = new SimpleCalculatorMode();

        var result = mode.Execute(new EvaluateCommand());

        Assert.Equal("0", result.DisplayString);
    }

    // ── Digit entry ───────────────────────────────────────────────────────────

    [Fact]
    public void AppendDigit_SingleDigit_DisplaysDigit()
    {
        var mode = new SimpleCalculatorMode();

        var result = mode.Execute(new AppendDigitCommand('5'));

        Assert.Equal("5", result.DisplayString);
    }

    [Fact]
    public void AppendDigit_MultipleDigits_BuildsNumber()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('1'));
        mode.Execute(new AppendDigitCommand('2'));
        var result = mode.Execute(new AppendDigitCommand('3'));

        Assert.Equal("123", result.DisplayString);
    }

    [Fact]
    public void AppendDigit_AfterZero_ReplacesZero()
    {
        var mode = new SimpleCalculatorMode();

        var result = mode.Execute(new AppendDigitCommand('7'));

        Assert.Equal("7", result.DisplayString);
    }

    // ── Decimal point ─────────────────────────────────────────────────────────

    [Fact]
    public void AppendDecimalPoint_AddsDecimalToDisplay()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('3'));
        var result = mode.Execute(new AppendDecimalPointCommand());

        Assert.Equal("3.", result.DisplayString);
    }

    [Fact]
    public void AppendDecimalPoint_Twice_DoesNotAddSecondDecimal()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('3'));
        mode.Execute(new AppendDecimalPointCommand());
        var result = mode.Execute(new AppendDecimalPointCommand());

        Assert.Equal("3.", result.DisplayString);
    }

    // ── Backspace ─────────────────────────────────────────────────────────────

    [Fact]
    public void Backspace_RemovesLastDigit()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('1'));
        mode.Execute(new AppendDigitCommand('2'));
        var result = mode.Execute(new BackspaceCommand());

        Assert.Equal("1", result.DisplayString);
    }

    [Fact]
    public void Backspace_OnSingleDigit_ShowsZero()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('5'));
        var result = mode.Execute(new BackspaceCommand());

        Assert.Equal("0", result.DisplayString);
    }

    // ── Sign toggle ───────────────────────────────────────────────────────────

    [Fact]
    public void SignToggle_NegatesPositiveValue()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('4'));
        var result = mode.Execute(new SignToggleCommand());

        Assert.Equal("-4", result.DisplayString);
    }

    [Fact]
    public void SignToggle_Twice_RestoresOriginalValue()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('4'));
        mode.Execute(new SignToggleCommand());
        var result = mode.Execute(new SignToggleCommand());

        Assert.Equal("4", result.DisplayString);
    }

    // ── Percent ───────────────────────────────────────────────────────────────

    [Fact]
    public void Percent_DividesValueByHundred()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('5'));
        mode.Execute(new AppendDigitCommand('0'));
        var result = mode.Execute(new PercentCommand());

        Assert.Equal("0.5", result.DisplayString);
    }

    // ── Square root ───────────────────────────────────────────────────────────

    [Fact]
    public void SquareRoot_OfPerfectSquare_ReturnsRoot()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('9'));
        var result = mode.Execute(new SquareRootCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal("3", result.DisplayString);
    }

    [Fact]
    public void SquareRoot_OfNegativeNumber_ReturnsFailure()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('4'));
        mode.Execute(new SignToggleCommand());
        var result = mode.Execute(new SquareRootCommand());

        Assert.False(result.IsSuccess);
        Assert.Equal("Error", result.DisplayString);
    }

    [Fact]
    public void SquareRoot_OfNegativeNumber_AllowsFreshDigitEntry()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('4'));
        mode.Execute(new SignToggleCommand());
        mode.Execute(new SquareRootCommand()); // → Error

        var result = mode.Execute(new AppendDigitCommand('7'));

        Assert.Equal("7", result.DisplayString);
    }

    // ── Arithmetic ────────────────────────────────────────────────────────────

    [Fact]
    public void Addition_TwoOperands_ReturnsSum()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('3'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Add));
        mode.Execute(new AppendDigitCommand('5'));
        var result = mode.Execute(new EvaluateCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal("8", result.DisplayString);
    }

    [Fact]
    public void Subtraction_TwoOperands_ReturnsDifference()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('9'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Subtract));
        mode.Execute(new AppendDigitCommand('4'));
        var result = mode.Execute(new EvaluateCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal("5", result.DisplayString);
    }

    [Fact]
    public void Multiplication_TwoOperands_ReturnsProduct()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('6'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Multiply));
        mode.Execute(new AppendDigitCommand('7'));
        var result = mode.Execute(new EvaluateCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal("42", result.DisplayString);
    }

    [Fact]
    public void Division_TwoOperands_ReturnsQuotient()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('8'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Divide));
        mode.Execute(new AppendDigitCommand('4'));
        var result = mode.Execute(new EvaluateCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal("2", result.DisplayString);
    }

    [Fact]
    public void Division_ByZero_ReturnsFailure()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('8'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Divide));
        mode.Execute(new AppendDigitCommand('0'));
        var result = mode.Execute(new EvaluateCommand());

        Assert.False(result.IsSuccess);
        Assert.Equal("Error", result.DisplayString);
    }

    [Fact]
    public void OperatorChaining_AfterResult_UsesPriorResult()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('3'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Add));
        mode.Execute(new AppendDigitCommand('5'));
        mode.Execute(new EvaluateCommand()); // = 8

        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Add));
        mode.Execute(new AppendDigitCommand('2'));
        var result = mode.Execute(new EvaluateCommand()); // 8 + 2 = 10

        Assert.Equal("10", result.DisplayString);
    }

    // ── Clear / AllClear ──────────────────────────────────────────────────────

    [Fact]
    public void ClearCommand_ResetsCurrentInputToZero()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('7'));
        mode.Execute(new AppendDigitCommand('8'));
        var result = mode.Execute(new ClearCommand());

        Assert.Equal("0", result.DisplayString);
    }

    [Fact]
    public void AllClearCommand_ResetsFullState()
    {
        var mode = new SimpleCalculatorMode();

        mode.Execute(new AppendDigitCommand('3'));
        mode.Execute(new ApplyOperatorCommand(CalculatorOperator.Add));
        mode.Execute(new AppendDigitCommand('5'));
        mode.Execute(new AllClearCommand());

        // After AC, pressing = should show 0 (no pending operands)
        var result = mode.Execute(new EvaluateCommand());

        Assert.Equal("0", result.DisplayString);
    }

    // ── Mode property ─────────────────────────────────────────────────────────

    [Fact]
    public void Mode_ReturnsSimple()
    {
        var mode = new SimpleCalculatorMode();

        Assert.Equal(CalculatorMode.Simple, mode.Mode);
    }
}
