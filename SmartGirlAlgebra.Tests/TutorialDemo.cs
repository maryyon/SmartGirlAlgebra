using Xunit;
using Xunit.Abstractions;

namespace SmartGirlAlgebra.Tests;

/// <summary>
/// Demo of the tutorial/explanation shown before Problem 1
/// </summary>
public class TutorialDemo
{
    private readonly ITestOutputHelper _output;

    public TutorialDemo(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Demo_Problem1_Tutorial()
    {
        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║  🎀 WELCOME TO ALGEBRA SQUAD! 🎀                              ║");
        _output.WriteLine("║  Let's learn: SOLVING LINEAR EQUATIONS                        ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        _output.WriteLine("");
        
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("📖 CONCEPT: What is a Linear Equation?");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("A linear equation is like a BALANCE SCALE! ⚖️");
        _output.WriteLine("");
        _output.WriteLine("Think of it like this:");
        _output.WriteLine("  • The LEFT side has some stuff");
        _output.WriteLine("  • The RIGHT side has some stuff");
        _output.WriteLine("  • The = sign means they BALANCE (weigh the same)");
        _output.WriteLine("");
        _output.WriteLine("Example: 2x + 3 = 11");
        _output.WriteLine("         ^^^^^^   ^^");
        _output.WriteLine("         LEFT     RIGHT");
        _output.WriteLine("");
        _output.WriteLine("Your job? Find what 'x' equals to keep the scale balanced!");
        _output.WriteLine("");

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("🎯 THE GOLDEN RULE: Whatever you do to one side,");
        _output.WriteLine("                     you MUST do to the other side!");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("Why? Because we need to keep the scale BALANCED! ⚖️");
        _output.WriteLine("");
        _output.WriteLine("If you add 5 to the left side, you MUST add 5 to the right.");
        _output.WriteLine("If you subtract 3 from the left, you MUST subtract 3 from the right.");
        _output.WriteLine("If you divide the left by 2, you MUST divide the right by 2.");
        _output.WriteLine("");

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("🎬 LET'S SEE IT IN ACTION!");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("Simple example: x + 3 = 8");
        _output.WriteLine("");
        _output.WriteLine("STEP 1: What's our goal?");
        _output.WriteLine("        Get 'x' all by itself on one side!");
        _output.WriteLine("");
        _output.WriteLine("STEP 2: What's in our way?");
        _output.WriteLine("        That +3 is stuck to our x!");
        _output.WriteLine("");
        _output.WriteLine("STEP 3: How do we get rid of +3?");
        _output.WriteLine("        Subtract 3! (The opposite of adding 3)");
        _output.WriteLine("");
        _output.WriteLine("STEP 4: Do it to BOTH sides!");
        _output.WriteLine("        x + 3 - 3 = 8 - 3");
        _output.WriteLine("        x = 5");
        _output.WriteLine("");
        _output.WriteLine("🎉 BOOM! We found x = 5!");
        _output.WriteLine("");

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("💡 THE STRATEGY (Your Cheer Routine for Solving!)");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("1️⃣  SIMPLIFY both sides (combine like terms)");
        _output.WriteLine("2️⃣  GET RID OF CONSTANTS (numbers without x)");
        _output.WriteLine("    → Use addition or subtraction");
        _output.WriteLine("3️⃣  GET RID OF COEFFICIENTS (numbers multiplied by x)");
        _output.WriteLine("    → Use division or multiplication");
        _output.WriteLine("4️⃣  CHECK YOUR ANSWER (plug it back in!)");
        _output.WriteLine("");

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("🛍️ NOW LET'S APPLY IT: POM-POM SHOPPING SPREE!");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("THE STORY:");
        _output.WriteLine("You're shopping for the squad! 🎀");
        _output.WriteLine("  • Each pair of pom-poms costs $8");
        _output.WriteLine("  • You also need a team bag for $15");
        _output.WriteLine("  • Your coach gave you $55 total");
        _output.WriteLine("  • How many pairs of pom-poms can you buy?");
        _output.WriteLine("");
        
        _output.WriteLine("TRANSLATE TO ALGEBRA:");
        _output.WriteLine("  Let x = number of pom-pom pairs");
        _output.WriteLine("");
        _output.WriteLine("  Cost of pom-poms: 8x  (8 dollars × x pairs)");
        _output.WriteLine("  Cost of bag:      15  (just 15 dollars)");
        _output.WriteLine("  Total money:      55  (what coach gave you)");
        _output.WriteLine("");
        _output.WriteLine("  EQUATION: 8x + 15 = 55");
        _output.WriteLine("");

        _output.WriteLine("SOLVE IT STEP-BY-STEP:");
        _output.WriteLine("");
        _output.WriteLine("  Start:           8x + 15 = 55");
        _output.WriteLine("");
        _output.WriteLine("  Step 1: Get rid of +15");
        _output.WriteLine("          Subtract 15 from both sides:");
        _output.WriteLine("          8x + 15 - 15 = 55 - 15");
        _output.WriteLine("          8x = 40");
        _output.WriteLine("");
        _output.WriteLine("  Step 2: Get rid of the 8 multiplying x");
        _output.WriteLine("          Divide both sides by 8:");
        _output.WriteLine("          8x ÷ 8 = 40 ÷ 8");
        _output.WriteLine("          x = 5");
        _output.WriteLine("");
        _output.WriteLine("  ✨ ANSWER: You can buy 5 pairs of pom-poms! ✨");
        _output.WriteLine("");

        _output.WriteLine("CHECK YOUR WORK:");
        _output.WriteLine("  Does 8(5) + 15 = 55?");
        _output.WriteLine("  40 + 15 = 55 ✓");
        _output.WriteLine("  YES! It works! 🎉");
        _output.WriteLine("");

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("🎯 KEY TAKEAWAYS:");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("✅ Linear equations are like balance scales");
        _output.WriteLine("✅ Whatever you do to one side, do to the other");
        _output.WriteLine("✅ Goal: Get the variable (x) all by itself");
        _output.WriteLine("✅ Use OPPOSITE operations to undo things:");
        _output.WriteLine("   • Addition ↔ Subtraction");
        _output.WriteLine("   • Multiplication ↔ Division");
        _output.WriteLine("✅ Always check your answer!");
        _output.WriteLine("");

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║  🎀 YOU'RE READY! Let's solve some problems! 🎀               ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");
    }
}

