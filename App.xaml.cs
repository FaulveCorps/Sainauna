// App.xaml.cs
// Main app class - IMPORTANT: Wrap MainPage in NavigationPage

namespace Sainauna;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Wrap MainPage in NavigationPage so PushAsync works!
        MainPage = new NavigationPage(new MainPage());
    }
} 