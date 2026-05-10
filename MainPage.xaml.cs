// MainPage.xaml.cs


namespace Sainauna;

public partial class MainPage : ContentPage
{
    // Tutorial state
    private int _tutorialStep = 0;
    private List<TutorialStep> _tutorialSteps = new();

    public MainPage()
    {
        InitializeComponent();
        UpdateProgressDisplay();
        SetupTutorial();

        // Subscribe to progress changes so UI updates automatically
        ProgressService.ProgressChanged += OnProgressChanged;

        // Start animations and check for first launch
        this.Loaded += OnPageLoaded;
    }

    private void SetupTutorial()
    {
        _tutorialSteps = new List<TutorialStep>
        {
            new TutorialStep
            {
                Title = "Welcome to Sainauna!",
                Content = "Learn Baybayin, the ancient Philippine writing system. Let's take a quick tour!",
                Image = "a.png"
            },
            new TutorialStep
            {
                Title = "📜 Baybayin Alphabet",
                Content = "View all Baybayin characters with their pronunciations. Tap any character to hear its sound!",
                Image = "ba.png"
            },
            new TutorialStep
            {
                Title = "📚 Learn Baybayin",
                Content = "Practice with flashcards, take quizzes, and test your knowledge!",
                Image = "ka.png"
            },
            new TutorialStep
            {
                Title = "✍️ Test Your Skills",
                Content = "Convert Latin words to Baybayin and practice reading ancient script!",
                Image = "sa.png"
            },
            new TutorialStep
            {
                Title = "Track Your Progress",
                Content = "Watch your progress bar grow as you master Baybayin. Unlock new levels by learning!",
                Image = "nga.png"
            },
            new TutorialStep
            {
                Title = "Ready to Start?",
                Content = "Tap 'Baybayin Alphabet' to begin your journey into Philippine history!",
                Image = "ya.png"
            }
        };
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        // Run entry animations
        await RunEntryAnimations();

        // Check if this is first launch (you can use Preferences to track this)
        bool hasSeenTutorial = Preferences.Get("HasSeenTutorial", false);
        if (!hasSeenTutorial)
        {
            ShowTutorial();
        }
    }

    private async Task RunEntryAnimations()
    {
        // Title pop-in
        await TitleLabel.FadeTo(1, 600, Easing.CubicOut);
        await TitleLabel.ScaleTo(1, 400, Easing.SpringOut);

        // Buttons slide in
        await BaybayinButton.TranslateTo(0, 0, 500, Easing.CubicOut);
        await BaybayinButton.FadeTo(1, 400);

        await LearnButton.TranslateTo(0, 0, 500, Easing.CubicOut);
        await LearnButton.FadeTo(1, 400);

        await TestButton.TranslateTo(0, 0, 500, Easing.CubicOut);
        await TestButton.FadeTo(1, 400);

        // Progress border slide up
        await ProgressBorder.TranslateTo(0, 0, 500, Easing.CubicOut);
        await ProgressBorder.FadeTo(1, 400);

        // Footer fade in
        await FooterLabel.FadeTo(1, 400);

        // Header buttons
        await HomeButton.FadeTo(1, 300);
        await SettingsButton.FadeTo(1, 300);
    }

    private void ShowTutorial()
    {
        _tutorialStep = 0;
        UpdateTutorialContent();
        TutorialOverlay.IsVisible = true;
        TutorialOverlay.Opacity = 0;
        TutorialOverlay.FadeTo(1, 300);
    }

    private void UpdateTutorialContent()
    {
        if (_tutorialStep < _tutorialSteps.Count)
        {
            var step = _tutorialSteps[_tutorialStep];
            TutorialTitle.Text = step.Title;
            TutorialContent.Text = step.Content;
            TutorialImage.Source = step.Image;
            TutorialNextButton.Text = _tutorialStep < _tutorialSteps.Count - 1 ? "Next" : "Start Learning!";
        }
    }

    private async void OnTutorialNextClicked(object sender, EventArgs e)
    {
        _tutorialStep++;

        if (_tutorialStep >= _tutorialSteps.Count)
        {
            // Tutorial complete
            Preferences.Set("HasSeenTutorial", true);
            await TutorialOverlay.FadeTo(0, 300);
            TutorialOverlay.IsVisible = false;
        }
        else
        {
            // Animate content change
            await TutorialContent.FadeTo(0, 150);
            UpdateTutorialContent();
            await TutorialContent.FadeTo(1, 150);
        }
    }

    private async void OnTutorialSkipClicked(object sender, EventArgs e)
    {
        Preferences.Set("HasSeenTutorial", true);
        await TutorialOverlay.FadeTo(0, 300);
        TutorialOverlay.IsVisible = false;
    }

    // Update progress bar and label
    private void UpdateProgressDisplay()
    {
        double percentage = ProgressService.ProgressPercentage;
        ProgressBar.Progress = percentage / 100.0;
        ProgressLabel.Text = $"{percentage:F0}%";
    }

    // Called when progress changes anywhere in the app
    private void OnProgressChanged()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            UpdateProgressDisplay();
            // Animate progress update
            await ProgressLabel.ScaleTo(1.2, 200);
            await ProgressLabel.ScaleTo(1, 200);
        });
    }

    // Home icon clicked - refresh/go home
    private void OnHomeClicked(object sender, EventArgs e)
    {
        UpdateProgressDisplay();
    }

    // Settings button clicked
    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await AnimateButtonPress(SettingsButton);
        await Navigation.PushAsync(new SettingsPage());
    }

    // Baybayin Page button clicked
    private async void OnBaybayinClicked(object sender, EventArgs e)
    {
        await AnimateButtonPress(BaybayinButton);
        await Navigation.PushAsync(new BaybayinPage());
    }

    // Learn Page button clicked
    private async void OnLearnClicked(object sender, EventArgs e)
    {
        await AnimateButtonPress(LearnButton);
        await Navigation.PushAsync(new LearnPage());
    }

    // Test Your Baybayin button clicked
    private async void OnTestYourBaybayinClicked(object sender, EventArgs e)
    {
        await AnimateButtonPress(TestButton);
        await Navigation.PushAsync(new TestPage());
    }

    private async Task AnimateButtonPress(VisualElement button)
    {
        await button.ScaleTo(0.95, 100);
        await button.ScaleTo(1, 100);
    }

    // Unsubscribe from event when page is destroyed
    ~MainPage()
    {
        ProgressService.ProgressChanged -= OnProgressChanged;
    }
}

// Tutorial step data class
public class TutorialStep
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

// Animation Behaviors (simplified - you can add these as separate classes if needed)
public class FadeInBehavior : Behavior<VisualElement>
{
    public int Delay { get; set; } = 0;
}

public class PopInBehavior : Behavior<VisualElement>
{
    public int Delay { get; set; } = 0;
}

public class SlideUpBehavior : Behavior<VisualElement>
{
    public int Delay { get; set; } = 0;
}

public class SlideInBehavior : Behavior<VisualElement>
{
    public int Delay { get; set; } = 0;
}
