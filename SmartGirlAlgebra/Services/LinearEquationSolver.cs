using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Solves simple linear equations of the form ax + b = c
/// </summary>
public class LinearEquationSolver
{
    private readonly ExpressionSimplifier _simplifier = new();

    /// <summary>
    /// Solves a linear equation for a single variable
    /// For example: "2x + 3 = 11" solves to x = 4
    /// </summary>
    public EquationSolution Solve(Equation equation, string variable)
    {
        var steps = new List<string>();
        steps.Add($"Original equation: {equation}");

        // Simplify both sides
        var leftSimplified = _simplifier.Simplify(equation.LeftSide);
        var rightSimplified = _simplifier.Simplify(equation.RightSide);
        
        if (leftSimplified.ToString() != equation.LeftSide.ToString() || 
            rightSimplified.ToString() != equation.RightSide.ToString())
        {
            steps.Add($"Simplify: {leftSimplified} = {rightSimplified}");
        }

        // Move all variable terms to the left, constants to the right
        decimal variableCoefficient = 0;
        decimal leftConstant = 0;
        decimal rightConstant = 0;

        foreach (var term in leftSimplified.Terms)
        {
            if (term.Variable == variable)
                variableCoefficient += term.Coefficient;
            else if (term.IsConstant)
                leftConstant += term.Coefficient;
        }

        foreach (var term in rightSimplified.Terms)
        {
            if (term.Variable == variable)
                variableCoefficient -= term.Coefficient; // Move to left (subtract)
            else if (term.IsConstant)
                rightConstant += term.Coefficient;
        }

        // Now we have: variableCoefficient * x + leftConstant = rightConstant
        // Move leftConstant to right side
        decimal finalConstant = rightConstant - leftConstant;

        if (leftConstant != 0)
        {
            string operation = leftConstant > 0 ? "Subtract" : "Add";
            steps.Add($"{operation} {Math.Abs(leftConstant)} from both sides: {variableCoefficient}{variable} = {finalConstant}");
        }

        // Check if we have a valid equation
        if (variableCoefficient == 0)
        {
            if (finalConstant == 0)
                throw new InvalidOperationException("Equation has infinite solutions");
            else
                throw new InvalidOperationException("Equation has no solution");
        }

        // Solve for variable: x = finalConstant / variableCoefficient
        decimal solution = finalConstant / variableCoefficient;

        if (variableCoefficient != 1)
        {
            steps.Add($"Divide both sides by {variableCoefficient}: {variable} = {solution}");
        }

        steps.Add($"Solution: {variable} = {solution}");

        return new EquationSolution(variable, solution, steps);
    }
}

/// <summary>
/// Represents the solution to an equation
/// </summary>
public class EquationSolution
{
    public string Variable { get; set; }
    public decimal Value { get; set; }
    public List<string> Steps { get; set; }

    public EquationSolution(string variable, decimal value, List<string> steps)
    {
        Variable = variable;
        Value = value;
        Steps = steps;
    }

    public override string ToString()
    {
        return $"{Variable} = {Value}";
    }
}

