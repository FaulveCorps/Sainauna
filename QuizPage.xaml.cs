// QuizPage.xaml.cs
// History quiz with SYMBOL-ONLY images for character questions - VINTAGE THEME

namespace Sainauna;

public partial class QuizPage : ContentPage
{
    // Question class - supports both text and image questions
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public string? QuestionImage { get; set; }  // For symbol questions
        public List<string> OriginalOptions { get; set; } = new();
        public List<string>? ImageOptions { get; set; }  // For symbol answer options
        public int CorrectAnswerIndex { get; set; }
        public List<string> ShuffledOptions { get; set; } = new();
        public int ShuffledCorrectIndex { get; set; }
        public bool IsSymbolQuestion { get; set; }
    }

    // List of all questions
    private List<Question> _questions = new();

    // Current question index
    private int _currentQuestionIndex = 0;

    // Score tracking
    private int _correctAnswers = 0;

    // Random for shuffling
    private Random _random = new();

    public QuizPage()
    {
        InitializeComponent();
        InitializeQuestions();
        ShuffleQuestions();
        ShowCurrentQuestion();
    }

    // Initialize all quiz questions - MIX of history and symbol questions
    private void InitializeQuestions()
    {
        _questions = new List<Question>
        {
            // HISTORY QUESTIONS (text-based)
            new Question
            {
                QuestionText = "What is Baybayin?",
                OriginalOptions = new List<string>
                {
                    "An ancient Philippine writing system",
                    "A type of Filipino food",
                    "A traditional dance",
                    "A musical instrument"
                },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = false
            },
            new Question
            {
                QuestionText = "When was Baybayin commonly used in the Philippines?",
                OriginalOptions = new List<string>
                {
                    "Before Spanish colonization",
                    "During World War II",
                    "In the 21st century",
                    "During American occupation"
                },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = false
            },
            new Question
            {
                QuestionText = "What does the word 'Baybayin' mean?",
                OriginalOptions = new List<string>
                {
                    "To spell or write",
                    "To read books",
                    "To speak loudly",
                    "To draw pictures"
                },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = false
            },
            // SYMBOL QUESTIONS - Show symbol, ask for pronunciation
            new Question
            {
                QuestionText = "What sound does this character make?",
                QuestionImage = "ka.png",
                OriginalOptions = new List<string> { "ka", "ca", "qa", "ga" },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = true
            },
            new Question
            {
                QuestionText = "What sound does this character make?",
                QuestionImage = "ba.png",
                OriginalOptions = new List<string> { "ba", "pa", "va", "fa" },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = true
            },
            new Question
            {
                QuestionText = "What is a 'kudlit' in Baybayin?",
                OriginalOptions = new List<string>
                {
                    "A small mark to change vowel sounds",
                    "A type of pen",
                    "A writing surface",
                    "A special paper"
                },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = false
            },
            // MORE SYMBOL QUESTIONS
            new Question
            {
                QuestionText = "What sound does this character make?",
                QuestionImage = "nga.png",
                OriginalOptions = new List<string> { "nga", "na", "naga", "ang" },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = true
            },
            new Question
            {
                QuestionText = "How many basic characters are in Baybayin?",
                OriginalOptions = new List<string> { "17", "26", "10", "50" },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = false
            },
            new Question
            {
                QuestionText = "What sound does this character make?",
                QuestionImage = "sa.png",
                OriginalOptions = new List<string> { "sa", "sha", "za", "ca" },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = true
            },
            new Question
            {
                QuestionText = "What happened to Baybayin during Spanish colonization?",
                OriginalOptions = new List<string>
                {
                    "It declined as Latin alphabet was introduced",
                    "It became more popular",
                    "It was declared the national script",
                    "Nothing changed"
                },
                CorrectAnswerIndex = 0,
                IsSymbolQuestion = false
            }
        };
    }

    // Shuffle questions randomly AND shuffle options for each question
    private void ShuffleQuestions()
    {
        _questions = _questions.OrderBy(x => _random.Next()).ToList();

        foreach (var question in _questions)
        {
            ShuffleQuestionOptions(question);
        }

        _currentQuestionIndex = 0;
        _correctAnswers = 0;
    }

    // Shuffle options for a single question
    private void ShuffleQuestionOptions(Question question)
    {
        string correctAnswer = question.OriginalOptions[question.CorrectAnswerIndex];

        var shuffled = question.OriginalOptions.Select((option, index) => new { Option = option, Index = index })
                                               .OrderBy(x => _random.Next())
                                               .ToList();

        question.ShuffledOptions = shuffled.Select(x => x.Option).ToList();
        question.ShuffledCorrectIndex = shuffled.FindIndex(x => x.Index == question.CorrectAnswerIndex);
    }

    // Show current question
    private void ShowCurrentQuestion()
    {
        if (_currentQuestionIndex < _questions.Count)
        {
            var question = _questions[_currentQuestionIndex];

            QuestionLabel.Text = question.QuestionText;
            ProgressLabel.Text = $"Question {_currentQuestionIndex + 1} of {_questions.Count}";

            // Handle symbol image display
            if (question.IsSymbolQuestion && !string.IsNullOrEmpty(question.QuestionImage))
            {
                QuestionSymbolImage.Source = question.QuestionImage;
                QuestionSymbolImage.IsVisible = true;
            }
            else
            {
                QuestionSymbolImage.IsVisible = false;
            }

            // Set answer button texts from SHUFFLED options
            Answer1Button.Text = question.ShuffledOptions[0];
            Answer2Button.Text = question.ShuffledOptions[1];
            Answer3Button.Text = question.ShuffledOptions[2];
            Answer4Button.Text = question.ShuffledOptions[3];

            // Reset button colors and enable them
            ResetAnswerButtons();

            // Hide result and next button
            ResultLabel.Text = "";
            NextButton.IsVisible = false;
        }
        else
        {
            QuizFinished();
        }
    }

    // Reset answer buttons
    private void ResetAnswerButtons()
    {
        Answer1Button.BackgroundColor = Color.FromArgb("#E8DCC8");
        Answer2Button.BackgroundColor = Color.FromArgb("#E8DCC8");
        Answer3Button.BackgroundColor = Color.FromArgb("#E8DCC8");
        Answer4Button.BackgroundColor = Color.FromArgb("#E8DCC8");

        Answer1Button.TextColor = Color.FromArgb("#654321");
        Answer2Button.TextColor = Color.FromArgb("#654321");
        Answer3Button.TextColor = Color.FromArgb("#654321");
        Answer4Button.TextColor = Color.FromArgb("#654321");

        Answer1Button.IsEnabled = true;
        Answer2Button.IsEnabled = true;
        Answer3Button.IsEnabled = true;
        Answer4Button.IsEnabled = true;
    }

    // Answer button clicked
    private void OnAnswerClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        int selectedIndex = -1;

        if (button == Answer1Button) selectedIndex = 0;
        else if (button == Answer2Button) selectedIndex = 1;
        else if (button == Answer3Button) selectedIndex = 2;
        else if (button == Answer4Button) selectedIndex = 3;

        var currentQuestion = _questions[_currentQuestionIndex];
        bool isCorrect = (selectedIndex == currentQuestion.ShuffledCorrectIndex);

        Answer1Button.IsEnabled = false;
        Answer2Button.IsEnabled = false;
        Answer3Button.IsEnabled = false;
        Answer4Button.IsEnabled = false;

        if (isCorrect)
        {
            _correctAnswers++;
            button.BackgroundColor = Color.FromArgb("#90EE90");
            button.TextColor = Color.FromArgb("#006400");
            ResultLabel.Text = "Correct!";
            ResultLabel.TextColor = Color.FromArgb("#6B8E23");
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("#FFB6C1");
            button.TextColor = Color.FromArgb("#8B0000");
            ResultLabel.Text = "Wrong!";
            ResultLabel.TextColor = Color.FromArgb("#B22222");
            HighlightCorrectAnswer(currentQuestion.ShuffledCorrectIndex);
        }

        UpdateScore();

        if (_currentQuestionIndex < _questions.Count - 1)
        {
            NextButton.IsVisible = true;
        }
        else
        {
            NextButton.IsVisible = false;
            RetakeButton.IsVisible = true;
        }
    }

    private void HighlightCorrectAnswer(int correctIndex)
    {
        switch (correctIndex)
        {
            case 0:
                Answer1Button.BackgroundColor = Color.FromArgb("#90EE90");
                Answer1Button.TextColor = Color.FromArgb("#006400");
                break;
            case 1:
                Answer2Button.BackgroundColor = Color.FromArgb("#90EE90");
                Answer2Button.TextColor = Color.FromArgb("#006400");
                break;
            case 2:
                Answer3Button.BackgroundColor = Color.FromArgb("#90EE90");
                Answer3Button.TextColor = Color.FromArgb("#006400");
                break;
            case 3:
                Answer4Button.BackgroundColor = Color.FromArgb("#90EE90");
                Answer4Button.TextColor = Color.FromArgb("#006400");
                break;
        }
    }

    private void UpdateScore()
    {
        ScoreLabel.Text = $"Score: {_correctAnswers}/{_questions.Count}";
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        _currentQuestionIndex++;
        ShowCurrentQuestion();
    }

    private void QuizFinished()
    {
        QuestionLabel.Text = "Quiz Complete!";
        QuestionSymbolImage.IsVisible = false;
        ProgressLabel.Text = "";
        ResultLabel.Text = $"You scored {_correctAnswers} out of {_questions.Count}!";
        ResultLabel.TextColor = Color.FromArgb("#654321");

        Answer1Button.IsVisible = false;
        Answer2Button.IsVisible = false;
        Answer3Button.IsVisible = false;
        Answer4Button.IsVisible = false;

        RetakeButton.IsVisible = true;

        int score = (int)((_correctAnswers / (double)_questions.Count) * 100);
        ProgressService.UpdateQuizScore(score);
    }

    private void OnRetakeClicked(object sender, EventArgs e)
    {
        Answer1Button.IsVisible = true;
        Answer2Button.IsVisible = true;
        Answer3Button.IsVisible = true;
        Answer4Button.IsVisible = true;

        RetakeButton.IsVisible = false;

        ShuffleQuestions();
        ShowCurrentQuestion();
    }
}
