// MemorizePage.xaml.cs
// Flashcard system with SYMBOL-ONLY images - NO CHEATING!

using static Microsoft.Maui.Controls.Button;

namespace Sainauna;

public partial class MemorizePage : ContentPage
{
    // Flashcard data class
    public class Flashcard
    {
        public string ImageName { get; set; } = string.Empty;
        public string Pronunciation { get; set; } = string.Empty;
    }

    // List of all flashcards
    private List<Flashcard> _flashcards = new();

    // Current flashcard index
    private int _currentIndex = 0;

    // Score tracking
    private int _correctAnswers = 0;
    private int _totalAnswered = 0;

    // For multiple choice mode
    private string _currentCorrectImage = "";
    private Random _random = new();

    public MemorizePage()
    {
        InitializeComponent();
        InitializeFlashcards();
        ShuffleFlashcards();
        ShowCurrentFlashcard();
    }

    // Initialize all flashcard data
    private void InitializeFlashcards()
    {
        _flashcards = new List<Flashcard>
        {
            new Flashcard { ImageName = "a.png", Pronunciation = "a" },
            new Flashcard { ImageName = "e_i.png", Pronunciation = "e" },
            new Flashcard { ImageName = "o_u.png", Pronunciation = "o" },
            new Flashcard { ImageName = "ba.png", Pronunciation = "ba" },
            new Flashcard { ImageName = "ka.png", Pronunciation = "ka" },
            new Flashcard { ImageName = "da_ra.png", Pronunciation = "da" },
            new Flashcard { ImageName = "ga.png", Pronunciation = "ga" },
            new Flashcard { ImageName = "ha.png", Pronunciation = "ha" },
            new Flashcard { ImageName = "la.png", Pronunciation = "la" },
            new Flashcard { ImageName = "ma.png", Pronunciation = "ma" },
            new Flashcard { ImageName = "na.png", Pronunciation = "na" },
            new Flashcard { ImageName = "nga.png", Pronunciation = "nga" },
            new Flashcard { ImageName = "pa.png", Pronunciation = "pa" },
            new Flashcard { ImageName = "sa.png", Pronunciation = "sa" },
            new Flashcard { ImageName = "ta.png", Pronunciation = "ta" },
            new Flashcard { ImageName = "wa.png", Pronunciation = "wa" },
            new Flashcard { ImageName = "ya.png", Pronunciation = "ya" }
        };
    }

    // Shuffle the flashcards randomly
    private void ShuffleFlashcards()
    {
        _flashcards = _flashcards.OrderBy(x => _random.Next()).ToList();
        _currentIndex = 0;
        _correctAnswers = 0;
        _totalAnswered = 0;
        UpdateScore();
    }

    // Show current flashcard - SYMBOL ONLY
    private void ShowCurrentFlashcard()
    {
        if (_currentIndex < _flashcards.Count)
        {
            var card = _flashcards[_currentIndex];
            // Show symbol ONLY - no pronunciation visible
            SymbolImage.Source = card.ImageName;
            AnswerEntry.Text = "";
            ResultLabel.Text = "";
            ResultLabel.TextColor = Color.FromArgb("#654321");
            NextButton.IsVisible = false;
            AnswerEntry.IsEnabled = true;
        }
        else
        {
            // Finished all cards
            SymbolImage.Source = null;
            ResultLabel.Text = $"You got {_correctAnswers} out of {_totalAnswered} correct!";
            ResultLabel.TextColor = Color.FromArgb("#6B8E23");

            // Update progress tracker with score (max 100 points for perfect score)
            int score = (int)((_correctAnswers / (double)_totalAnswered) * 100);
            ProgressService.UpdateMemorizeScore(score);
        }
    }

    // Update score display
    private void UpdateScore()
    {
        ScoreLabel.Text = $"Score: {_correctAnswers}/{_totalAnswered}";
        Mode2ScoreLabel.Text = $"Score: {_correctAnswers}/{_totalAnswered}";
    }

    // Check answer (Mode 1)
    private void OnCheckAnswer(object sender, EventArgs e)
    {
        if (_currentIndex >= _flashcards.Count) return;

        var card = _flashcards[_currentIndex];
        string userAnswer = AnswerEntry.Text?.Trim().ToLower() ?? "";
        string correctAnswer = card.Pronunciation.ToLower();

        _totalAnswered++;

        if (userAnswer == correctAnswer)
        {
            _correctAnswers++;
            ResultLabel.Text = "Correct!";
            ResultLabel.TextColor = Color.FromArgb("#6B8E23");
        }
        else
        {
            ResultLabel.Text = $"Wrong! The answer is: {card.Pronunciation}";
            ResultLabel.TextColor = Color.FromArgb("#B22222");
        }

        UpdateScore();
        AnswerEntry.IsEnabled = false;
        NextButton.IsVisible = true;
    }

    // Next button clicked (Mode 1)
    private void OnNextClicked(object sender, EventArgs e)
    {
        _currentIndex++;
        ShowCurrentFlashcard();
    }

    // Switch to Mode 1 (Type Answer)
    private void OnMode1Clicked(object sender, EventArgs e)
    {
        Mode1Layout.IsVisible = true;
        Mode2Layout.IsVisible = false;
        Mode1Button.BackgroundColor = Color.FromArgb("#8B5A2B");
        Mode1Button.TextColor = Color.FromArgb("#FFF8DC");
        Mode2Button.BackgroundColor = Color.FromArgb("#D2B48C");
        Mode2Button.TextColor = Color.FromArgb("#654321");
        ShuffleFlashcards();
        ShowCurrentFlashcard();
    }

