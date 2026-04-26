using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Engine;

/// <summary>
/// Holds the shared mutable state of the calculator and delegates all user input
/// to the currently active <see cref="ICalculatorState"/>.
/// </summary>
/// <remarks>
/// <para>
/// This class is the central coordinator of the State pattern.
/// Concrete state classes call <see cref="TransitionTo"/> to move the context
/// into a new state without the context needing to know about the concrete types.
/// </para>
/// <para>
/// Each calculator mode creates and owns its own <see cref="CalculatorContext"/> instance.
/// </para>
/// </remarks>
public sealed class CalculatorContext
{
    private ICalculatorState _currentState;

    /// <summary>Gets the first (left-hand) operand of a binary operation.</summary>
    public decimal? FirstOperand { get; internal set; }

    /// <summary>Gets the pending binary operator.</summary>
    public CalculatorOperator? PendingOperator { get; internal set; }

    /// <summary>Gets the raw string being typed by the user for the current operand.</summary>
    public string CurrentInput { get; internal set; } = "0";

    /// <summary>Gets the last computed result.</summary>
    public CalculationResult? LastResult { get; internal set; }

    /// <summary>
    /// Initialises a new <see cref="CalculatorContext"/> with the given initial state.
    /// </summary>
    /// <param name="initialState">The state the calculator starts in.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="initialState"/> is null.</exception>
    public CalculatorContext(ICalculatorState initialState)
    {
        ArgumentNullException.ThrowIfNull(initialState, nameof(initialState));
        _currentState = initialState;
    }

    /// <summary>
    /// Transitions the context to a new state.
    /// Called by concrete <see cref="ICalculatorState"/> implementations.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="newState"/> is null.</exception>
    public void TransitionTo(ICalculatorState newState)
    {
        ArgumentNullException.ThrowIfNull(newState, nameof(newState));
        _currentState = newState;
    }

    /// <summary>Gets the string to display, delegated to the current state.</summary>
    public string DisplayValue => _currentState.GetDisplayValue(this);

    // --- Input delegation methods ---
    // Each method forwards to the current state. Callers are ICalculatorMode implementations.

    /// <summary>Forwards a digit input to the current state.</summary>
    public void HandleDigit(char digit) => _currentState.HandleDigit(this, digit);

    /// <summary>Forwards a decimal point input to the current state.</summary>
    public void HandleDecimalPoint() => _currentState.HandleDecimalPoint(this);

    /// <summary>Forwards an operator input to the current state.</summary>
    public void HandleOperator(CalculatorOperator op) => _currentState.HandleOperator(this, op);

    /// <summary>Forwards an equals input to the current state.</summary>
    public void HandleEquals() => _currentState.HandleEquals(this);

    /// <summary>Forwards a clear input to the current state.</summary>
    public void HandleClear() => _currentState.HandleClear(this);

    /// <summary>Forwards an all-clear input to the current state.</summary>
    public void HandleAllClear() => _currentState.HandleAllClear(this);

    /// <summary>Forwards a backspace input to the current state.</summary>
    public void HandleBackspace() => _currentState.HandleBackspace(this);

    /// <summary>Forwards a sign-toggle input to the current state.</summary>
    public void HandleSignToggle() => _currentState.HandleSignToggle(this);

    /// <summary>Forwards a percent input to the current state.</summary>
    public void HandlePercent() => _currentState.HandlePercent(this);

    /// <summary>Forwards a square-root input to the current state.</summary>
    public void HandleSquareRoot() => _currentState.HandleSquareRoot(this);
}