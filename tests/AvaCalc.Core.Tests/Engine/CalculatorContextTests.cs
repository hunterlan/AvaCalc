using AvaCalc.Core.Engine;
using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Tests.Engine;

public sealed class CalculatorContextTests
{
    [Fact]
    public void Constructor_WithNullState_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new CalculatorContext(null!));
    }

    [Fact]
    public void TransitionTo_WithNullState_ThrowsArgumentNullException()
    {
        var context = new CalculatorContext(new SpyState());

        Assert.Throws<ArgumentNullException>(() => context.TransitionTo(null!));
    }

    [Fact]
    public void TransitionTo_WithNewState_DelegatesSubsequentCallsToNewState()
    {
        var initial = new SpyState();
        var next = new SpyState();
        var context = new CalculatorContext(initial);

        context.TransitionTo(next);
        context.HandleDigit('5');

        Assert.Equal(0, initial.DigitCallCount);
        Assert.Equal(1, next.DigitCallCount);
    }

    [Fact]
    public void HandleDigit_DelegatesToCurrentState()
    {
        var spy = new SpyState();
        var context = new CalculatorContext(spy);

        context.HandleDigit('3');

        Assert.Equal(1, spy.DigitCallCount);
        Assert.Equal('3', spy.LastDigit);
    }

    [Fact]
    public void HandleEquals_DelegatesToCurrentState()
    {
        var spy = new SpyState();
        var context = new CalculatorContext(spy);

        context.HandleEquals();

        Assert.Equal(1, spy.EqualsCallCount);
    }

    [Fact]
    public void HandleAllClear_DelegatesToCurrentState()
    {
        var spy = new SpyState();
        var context = new CalculatorContext(spy);

        context.HandleAllClear();

        Assert.Equal(1, spy.AllClearCallCount);
    }

    [Fact]
    public void DisplayValue_ReturnsValueFromCurrentState()
    {
        var spy = new SpyState { DisplayValueToReturn = "99" };
        var context = new CalculatorContext(spy);

        Assert.Equal("99", context.DisplayValue);
    }

    // ── Test spy ─────────────────────────────────────────────────────────────

    private sealed class SpyState : ICalculatorState
    {
        public int DigitCallCount { get; private set; }
        public char LastDigit { get; private set; }
        public int EqualsCallCount { get; private set; }
        public int AllClearCallCount { get; private set; }
        public string DisplayValueToReturn { get; init; } = "0";

        public void HandleDigit(CalculatorContext context, char digit)
        {
            DigitCallCount++;
            LastDigit = digit;
        }

        public void HandleDecimalPoint(CalculatorContext context) { }
        public void HandleOperator(CalculatorContext context, CalculatorOperator op) { }

        public void HandleEquals(CalculatorContext context) => EqualsCallCount++;

        public void HandleClear(CalculatorContext context) { }

        public void HandleAllClear(CalculatorContext context) => AllClearCallCount++;

        public void HandleBackspace(CalculatorContext context) { }
        public void HandleSignToggle(CalculatorContext context) { }
        public void HandlePercent(CalculatorContext context) { }
        public void HandleSquareRoot(CalculatorContext context) { }

        public string GetDisplayValue(CalculatorContext context) => DisplayValueToReturn;
    }
}
