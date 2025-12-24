using SmartGirlAlgebra.Models;
using Xunit;

namespace SmartGirlAlgebra.Tests.Models;

public class TermTests
{
    [Fact]
    public void ConstantTerm_ShouldHaveNoVariable()
    {
        var term = new Term(5);
        
        Assert.Equal(5, term.Coefficient);
        Assert.Null(term.Variable);
        Assert.True(term.IsConstant);
    }

    [Fact]
    public void VariableTerm_ShouldHaveVariable()
    {
        var term = new Term(3, "x");
        
        Assert.Equal(3, term.Coefficient);
        Assert.Equal("x", term.Variable);
        Assert.False(term.IsConstant);
    }

    [Fact]
    public void HasSameVariable_ShouldReturnTrue_WhenVariablesMatch()
    {
        var term1 = new Term(3, "x");
        var term2 = new Term(5, "x");
        
        Assert.True(term1.HasSameVariable(term2));
    }

    [Fact]
    public void HasSameVariable_ShouldReturnFalse_WhenVariablesDiffer()
    {
        var term1 = new Term(3, "x");
        var term2 = new Term(5, "y");
        
        Assert.False(term1.HasSameVariable(term2));
    }

    [Fact]
    public void ToString_ConstantTerm_ShouldReturnNumber()
    {
        var term = new Term(5);
        
        Assert.Equal("5", term.ToString());
    }

    [Fact]
    public void ToString_CoefficientOne_ShouldReturnVariableOnly()
    {
        var term = new Term(1, "x");
        
        Assert.Equal("x", term.ToString());
    }

    [Fact]
    public void ToString_CoefficientNegativeOne_ShouldReturnMinusVariable()
    {
        var term = new Term(-1, "x");
        
        Assert.Equal("-x", term.ToString());
    }

    [Fact]
    public void ToString_RegularTerm_ShouldReturnCoefficientAndVariable()
    {
        var term = new Term(3, "x");
        
        Assert.Equal("3x", term.ToString());
    }
}

