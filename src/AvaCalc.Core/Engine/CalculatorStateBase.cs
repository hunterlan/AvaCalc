using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Engine;

/// <summary>
/// Base class for calculator states that provides default no-op implementations
/// for all <see cref="ICalculatorState"/> handler methods.
/// Concrete states override only the methods relevant to their current phase.
/// </summary>
/// <remarks>
/// Extend this class instead of implementing <see cref="ICalculatorState"/> directly
/// to satisfy the Interface Segregation Principle — concrete states only override
/// the inputs they actually handle.
/// </remarks>
public abstract class CalculatorStateBase : ICalculatorState
{
    /// <inheritdoc/>
    public virtual void HandleDigit(CalculatorContext context, char digit) { }

    /// <inheritdoc/>
    public virtual void HandleDecimalPoint(CalculatorContext context) { }

    /// <inheritdoc/>
    public virtual void HandleOperator(CalculatorContext context, CalculatorOperator op) { }

    /// <inheritdoc/>
    public virtual void HandleEquals(CalculatorContext context) { }

    /// <inheritdoc/>
    public virtual void HandleClear(CalculatorContext context) { }

    /// <inheritdoc/>
    public virtual void HandleAllClear(CalculatorContext context) { }

    /// <inheritdoc/>
    public virtual void HandleBackspace(CalculatorContext context) { }

    /// <inheritdoc/>
    public virtual void HandleSignToggle(CalculatorContext context) { }

    /// <inheritdoc/>
    public virtual void HandlePercent(CalculatorContext context) { }

    /// <inheritdoc/>
    public abstract string GetDisplayValue(CalculatorContext context);
}
