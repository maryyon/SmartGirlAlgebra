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

            if (isCorrect)
            {
                var correctText = new TextBlock
                {
                    Text = "✓ Perfect! That's Correct! 🎉",
                    FontSize = 36,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.LimeGreen,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 30)
                };
                panel.Children.Add(correctText);
            }
            else
            {
                var incorrectText = new TextBlock
                {
                    Text = "✗ Not Quite Right",
                    FontSize = 36,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Red,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                panel.Children.Add(incorrectText);

                var answerText = new TextBlock
                {
                    Text = $"The correct answer is: {question.Answer}",
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                panel.Children.Add(answerText);

                var explanationText = new TextBlock
                {
                    Text = question.Explanation,
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 30),
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 600
                };
                panel.Children.Add(explanationText);
            }

            var nextButton = new Button
            {
                Content = currentQuestionIndex < currentQuestions.Count - 1 ? "➡ Next Question" : "🏁 See Results",
                Style = (Style)FindResource("CheerButton"),
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            nextButton.Click += NextQuestion_Click;
            panel.Children.Add(nextButton);

            GamePanel.Children.Add(panel);
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
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
            string resultTitle = percentage >= 80 ? "🌟 OUTSTANDING! 🌟" :
                                percentage >= 60 ? "👍 GOOD JOB! 👍" :
                                "💪 KEEP PRACTICING! 💪";
            var resultColor = percentage >= 80 ? Brushes.LimeGreen :
                             percentage >= 60 ? Brushes.Orange : Brushes.Red;

            var titleText = new TextBlock
            {
                Text = resultTitle,
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                Foreground = resultColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            panel.Children.Add(titleText);

            var scoreText = new TextBlock
            {
                Text = $"Score: {correctAnswers}/{currentQuestions.Count} ({percentage:F0}%)",
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };
            panel.Children.Add(scoreText);

            if (percentage >= 80)
            {
                player!.LevelUp();
                UpdateStats();
                var levelUpText = new TextBlock
                {
                    Text = $"🎉 LEVEL UP! You're now level {player.Level}! 🎉",
                    FontSize = 24,
                    Foreground = (SolidColorBrush)FindResource("PrimaryPink"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                panel.Children.Add(levelUpText);
            }

            if (currentActivity == "Cheer Competition" && correctAnswers >= 4)
            {
                int bonusPomPoms = random.Next(1, 4);
                player!.AddPomPoms(bonusPomPoms);
                UpdateStats();
                var bonusText = new TextBlock
                {
                    Text = $"🎀 Bonus! You earned {bonusPomPoms} Pom-Pom(s)! 🎀",
                    FontSize = 20,
                    Foreground = (SolidColorBrush)FindResource("PrimaryPink"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                panel.Children.Add(bonusText);
            }

            var backButton = new Button
            {
                Content = "🏠 Back to Main Menu",
                Style = (Style)FindResource("CheerButton"),
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            backButton.Click += BackToMenu_Click;
            panel.Children.Add(backButton);

            GamePanel.Children.Add(panel);
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            GamePanel.Visibility = Visibility.Collapsed;
            MainMenuPanel.Visibility = Visibility.Visible;
        }

        private List<AlgebraQuestion> GenerateLinearEquations(int count)
        {
            var questions = new List<AlgebraQuestion>();
            string[] scenarios = {
                "glitter bottles|glitter|spirit posters",
                "team shirts|shirts|uniform distribution",
                "practice mats|mats|setting up the gym",
                "music tracks|tracks|creating the playlist",
                "trophy cases|trophies|awards display"
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

                string question = $"🎯 SQUAD PLANNING!\n\n" +
                                $"You're organizing {activity}! You need {coefficient} {shortItem} per person, " +
                                $"plus {constant} extras for backup.\n\n" +
                                $"You have {result} total {shortItem}.\n\n" +
                                $"How many people can you prepare for?\n\n" +
                                $"Solve: {coefficient}x + {constant} = {result}";

                string explanation = $"The algebra: {coefficient}x = {result} - {constant} = {result - constant}, " +
                                   $"so x = {result - constant}/{coefficient} = {answer}\n\n" +
                                   $"You can prepare for {answer} people! Perfect! 🎉";

                questions.Add(new AlgebraQuestion(question, answer, explanation));
            }
            return questions;
        }

        private List<AlgebraQuestion> GenerateExpressions(int count)
        {
            var questions = new List<AlgebraQuestion>();
            string[] performances = {
                "basket tosses|tosses",
                "pyramid levels|levels",
                "formation changes|changes",
                "synchronized jumps|jumps",
                "spirit fingers|moves"
            };

            for (int i = 0; i < count; i++)
            {
                int multiplier = random.Next(2, 6);
                int addend = random.Next(1, 8);
                int xValue = random.Next(1, 8);
                int answer = multiplier * (xValue + addend);

                string[] perfParts = performances[random.Next(performances.Length)].Split('|');
                string performance = perfParts[0];
                string shortName = perfParts[1];

                string question = $"⭐ PERFORMANCE PLANNING!\n\n" +
                                $"Your routine includes {performance}!\n\n" +
                                $"Each set has (x + {addend}) {shortName}, and you do {multiplier} sets.\n\n" +
                                $"If x = {xValue}, how many total {shortName}?\n\n" +
                                $"Evaluate: {multiplier}(x + {addend}) when x = {xValue}";

                string explanation = $"The algebra: {multiplier}({xValue} + {addend}) = {multiplier} × {xValue + addend} = {answer}\n\n" +
                                   $"You'll perform {answer} total {shortName}! Spectacular! ✨";

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
                        string[] supplies1 = { "spirit signs", "banners", "posters", "flags", "streamers" };
                        string supply1 = supplies1[random.Next(supplies1.Length)];

                        questions.Add(new AlgebraQuestion(
                            $"🎨 DECORATION TIME!\n\n" +
                            $"You're decorating for the pep rally!\n\n" +
                            $"You have {a} {supply1} already hung up, plus {b} packs with {c} {supply1} each to add.\n\n" +
                            $"How many {supply1} total will you have?\n\n" +
                            $"Calculate: {a} + {b} × {c}",
                            a + b * c,
                            $"Order of operations! Multiply first: {b} × {c} = {b * c}, then add: {a} + {b * c} = {a + b * c}\n\n" +
                            $"You'll have {a + b * c} {supply1} total! The gym will look amazing! 🎊"
                        ));
                    }
                    else if (problemType == 2)
                    {
                        string[] groups2 = { "squads", "teams", "groups", "sections", "divisions" };
                        string group2 = groups2[random.Next(groups2.Length)];

                        questions.Add(new AlgebraQuestion(
                            $"👯 COMPETITION ORGANIZATION!\n\n" +
                            $"You're organizing cheerleaders into {group2}!\n\n" +
                            $"You have {a} cheerleaders from varsity and {b} from JV.\n\n" +
                            $"Each {group2.TrimEnd('s')} needs {c} people.\n\n" +
                            $"How many total cheerleaders do you have?\n\n" +
                            $"Calculate: ({a} + {b}) × {c}",
                            (a + b) * c,
                            $"Parentheses first! ({a} + {b}) = {a + b}, then multiply: {a + b} × {c} = {(a + b) * c}\n\n" +
                            $"Wait, that's if each group has that many! You have {a + b} cheerleaders total! 💪"
                        ));
                    }
                    else
                    {
                        string[] awards3 = { "medals", "certificates", "trophies", "ribbons", "plaques" };
                        string award3 = awards3[random.Next(awards3.Length)];

                        questions.Add(new AlgebraQuestion(
                            $"🏅 AWARDS CEREMONY!\n\n" +
                            $"You're distributing {award3} to winners!\n\n" +
                            $"You have {a * c} {award3} to split equally among {c} teams, plus {b} special {award3}.\n\n" +
                            $"How many {award3} does each team get?\n\n" +
                            $"Calculate: {a * c} ÷ {c} + {b}",
                            a + b,
                            $"Divide first: {a * c} ÷ {c} = {a}, then add the special ones: {a} + {b} = {a + b}\n\n" +
                            $"Each team gets {a + b} {award3}! Everyone's a winner! 🌟"
                        ));
                    }
                }
            }
            return questions;
        }
    }
}