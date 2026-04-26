using AvaCalc.Core.Shared;

namespace AvaCalc.Core.Tests.Shared;

public sealed class CalculationResultTests
{
    [Fact]
    public void Success_WithValue_IsSuccessTrue()
    {
        var result = CalculationResult.Success(42m);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Success_WithValue_ValueMatchesInput()
    {
        var result = CalculationResult.Success(42m);

        Assert.Equal(42m, result.Value);
    }

    [Fact]
    public void Success_WithValue_DisplayStringIsValueToString()
    {
        var result = CalculationResult.Success(42m);

        Assert.Equal("42", result.DisplayString);
    }

    [Fact]
    public void Success_WithExplicitDisplayString_DisplayStringMatchesOverride()
    {
        var result = CalculationResult.Success(42m, "forty-two");

        Assert.Equal("forty-two", result.DisplayString);
    }

    [Fact]
    public void Success_WithValue_ErrorMessageIsNull()
    {
        var result = CalculationResult.Success(42m);

        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void Failure_WithError_IsSuccessFalse()
    {
        var result = CalculationResult.Failure("Division by zero");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Failure_WithError_DisplayStringIsError()
    {
        var result = CalculationResult.Failure("Division by zero");

        Assert.Equal("Error", result.DisplayString);
    }

    [Fact]
    public void Failure_WithError_ErrorMessageMatchesInput()
    {
        var result = CalculationResult.Failure("Division by zero");

        Assert.Equal("Division by zero", result.ErrorMessage);
    }

    [Fact]
    public void Equality_TwoSuccessWithSameValue_AreEqual()
    {
        var a = CalculationResult.Success(10m);
        var b = CalculationResult.Success(10m);

        Assert.Equal(a, b);
    }
}
