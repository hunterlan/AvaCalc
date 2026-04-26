namespace AvaCalc.Core.Shared;

/// <summary>
/// Represents the result of a calculator operation.
/// This is an immutable value object — use <see cref="Success"/> or <see cref="Failure"/> to create instances.
/// </summary>
public sealed record CalculationResult
{
    /// <summary>Gets whether the calculation succeeded.</summary>
    public bool IsSuccess { get; private init; }

    /// <summary>Gets the computed numeric value. Only meaningful when <see cref="IsSuccess"/> is <see langword="true"/>.</summary>
    public decimal Value { get; private init; }

    /// <summary>Gets the string to display on the calculator screen.</summary>
    public string DisplayString { get; private init; } = string.Empty;

    /// <summary>Gets the error message when <see cref="IsSuccess"/> is <see langword="false"/>; otherwise <see langword="null"/>.</summary>
    public string? ErrorMessage { get; private init; }

    // Private constructor — use factory methods
    private CalculationResult() { }

    /// <summary>
    /// Creates a successful calculation result.
    /// </summary>
    /// <param name="value">The computed numeric value.</param>
    /// <param name="displayString">Optional override for the display string. Defaults to <paramref name="value"/>.ToString().</param>
    /// <returns>A <see cref="CalculationResult"/> representing success.</returns>
    /// <example>
    /// <code>
    /// var result = CalculationResult.Success(42m);
    /// </code>
    /// </example>
    public static CalculationResult Success(decimal value, string? displayString = null) =>
        new() { IsSuccess = true, Value = value, DisplayString = displayString ?? value.ToString() };

    /// <summary>
    /// Creates a failed calculation result.
    /// </summary>
    /// <param name="error">A human-readable error message (e.g., "Division by zero").</param>
    /// <returns>A <see cref="CalculationResult"/> representing failure.</returns>
    /// <example>
    /// <code>
    /// var result = CalculationResult.Failure("Division by zero");
    /// </code>
    /// </example>
    public static CalculationResult Failure(string error) =>
        new() { IsSuccess = false, DisplayString = "Error", ErrorMessage = error };
}