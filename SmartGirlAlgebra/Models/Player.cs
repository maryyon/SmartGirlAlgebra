namespace SmartGirlAlgebra;

public class Player
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Points { get; private set; }
    public int PomPoms { get; set; }
    public int Streak { get; set; }
    public int BestStreak { get; set; }
    public Dictionary<AlgebraTopic, TopicMastery> TopicMastery { get; set; }
    public List<string> Achievements { get; set; }
    public int HintsUsed { get; set; }
    public string AvatarStyle { get; set; }
    public CheerSquadRank SquadRank { get; set; }

    public Player(string name)
    {
        Name = name;
        Level = 1;
        Points = 0;
        PomPoms = 0;
        Streak = 0;
        BestStreak = 0;
        TopicMastery = new Dictionary<AlgebraTopic, TopicMastery>();
        Achievements = new List<string>();
        HintsUsed = 0;
        AvatarStyle = "cheerleader";
        SquadRank = CheerSquadRank.Rookie;

        // Initialize all topics
        foreach (AlgebraTopic topic in Enum.GetValues(typeof(AlgebraTopic)))
        {
            TopicMastery[topic] = new TopicMastery();
        }
    }

    public string GetRankTitle()
    {
        return SquadRank switch
        {
            CheerSquadRank.Rookie => "🎀 Rookie Cheerleader",
            CheerSquadRank.JV => "⭐ JV Squad Member",
            CheerSquadRank.Varsity => "💫 Varsity Performer",
            CheerSquadRank.Elite => "🏆 Elite Captain",
            CheerSquadRank.Championship => "👑 Championship Squad Leader",
            _ => "🎀 Rookie Cheerleader"
        };
    }

    public string GetRankEmoji()
    {
        return SquadRank switch
        {
            CheerSquadRank.Rookie => "🎀",
            CheerSquadRank.JV => "⭐",
            CheerSquadRank.Varsity => "💫",
            CheerSquadRank.Elite => "🏆",
            CheerSquadRank.Championship => "👑",
            _ => "🎀"
        };
    }

    public void AddPoints(int points)
    {
        Points += points;
        CheckLevelUp();
    }

    public void AddPomPoms(int pomPoms)
    {
        PomPoms += pomPoms;
    }

    public void IncrementStreak()
    {
        Streak++;
        if (Streak > BestStreak) BestStreak = Streak;
    }

    public void ResetStreak()
    {
        Streak = 0;
    }

    public void UpdateTopicMastery(AlgebraTopic topic, bool correct)
    {
        TopicMastery[topic].TotalAttempts++;
        if (correct) TopicMastery[topic].CorrectAnswers++;
    }

    public void LevelUp()
    {
        Level++;
        UpdateSquadRank();
    }

    private void CheckLevelUp()
    {
        int requiredPoints = Level * 100;
        if (Points >= requiredPoints)
        {
            LevelUp();
        }
    }

    private void UpdateSquadRank()
    {
        SquadRank = Level switch
        {
            >= 20 => CheerSquadRank.Championship,
            >= 15 => CheerSquadRank.Elite,
            >= 10 => CheerSquadRank.Varsity,
            >= 5 => CheerSquadRank.JV,
            _ => CheerSquadRank.Rookie
        };
    }

    public void AddAchievement(string achievement)
    {
        if (!Achievements.Contains(achievement))
        {
            Achievements.Add(achievement);
        }
    }
}

public class TopicMastery
{
    public int TotalAttempts { get; set; }
    public int CorrectAnswers { get; set; }
    public double Accuracy => TotalAttempts > 0 ? (double)CorrectAnswers / TotalAttempts * 100 : 0;
    public string MasteryLevel => Accuracy >= 90 ? "Expert" : Accuracy >= 75 ? "Proficient" : Accuracy >= 50 ? "Learning" : "Needs Practice";
}

public enum CheerSquadRank
{
    Rookie,
    JV,
    Varsity,
    Elite,
    Championship
}

public enum AlgebraTopic
{
    LinearEquations,
    MultiStepEquations,
    Inequalities,
    SystemsOfEquations,
    Exponents,
    Polynomials,
    Factoring,
    QuadraticEquations,
    Graphing,
    OrderOfOperations
}

public class AlgebraQuestion
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Explanation { get; set; }
    public AlgebraTopic Topic { get; set; }
    public int Difficulty { get; set; }
    public List<string> Hints { get; set; }
    public List<string> AcceptableAnswers { get; set; }

    public AlgebraQuestion(string question, string answer, string explanation, AlgebraTopic topic, int difficulty = 1)
    {
        Question = question;
        Answer = answer;
        Explanation = explanation;
        Topic = topic;
        Difficulty = difficulty;
        Hints = new List<string>();
        AcceptableAnswers = new List<string> { answer };
    }

    public void AddHint(string hint)
    {
        Hints.Add(hint);
    }

    public void AddAcceptableAnswer(string answer)
    {
        AcceptableAnswers.Add(answer);
    }

    public bool IsCorrect(string userAnswer)
    {
        return AcceptableAnswers.Any(a => a.Trim().Equals(userAnswer.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

