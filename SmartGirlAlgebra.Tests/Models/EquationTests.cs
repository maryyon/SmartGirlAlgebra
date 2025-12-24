using SmartGirlAlgebra.Models;
using Xunit;

namespace SmartGirlAlgebra.Tests.Models;

public class EquationTests
{
    [Fact]
    public void Constructor_ShouldSetBothSides()
    {
        var left = new Expression(new List<Term> { new Term(2, "x"), new Term(3) });
        var right = new Expression(new List<Term> { new Term(11) });
        
        var equation = new Equation(left, right);
        
        Assert.Equal(left, equation.LeftSide);
        Assert.Equal(right, equation.RightSide);
    }

    [Fact]
    public void ToString_ShouldFormatEquation()
    {
        var left = new Expression(new List<Term> { new Term(2, "x"), new Term(3) });
        var right = new Expression(new List<Term> { new Term(11) });
        var equation = new Equation(left, right);
        
        Assert.Equal("2x + 3 = 11", equation.ToString());
    }
}

