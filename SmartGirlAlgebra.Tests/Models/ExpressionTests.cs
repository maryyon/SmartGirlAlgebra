using SmartGirlAlgebra.Models;
using Xunit;

namespace SmartGirlAlgebra.Tests.Models;

public class ExpressionTests
{
    [Fact]
    public void EmptyExpression_ShouldReturnZero()
    {
        var expr = new Expression();
        
        Assert.Equal("0", expr.ToString());
    }

    [Fact]
    public void AddTerm_ShouldAddTermToExpression()
    {
        var expr = new Expression();
        expr.AddTerm(new Term(3, "x"));
        
        Assert.Single(expr.Terms);
        Assert.Equal(3, expr.Terms[0].Coefficient);
    }

    [Fact]
    public void ToString_SingleTerm_ShouldReturnTermString()
    {
        var expr = new Expression();
        expr.AddTerm(new Term(3, "x"));
        
        Assert.Equal("3x", expr.ToString());
    }

    [Fact]
    public void ToString_MultipleTerms_ShouldFormatWithPlusSigns()
    {
        var expr = new Expression();
        expr.AddTerm(new Term(2, "x"));
        expr.AddTerm(new Term(3));
        
        Assert.Equal("2x + 3", expr.ToString());
    }

    [Fact]
    public void ToString_NegativeTerm_ShouldFormatWithMinusSign()
    {
        var expr = new Expression();
        expr.AddTerm(new Term(2, "x"));
        expr.AddTerm(new Term(-3));
        
        Assert.Equal("2x - 3", expr.ToString());
    }
}

