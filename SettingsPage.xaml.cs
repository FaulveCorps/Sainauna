// SettingsPage.xaml.cs
// Settings page with reset progress and about section - VINTAGE THEME

namespace Sainauna;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        UpdateProgressDisplay();

        // Subscribe to progress changes
        ProgressService.ProgressChanged += OnProgressChanged;
    }

    // Update progress display
    private void UpdateProgressDisplay()
    {
        double percentage = ProgressService.ProgressPercentage;
        ProgressBar.Progress = percentage / 100.0;
        ProgressLabel.Text = $"{percentage:F0}%";
    }

    // Called when progress changes
    private void OnProgressChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateProgressDisplay();
        });
    }

    // Reset progress button clicked
    private async void OnResetProgressClicked(object sender, EventArgs e)
    {
        // Show confirmation dialog
        bool confirm = await DisplayAlert(
            "Reset Progress",
            "Are you sure you want to reset all your progress? This cannot be undone.",
            "Yes, Reset",
            "Cancel");

        if (confirm)
        {
            ProgressService.ResetProgress();
            await DisplayAlert("Success", "Your progress has been reset.", "OK");
        }
    }

    // Unsubscribe from event when page is destroyed
    ~SettingsPage()
    {
        ProgressService.ProgressChanged -= OnProgressChanged;
    }
}
