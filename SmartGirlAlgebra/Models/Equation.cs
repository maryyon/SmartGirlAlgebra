namespace SmartGirlAlgebra.Models;

/// <summary>
/// Represents an algebraic equation with left and right sides (e.g., "2x + 3 = 11")
/// </summary>
public class Equation
{
    /// <summary>
    /// The left side of the equation
    /// </summary>
    public Expression LeftSide { get; set; }

    /// <summary>
    /// The right side of the equation
    /// </summary>
    public Expression RightSide { get; set; }

    public Equation(Expression leftSide, Expression rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide;
    }

    public override string ToString()
    {
        return $"{LeftSide} = {RightSide}";
    }
}