    // Switch to Mode 2 (Multiple Choice)
    private void OnMode2Clicked(object sender, EventArgs e)
    {
        Mode1Layout.IsVisible = false;
        Mode2Layout.IsVisible = true;
        Mode1Button.BackgroundColor = Color.FromArgb("#D2B48C");
        Mode1Button.TextColor = Color.FromArgb("#654321");
        Mode2Button.BackgroundColor = Color.FromArgb("#8B5A2B");
        Mode2Button.TextColor = Color.FromArgb("#FFF8DC");
        SetupMultipleChoiceQuestion();
    }

    // Setup multiple choice question - SYMBOLS ONLY, NO PRONUNCIATION ON BUTTONS
    private void SetupMultipleChoiceQuestion()
    {
        // Pick a random flashcard as the correct answer
        var correctCard = _flashcards[_random.Next(_flashcards.Count)];
        _currentCorrectImage = correctCard.ImageName;

        // Show the pronunciation in the question ONLY
        QuestionLabel.Text = $"Which character is '{correctCard.Pronunciation}'?";

        // Get 3 wrong answers
        var wrongAnswers = _flashcards
            .Where(f => f.ImageName != correctCard.ImageName)
            .OrderBy(x => _random.Next())
            .Take(3)
            .Select(f => f.ImageName)
            .ToList();

        // Combine and shuffle all options
        var allOptions = wrongAnswers.ToList();
        allOptions.Add(correctCard.ImageName);
        allOptions = allOptions.OrderBy(x => _random.Next()).ToList();

        // Assign SYMBOL-ONLY images to buttons - NO TEXT!
        SetButtonImageOnly(Option1Button, allOptions[0]);
        SetButtonImageOnly(Option2Button, allOptions[1]);
        SetButtonImageOnly(Option3Button, allOptions[2]);
        SetButtonImageOnly(Option4Button, allOptions[3]);

        // Store the image name in button's CommandParameter for identification
        Option1Button.CommandParameter = allOptions[0];
        Option2Button.CommandParameter = allOptions[1];
        Option3Button.CommandParameter = allOptions[2];
        Option4Button.CommandParameter = allOptions[3];

        // Reset button colors
        Option1Button.BackgroundColor = Color.FromArgb("#FFF8DC");
        Option2Button.BackgroundColor = Color.FromArgb("#FFF8DC");
        Option3Button.BackgroundColor = Color.FromArgb("#FFF8DC");
        Option4Button.BackgroundColor = Color.FromArgb("#FFF8DC");

        Mode2ResultLabel.Text = "";
        Mode2NextButton.IsVisible = false;

        // Enable all buttons
        Option1Button.IsEnabled = true;
        Option2Button.IsEnabled = true;
        Option3Button.IsEnabled = true;
        Option4Button.IsEnabled = true;
    }

    // Set button to show ONLY image, NO text (prevents cheating!)
    private void SetButtonImageOnly(Button button, string imageName)
    {
        button.ImageSource = ImageSource.FromFile(imageName);
        button.Text = "";
        button.ContentLayout = new ButtonContentLayout(ButtonContentLayout.ImagePosition.Top, 0);
    }

    // Option button clicked (Mode 2)
    private void OnOptionClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string? selectedImage = button.CommandParameter?.ToString();

        _totalAnswered++;

        // Disable all buttons
        Option1Button.IsEnabled = false;
        Option2Button.IsEnabled = false;
        Option3Button.IsEnabled = false;
        Option4Button.IsEnabled = false;

        if (selectedImage == _currentCorrectImage)
        {
            _correctAnswers++;
            button.BackgroundColor = Color.FromArgb("#90EE90");
            Mode2ResultLabel.Text = "Correct!";
            Mode2ResultLabel.TextColor = Color.FromArgb("#6B8E23");
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("#FFB6C1");
            Mode2ResultLabel.Text = "Wrong! The correct answer is highlighted.";
            Mode2ResultLabel.TextColor = Color.FromArgb("#B22222");

            // Highlight correct answer
            if (Option1Button.CommandParameter?.ToString() == _currentCorrectImage) Option1Button.BackgroundColor = Color.FromArgb("#90EE90");
            if (Option2Button.CommandParameter?.ToString() == _currentCorrectImage) Option2Button.BackgroundColor = Color.FromArgb("#90EE90");
            if (Option3Button.CommandParameter?.ToString() == _currentCorrectImage) Option3Button.BackgroundColor = Color.FromArgb("#90EE90");
            if (Option4Button.CommandParameter?.ToString() == _currentCorrectImage) Option4Button.BackgroundColor = Color.FromArgb("#90EE90");
        }

        UpdateScore();
        Mode2NextButton.IsVisible = true;

        // Update progress if this is the 10th question
        if (_totalAnswered % 10 == 0)
        {
            int score = (int)((_correctAnswers / (double)_totalAnswered) * 100);
            ProgressService.UpdateMemorizeScore(score);
        }
    }

    // Next question button (Mode 2)
    private void OnMode2NextClicked(object sender, EventArgs e)
    {
        SetupMultipleChoiceQuestion();
    }
}
