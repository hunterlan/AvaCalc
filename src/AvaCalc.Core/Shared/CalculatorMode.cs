namespace AvaCalc.Core.Shared;

/// <summary>
/// Represents the available calculator modes.
/// </summary>
public enum CalculatorMode
{
    /// <summary>Basic arithmetic operations.</summary>
    Simple,

    /// <summary>Scientific functions (trigonometry, logarithms, powers, etc.).</summary>
    Science,

    /// <summary>Statistical calculations (mean, median, standard deviation, etc.).</summary>
    Statistics,

    /// <summary>Numeral system conversions (binary, octal, hexadecimal).</summary>
    NumeralSystems
}
