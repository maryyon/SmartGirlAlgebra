using SmartGirlAlgebra.Models;
using SmartGirlAlgebra.Services;
using Xunit;

namespace SmartGirlAlgebra.Tests.Services;

public class ExpressionSimplifierTests
{
    private readonly ExpressionSimplifier _simplifier = new();

    [Fact]
    public void Simplify_EmptyExpression_ShouldReturnEmptyExpression()
    {
        var expr = new Expression();
        var result = _simplifier.Simplify(expr);
        
        Assert.Empty(result.Terms);
    }

    [Fact]
    public void Simplify_SingleTerm_ShouldReturnSameTerm()
    {
        var expr = new Expression(new List<Term> { new Term(3, "x") });
        var result = _simplifier.Simplify(expr);
        
        Assert.Single(result.Terms);
        Assert.Equal(3, result.Terms[0].Coefficient);
        Assert.Equal("x", result.Terms[0].Variable);
    }

    [Fact]
    public void Simplify_LikeTerms_ShouldCombine()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(2, "x"),
            new Term(3, "x")
        });
        var result = _simplifier.Simplify(expr);
        
        Assert.Single(result.Terms);
        Assert.Equal(5, result.Terms[0].Coefficient);
        Assert.Equal("x", result.Terms[0].Variable);
    }

    [Fact]
    public void Simplify_Constants_ShouldCombine()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(5),
            new Term(3)
        });
        var result = _simplifier.Simplify(expr);
        
        Assert.Single(result.Terms);
        Assert.Equal(8, result.Terms[0].Coefficient);
        Assert.True(result.Terms[0].IsConstant);
    }

    [Fact]
    public void Simplify_MixedTerms_ShouldCombineLikeTerms()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(2, "x"),
            new Term(3),
            new Term(3, "x"),
            new Term(5)
        });
        var result = _simplifier.Simplify(expr);
        
        Assert.Equal(2, result.Terms.Count);
        
        // Should be sorted: variables first, then constants
        Assert.Equal(5, result.Terms[0].Coefficient);
        Assert.Equal("x", result.Terms[0].Variable);
        Assert.Equal(8, result.Terms[1].Coefficient);
        Assert.True(result.Terms[1].IsConstant);
    }

    [Fact]
    public void Simplify_DifferentVariables_ShouldKeepSeparate()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(2, "x"),
            new Term(3, "y")
        });
        var result = _simplifier.Simplify(expr);
        
        Assert.Equal(2, result.Terms.Count);
        Assert.Equal(2, result.Terms[0].Coefficient);
        Assert.Equal("x", result.Terms[0].Variable);
        Assert.Equal(3, result.Terms[1].Coefficient);
        Assert.Equal("y", result.Terms[1].Variable);
    }

    [Fact]
    public void Simplify_ZeroCoefficient_ShouldRemoveTerm()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(3, "x"),
            new Term(-3, "x")
        });
        var result = _simplifier.Simplify(expr);
        
        Assert.Empty(result.Terms);
    }

    [Fact]
    public void Simplify_NegativeTerms_ShouldCombineCorrectly()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(5, "x"),
            new Term(-2, "x")
        });
        var result = _simplifier.Simplify(expr);
        
        Assert.Single(result.Terms);
        Assert.Equal(3, result.Terms[0].Coefficient);
        Assert.Equal("x", result.Terms[0].Variable);
    }
}

