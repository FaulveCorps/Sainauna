// LearnPage.xaml.cs
// Main learning hub with navigation to Memorize, Quiz, Test, and Write pages - VINTAGE THEME

namespace Sainauna;

public partial class LearnPage : ContentPage
{
    public LearnPage()
    {
        InitializeComponent();
        UpdateWriteButtonStatus();
        ProgressService.ProgressChanged += OnProgressChanged;

        // Run entry animations when page loads
        this.Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        await RunEntryAnimations();
    }

    private async Task RunEntryAnimations()
    {
        // Staggered slide-in animations for each option
        await MemorizeBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await MemorizeBorder.FadeTo(1, 300);

        await QuizBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await QuizBorder.FadeTo(1, 300);

        await TestBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await TestBorder.FadeTo(1, 300);

        await WriteBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await WriteBorder.FadeTo(1, 300);
    }

    private void OnProgressChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateWriteButtonStatus();
        });
    }

    private void UpdateWriteButtonStatus()
    {
        double progress = ProgressService.ProgressPercentage;

        if (progress >= 71)
        {
            WriteUnlockLabel.Text = "Master Level Unlocked!";
            WriteUnlockLabel.TextColor = Color.FromArgb("#B22222");
            WriteBorder.Stroke = Color.FromArgb("#B22222");
        }
        else if (progress >= 31)
        {
            WriteUnlockLabel.Text = "Intermediate Level Active";
            WriteUnlockLabel.TextColor = Color.FromArgb("#D2691E");
            WriteBorder.Stroke = Color.FromArgb("#D2691E");
        }
        else
        {
            WriteUnlockLabel.Text = "Start with Guided Tracing!";
            WriteUnlockLabel.TextColor = Color.FromArgb("#6B8E23");
            WriteBorder.Stroke = Color.FromArgb("#6B8E23");
        }
    }

    private async void OnMemorizeClicked(object sender, EventArgs e)
    {
        await AnimateBorderTap(MemorizeBorder);
        await Navigation.PushAsync(new MemorizePage());
    }

    private async void OnQuizClicked(object sender, EventArgs e)
    {
        await AnimateBorderTap(QuizBorder);
        await Navigation.PushAsync(new QuizPage());
    }

    private async void OnTestYourBaybayinClicked(object sender, EventArgs e)
    {
        await AnimateBorderTap(TestBorder);
        await Navigation.PushAsync(new TestPage());
    }

    private async void OnWriteClicked(object sender, EventArgs e)
    {
        await AnimateBorderTap(WriteBorder);

        string difficulty = ProgressService.ProgressPercentage >= 71 ? "Free Writing" :
                           ProgressService.ProgressPercentage >= 31 ? "Dotted Guide" : "Guided Tracing";

        await DisplayAlert("Write Your Baybayin",
            $"Welcome! Current Level: {difficulty}\n\n" +
            "Trace characters with your finger. Speed + Accuracy = Points!",
            "Let's Write!");

        await Navigation.PushAsync(new WritePage());
    }

    private async Task AnimateBorderTap(Border border)
    {
        await border.ScaleTo(0.97, 100);
        await border.ScaleTo(1, 100);
    }

    ~LearnPage()
    {
        ProgressService.ProgressChanged -= OnProgressChanged;
    }
}
