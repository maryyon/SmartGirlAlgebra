using SmartGirlAlgebra.Models;
using SmartGirlAlgebra.Services;
using Xunit;

namespace SmartGirlAlgebra.Tests.Services;

public class ExpressionParserTests
{
    private readonly ExpressionParser _parser = new();

    [Fact]
    public void Parse_SingleConstant_ShouldReturnConstantTerm()
    {
        var expr = _parser.Parse("5");
        
        Assert.Single(expr.Terms);
        Assert.Equal(5, expr.Terms[0].Coefficient);
        Assert.True(expr.Terms[0].IsConstant);
    }

    [Fact]
    public void Parse_SingleVariable_ShouldReturnVariableTermWithCoefficientOne()
    {
        var expr = _parser.Parse("x");
        
        Assert.Single(expr.Terms);
        Assert.Equal(1, expr.Terms[0].Coefficient);
        Assert.Equal("x", expr.Terms[0].Variable);
    }

    [Fact]
    public void Parse_VariableWithCoefficient_ShouldReturnCorrectTerm()
    {
        var expr = _parser.Parse("3x");
        
        Assert.Single(expr.Terms);
        Assert.Equal(3, expr.Terms[0].Coefficient);
        Assert.Equal("x", expr.Terms[0].Variable);
    }

    [Fact]
    public void Parse_TwoTermsWithPlus_ShouldReturnTwoTerms()
    {
        var expr = _parser.Parse("2x + 3");
        
        Assert.Equal(2, expr.Terms.Count);
        Assert.Equal(2, expr.Terms[0].Coefficient);
        Assert.Equal("x", expr.Terms[0].Variable);
        Assert.Equal(3, expr.Terms[1].Coefficient);
        Assert.True(expr.Terms[1].IsConstant);
    }

    [Fact]
    public void Parse_TwoTermsWithMinus_ShouldReturnNegativeSecondTerm()
    {
        var expr = _parser.Parse("2x - 3");
        
        Assert.Equal(2, expr.Terms.Count);
        Assert.Equal(2, expr.Terms[0].Coefficient);
        Assert.Equal(-3, expr.Terms[1].Coefficient);
    }

    [Fact]
    public void Parse_WithSpaces_ShouldIgnoreSpaces()
    {
        var expr = _parser.Parse("2x + 3");
        
        Assert.Equal(2, expr.Terms.Count);
        Assert.Equal(2, expr.Terms[0].Coefficient);
        Assert.Equal(3, expr.Terms[1].Coefficient);
    }

    [Fact]
    public void Parse_MultipleTerms_ShouldParseAll()
    {
        var expr = _parser.Parse("3x + 2y - 5");
        
        Assert.Equal(3, expr.Terms.Count);
        Assert.Equal(3, expr.Terms[0].Coefficient);
        Assert.Equal("x", expr.Terms[0].Variable);
        Assert.Equal(2, expr.Terms[1].Coefficient);
        Assert.Equal("y", expr.Terms[1].Variable);
        Assert.Equal(-5, expr.Terms[2].Coefficient);
        Assert.True(expr.Terms[2].IsConstant);
    }

    [Fact]
    public void Parse_EmptyString_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _parser.Parse(""));
    }

    [Fact]
    public void Parse_NegativeFirstTerm_ShouldHandleCorrectly()
    {
        var expr = _parser.Parse("-2x + 5");
        
        Assert.Equal(2, expr.Terms.Count);
        Assert.Equal(-2, expr.Terms[0].Coefficient);
        Assert.Equal(5, expr.Terms[1].Coefficient);
    }

    [Fact]
    public void Parse_DecimalCoefficient_ShouldParseCorrectly()
    {
        var expr = _parser.Parse("2.5x + 1.5");
        
        Assert.Equal(2, expr.Terms.Count);
        Assert.Equal(2.5m, expr.Terms[0].Coefficient);
        Assert.Equal(1.5m, expr.Terms[1].Coefficient);
    }
}

