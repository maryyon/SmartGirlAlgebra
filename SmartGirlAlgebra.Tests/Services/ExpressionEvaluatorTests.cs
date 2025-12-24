using SmartGirlAlgebra.Models;
using SmartGirlAlgebra.Services;
using Xunit;

namespace SmartGirlAlgebra.Tests.Services;

public class ExpressionEvaluatorTests
{
    private readonly ExpressionEvaluator _evaluator = new();

    [Fact]
    public void Evaluate_ConstantOnly_ShouldReturnConstant()
    {
        var expr = new Expression(new List<Term> { new Term(5) });
        var result = _evaluator.Evaluate(expr, new Dictionary<string, decimal>());
        
        Assert.Equal(5, result);
    }

    [Fact]
    public void Evaluate_SingleVariable_ShouldSubstituteValue()
    {
        var expr = new Expression(new List<Term> { new Term(3, "x") });
        var result = _evaluator.Evaluate(expr, "x", 5);
        
        Assert.Equal(15, result);
    }

    [Fact]
    public void Evaluate_VariableAndConstant_ShouldCalculateCorrectly()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(2, "x"),
            new Term(3)
        });
        var result = _evaluator.Evaluate(expr, "x", 5);
        
        Assert.Equal(13, result); // 2*5 + 3 = 13
    }

    [Fact]
    public void Evaluate_MultipleVariables_ShouldSubstituteAll()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(2, "x"),
            new Term(3, "y"),
            new Term(5)
        });
        var values = new Dictionary<string, decimal>
        {
            { "x", 4 },
            { "y", 2 }
        };
        var result = _evaluator.Evaluate(expr, values);
        
        Assert.Equal(19, result); // 2*4 + 3*2 + 5 = 19
    }

    [Fact]
    public void Evaluate_NegativeCoefficient_ShouldCalculateCorrectly()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(-2, "x"),
            new Term(10)
        });
        var result = _evaluator.Evaluate(expr, "x", 3);
        
        Assert.Equal(4, result); // -2*3 + 10 = 4
    }

    [Fact]
    public void Evaluate_MissingVariable_ShouldThrowException()
    {
        var expr = new Expression(new List<Term> { new Term(3, "x") });
        
        Assert.Throws<ArgumentException>(() => 
            _evaluator.Evaluate(expr, new Dictionary<string, decimal>()));
    }

    [Fact]
    public void Evaluate_DecimalValues_ShouldCalculateCorrectly()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(2.5m, "x"),
            new Term(1.5m)
        });
        var result = _evaluator.Evaluate(expr, "x", 4);
        
        Assert.Equal(11.5m, result); // 2.5*4 + 1.5 = 11.5
    }

    [Fact]
    public void Evaluate_ZeroValue_ShouldCalculateCorrectly()
    {
        var expr = new Expression(new List<Term>
        {
            new Term(5, "x"),
            new Term(3)
        });
        var result = _evaluator.Evaluate(expr, "x", 0);
        
        Assert.Equal(3, result); // 5*0 + 3 = 3
    }
}

