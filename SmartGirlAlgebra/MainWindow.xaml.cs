using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartGirlAlgebra
{
    public partial class MainWindow : Window
    {
        private Player? player;
        private Random random = new Random();
        private List<AlgebraQuestion> currentQuestions = new List<AlgebraQuestion>();
        private int currentQuestionIndex = 0;
        private int correctAnswers = 0;
        private string currentActivity = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            string playerName = PlayerNameInput.Text.Trim();
            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter your name!", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            player = new Player(playerName);
            UpdateStats();

            WelcomePanel.Visibility = Visibility.Collapsed;
            MainMenuPanel.Visibility = Visibility.Visible;
        }

        private void UpdateStats()
        {
            if (player != null)
            {
                PlayerNameText.Text = player.Name;
                LevelText.Text = player.Level.ToString();
                PointsText.Text = player.Points.ToString();
                PomPomsText.Text = player.PomPoms.ToString();
            }
        }

        private void PracticeRoutine_Click(object sender, RoutedEventArgs e)
        {
            currentActivity = "Practice Routine";
            StartActivity("📚 Practice Routine - Solve Equations", GenerateLinearEquations(3));
        }

        private void PerformStunts_Click(object sender, RoutedEventArgs e)
        {
            currentActivity = "Perform Stunts";
            StartActivity("🤸 Perform Stunts - Simplify Expressions", GenerateExpressions(3));
        }

        private void CheerCompetition_Click(object sender, RoutedEventArgs e)
        {
            currentActivity = "Cheer Competition";
            StartActivity("🎯 Cheer Competition - Mixed Challenges", GenerateMixedQuestions(5));
        }

        private void StartActivity(string title, List<AlgebraQuestion> questions)
        {
            currentQuestions = questions;
            currentQuestionIndex = 0;
            correctAnswers = 0;

            MainMenuPanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Visible;
            GamePanel.Children.Clear();

            ShowQuestion();
        }

        private void ShowQuestion()
        {
            GamePanel.Children.Clear();

            var question = currentQuestions[currentQuestionIndex];

            var panel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

            // Title
            var titleText = new TextBlock
            {
                Text = currentActivity,
                FontSize = 32,
                FontWeight = FontWeights.Bold,
                Foreground = (SolidColorBrush)FindResource("PrimaryPink"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };
            panel.Children.Add(titleText);

            // Progress
            var progressText = new TextBlock
            {
                Text = $"Question {currentQuestionIndex + 1} of {currentQuestions.Count}",
                FontSize = 18,
                Foreground = (SolidColorBrush)FindResource("SecondaryPurple"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            panel.Children.Add(progressText);

            // Question
            var questionText = new TextBlock
            {
                Text = question.Question,
                FontSize = 24,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30),
                TextWrapping = TextWrapping.Wrap
            };
            panel.Children.Add(questionText);

            // Answer input
            var answerBox = new TextBox
            {
                Name = "AnswerBox",
                Width = 200,
                FontSize = 20,
                Padding = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            panel.Children.Add(answerBox);

            // Submit button
            var submitButton = new Button
            {
                Content = "✓ Submit Answer",
                Style = (Style)FindResource("CheerButton"),
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            submitButton.Click += (s, e) => CheckAnswer(answerBox.Text, question);
            panel.Children.Add(submitButton);

            GamePanel.Children.Add(panel);
            answerBox.Focus();
        }

        private void CheckAnswer(string userAnswer, AlgebraQuestion question)
        {
            if (int.TryParse(userAnswer, out int answer))
            {
                bool isCorrect = answer == question.Answer;

                if (isCorrect)
                {
                    correctAnswers++;
                    int points = currentActivity == "Cheer Competition" ? 20 : (currentActivity == "Perform Stunts" ? 15 : 10);
                    player!.AddPoints(points);
                    UpdateStats();
                }

                ShowFeedback(isCorrect, question);
            }
            else
            {
                MessageBox.Show("Please enter a valid number!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowFeedback(bool isCorrect, AlgebraQuestion question)
        {
            GamePanel.Children.Clear();

            var panel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

            // Feedback icon and message
            var feedbackText = new TextBlock
            {
                Text = isCorrect ? "✓ Perfect! That's Correct! 🎉" : "✗ Not Quite Right",
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                Foreground = isCorrect ? Brushes.Green : Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            panel.Children.Add(feedbackText);

            if (!isCorrect)
            {
                var correctAnswerText = new TextBlock
                {
                    Text = $"The correct answer is: {question.Answer}",
                    FontSize = 22,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = (SolidColorBrush)FindResource("SecondaryPurple"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                panel.Children.Add(correctAnswerText);

                var explanationText = new TextBlock
                {
                    Text = question.Explanation,
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(20, 0, 20, 30),
                    TextWrapping = TextWrapping.Wrap
                };
                panel.Children.Add(explanationText);
            }

            // Next button
            var nextButton = new Button
            {
                Content = currentQuestionIndex < currentQuestions.Count - 1 ? "➡ Next Question" : "🏁 See Results",
                Style = (Style)FindResource("CheerButton"),
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            nextButton.Click += (s, e) => NextQuestion();
            panel.Children.Add(nextButton);

            GamePanel.Children.Add(panel);
        }

        private void NextQuestion()
        {
            currentQuestionIndex++;

            if (currentQuestionIndex < currentQuestions.Count)
            {
                ShowQuestion();
            }
            else
            {
                ShowResults();
            }
        }

        private void ShowResults()
        {
            GamePanel.Children.Clear();

            var panel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

            double percentage = (double)correctAnswers / currentQuestions.Count * 100;

            // Results title
            string resultTitle = percentage >= 80 ? "🌟 OUTSTANDING! 🌟" :
                                percentage >= 60 ? "👍 GOOD JOB! 👍" :
                                "💪 KEEP PRACTICING! 💪";

            var titleText = new TextBlock
            {
                Text = resultTitle,
                FontSize = 40,
                FontWeight = FontWeights.Bold,
                Foreground = percentage >= 80 ? Brushes.Green :
                            percentage >= 60 ? Brushes.Orange : Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };
            panel.Children.Add(titleText);

            // Score
            var scoreText = new TextBlock
            {
                Text = $"Score: {correctAnswers}/{currentQuestions.Count} ({percentage:F0}%)",
                FontSize = 28,
                FontWeight = FontWeights.SemiBold,
                Foreground = (SolidColorBrush)FindResource("SecondaryPurple"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            panel.Children.Add(scoreText);

            // Level up if good performance
            if (percentage >= 80)
            {
                player!.LevelUp();
                var levelUpText = new TextBlock
                {
                    Text = $"🎉 LEVEL UP! You're now level {player.Level}! 🎉",
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Foreground = (SolidColorBrush)FindResource("PrimaryPink"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                panel.Children.Add(levelUpText);
            }

            // Bonus pom-poms for competition
            if (currentActivity == "Cheer Competition" && correctAnswers >= 4)
            {
                int pomPoms = random.Next(1, 4);
                player!.AddPomPoms(pomPoms);
                var bonusText = new TextBlock
                {
                    Text = $"🎀 Bonus! You earned {pomPoms} Pom-Pom(s)! 🎀",
                    FontSize = 22,
                    FontWeight = FontWeights.Bold,
                    Foreground = (SolidColorBrush)FindResource("PrimaryPink"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                panel.Children.Add(bonusText);
            }

            UpdateStats();

            // Back to menu button
            var backButton = new Button
            {
                Content = "🏠 Back to Main Menu",
                Style = (Style)FindResource("CheerButton"),
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            backButton.Click += (s, e) => BackToMainMenu();
            panel.Children.Add(backButton);

            GamePanel.Children.Add(panel);
        }


        private void BackToMainMenu()
        {
            GamePanel.Visibility = Visibility.Collapsed;
            MainMenuPanel.Visibility = Visibility.Visible;
        }

        private void PomPomShop_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"You have {player!.PomPoms} Pom-Poms!\n\n" +
                "🎓 Algebra Hint (5 Pom-Poms)\n" +
                "⭐ Level Boost (10 Pom-Poms)\n" +
                "💎 +100 Points (3 Pom-Poms)\n\n" +
                "Choose an item to buy:",
                "🏪 Pom-Pom Shop",
                MessageBoxButton.OK);
        }

        private void ViewProgress_Click(object sender, RoutedEventArgs e)
        {
            string rank = player!.Level switch
            {
                >= 10 => "🏆 Elite Captain",
                >= 7 => "⭐ Star Performer",
                >= 5 => "💫 Rising Star",
                >= 3 => "🌟 Squad Member",
                _ => "🎀 Rookie Cheerleader"
            };

            MessageBox.Show(
                $"📊 YOUR CHEERLEADING JOURNEY 📊\n\n" +
                $"Cheerleader: {player.Name}\n" +
                $"Level: {player.Level}\n" +
                $"Points: {player.Points}\n" +
                $"Pom-Poms: {player.PomPoms}\n\n" +
                $"Rank: {rank}\n\n" +
                "Keep solving algebra problems to level up!",
                "Your Progress",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // Question Generators
        private List<AlgebraQuestion> GenerateLinearEquations(int count)
        {
            var questions = new List<AlgebraQuestion>();
            string[] scenarios = {
                "ribbon rolls|ribbons|decorating the gym",
                "pompom sets|pompoms|making spirit gear",
                "megaphone stickers|stickers|customizing megaphones",
                "team bracelets|bracelets|the squad bonding activity",
                "cheer bows|bows|competition day prep"
            };

            for (int i = 0; i < count; i++)
            {
                int coefficient = random.Next(2, 6);
                int constant = random.Next(1, 10);
                int answer = random.Next(1, 10);
                int result = coefficient * answer + constant;

                string[] scenarioParts = scenarios[random.Next(scenarios.Length)].Split('|');
                string item = scenarioParts[0];
                string shortItem = scenarioParts[1];
                string activity = scenarioParts[2];

                string question = $"🎀 CHEER PREP TIME!\n\n" +
                                $"You're helping with {activity}! Each cheerleader needs {coefficient} {shortItem}, " +
                                $"plus you already have {constant} extras in the supply closet.\n\n" +
                                $"You counted {result} total {shortItem}.\n\n" +
                                $"How many cheerleaders are you preparing for?\n\n" +
                                $"Solve: {coefficient}x + {constant} = {result}";

                string explanation = $"The algebra: {coefficient}x = {result} - {constant} = {result - constant}, " +
                                   $"so x = {result - constant}/{coefficient} = {answer}\n\n" +
                                   $"You're preparing for {answer} cheerleaders! 📣";

                questions.Add(new AlgebraQuestion(question, answer, explanation));
            }
            return questions;
        }

        private List<AlgebraQuestion> GenerateExpressions(int count)
        {
            var questions = new List<AlgebraQuestion>();
            string[] routines = {
                "tumbling passes|tumbles",
                "dance sequences|dance moves",
                "jump combinations|jumps",
                "cheer chants|chants",
                "stunt transitions|stunts"
            };

            for (int i = 0; i < count; i++)
            {
                int multiplier = random.Next(2, 6);
                int addend = random.Next(1, 8);
                int xValue = random.Next(1, 8);
                int answer = multiplier * (xValue + addend);

                string[] routineParts = routines[random.Next(routines.Length)].Split('|');
                string routine = routineParts[0];
                string shortName = routineParts[1];

                string question = $"📊 ROUTINE SCORING!\n\n" +
                                $"At competition, your {routine} are being scored!\n\n" +
                                $"Each section has (x + {addend}) {shortName}, and you perform {multiplier} sections.\n\n" +
                                $"If x = {xValue}, how many total {shortName} did you perform?\n\n" +
                                $"Evaluate: {multiplier}(x + {addend}) when x = {xValue}";

                string explanation = $"The algebra: {multiplier}({xValue} + {addend}) = {multiplier} × {xValue + addend} = {answer}\n\n" +
                                   $"You performed {answer} total {shortName}! Amazing! 🌟";

                questions.Add(new AlgebraQuestion(question, answer, explanation));
            }
            return questions;
        }

        private List<AlgebraQuestion> GenerateMixedQuestions(int count)
        {
            var questions = new List<AlgebraQuestion>();
            for (int i = 0; i < count; i++)
            {
                int type = random.Next(1, 4);
                if (type == 1)
                {
                    questions.AddRange(GenerateLinearEquations(1));
                }
                else if (type == 2)
                {
                    questions.AddRange(GenerateExpressions(1));
                }
                else
                {
                    int a = random.Next(1, 6);
                    int b = random.Next(2, 6);
                    int c = random.Next(2, 6);

                    int problemType = random.Next(1, 4);

                    if (problemType == 1)
                    {
                        string[] items1 = { "pompoms", "megaphones", "spirit flags", "water bottles", "hair bows" };
                        string item1 = items1[random.Next(items1.Length)];

                        questions.Add(new AlgebraQuestion(
                            $"🎒 EQUIPMENT COUNT!\n\n" +
                            $"You're organizing cheer equipment!\n\n" +
                            $"You have {a} {item1} in your bag, plus {b} boxes with {c} {item1} each.\n\n" +
                            $"How many {item1} total?\n\n" +
                            $"Calculate: {a} + {b} × {c}",
                            a + b * c,
                            $"Order of operations! Multiply first: {b} × {c} = {b * c}, then add: {a} + {b * c} = {a + b * c}\n\n" +
                            $"You have {a + b * c} {item1} total! 📣"
                        ));
                    }
                    else if (problemType == 2)
                    {
                        string[] items2 = { "cheerleaders", "team members", "squad captains", "dancers", "tumblers" };
                        string item2 = items2[random.Next(items2.Length)];

                        questions.Add(new AlgebraQuestion(
                            $"👥 TEAM GROUPING!\n\n" +
                            $"You're organizing {item2} into groups for practice!\n\n" +
                            $"You have {a} {item2} from one school and {b} from another.\n\n" +
                            $"You need to split them into {c} equal practice groups.\n\n" +
                            $"How many total {item2}?\n\n" +
                            $"Calculate: ({a} + {b}) × {c}",
                            (a + b) * c,
                            $"Parentheses first! ({a} + {b}) = {a + b}, then multiply: {a + b} × {c} = {(a + b) * c}\n\n" +
                            $"Wait, that's the total if each group has that many! You have {a + b} {item2}! 🌟"
                        ));
                    }
                    else
                    {
                        string[] items3 = { "ribbons", "stickers", "pins", "badges", "wristbands" };
                        string item3 = items3[random.Next(items3.Length)];

                        questions.Add(new AlgebraQuestion(
                            $"🎁 PRIZE DISTRIBUTION!\n\n" +
                            $"You're handing out {item3} as prizes!\n\n" +
                            $"You have {a * c} {item3} to split equally among {c} winners, plus {b} bonus {item3}.\n\n" +
                            $"How many {item3} does each winner get?\n\n" +
                            $"Calculate: {a * c} ÷ {c} + {b}",
                            a + b,
                            $"Divide first: {a * c} ÷ {c} = {a}, then add the bonus: {a} + {b} = {a + b}\n\n" +
                            $"Each winner gets {a + b} {item3}! 🏆"
                        ));
                    }
                }
            }
            return questions;
        }
    }

    // Data Models
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Points { get; private set; }
        public int PomPoms { get; set; }

        public Player(string name)
        {
            Name = name;
            Level = 1;
            Points = 0;
            PomPoms = 0;
        }

        public void AddPoints(int points)
        {
            Points += points;
        }

        public void AddPomPoms(int pomPoms)
        {
            PomPoms += pomPoms;
        }

        public void LevelUp()
        {
            Level++;
        }
    }

    public class AlgebraQuestion
    {
        public string Question { get; set; }
        public int Answer { get; set; }
        public string Explanation { get; set; }

        public AlgebraQuestion(string question, int answer, string explanation)
        {
            Question = question;
            Answer = answer;
            Explanation = explanation;
        }
    }
}
