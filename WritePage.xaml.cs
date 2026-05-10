// WritePage.xaml.cs
// Drawing canvas with speed/accuracy scoring - 3 Difficulty Levels

namespace Sainauna;

public partial class WritePage : ContentPage
{
    // Drawing path data
    private List<List<PointF>> _allPaths = new();
    private List<PointF> _currentPath = new();
    private bool _isDrawing = false;

    // Drawing drawable for rendering
    private DrawingDrawable _drawable;

    // Session data
    private List<string> _sessionItems = new();
    private int _currentItemIndex = 0;
    private int _sessionScore = 0;

    // Timing
    private DateTime _startTime;
    private bool _timerRunning = false;

    // Scoring for current item
    private int _currentSpeedPoints = 0;
    private int _currentAccuracyPoints = 0;

    // Random
    private Random _random = new();

    // Level data
    private int _currentLevel = 1;

    // Character and word lists
    private List<string> _level1Characters = new() { "a", "ba", "ka", "da", "ga", "ha", "la", "ma", "na", "nga", "pa", "sa", "ta", "wa", "ya" };
    private List<string> _level2Words = new() { "bahay", "aso", "pusa", "bata", "ina", "ama", "tubig", "araw", "gabi" };
    private List<string> _level3Words = new() { "kaibigan", "pagkain", "bundok", "bulaklak", "punongkahoy", "kalabaw", "bintana" };

    public WritePage()
    {
        InitializeComponent();
        InitializeDrawingCanvas();
        DetermineLevel();
        SetupSession();
    }

    private void DetermineLevel()
    {
        double progress = ProgressService.ProgressPercentage;

        if (progress >= 71)
        {
            _currentLevel = 3;
            LevelLabel.Text = "Level 3: Free Writing";
            GuideImage.Opacity = 0;
            GuideImage.IsVisible = false;
        }
        else if (progress >= 31)
        {
            _currentLevel = 2;
            LevelLabel.Text = "Level 2: Dotted Guide";
            GuideImage.Opacity = 0.15;
        }
        else
        {
            _currentLevel = 1;
            LevelLabel.Text = "Level 1: Guided Tracing";
            GuideImage.Opacity = 0.3;
        }
    }

    private void InitializeDrawingCanvas()
    {
        _drawable = new DrawingDrawable();
        DrawingCanvas.Drawable = _drawable;
        DrawingCanvas.Invalidate();
    }

    private void SetupSession()
    {
        _sessionItems.Clear();
        _currentItemIndex = 0;
        _sessionScore = 0;

        // Select 5 items based on level
        var sourceList = _currentLevel == 1 ? _level1Characters :
                        _currentLevel == 2 ? _level2Words : _level3Words;

        _sessionItems = sourceList.OrderBy(x => _random.Next()).Take(5).ToList();

        LoadCurrentItem();
    }

    private void LoadCurrentItem()
    {
        if (_currentItemIndex >= _sessionItems.Count)
        {
            ShowSessionComplete();
            return;
        }

        string currentItem = _sessionItems[_currentItemIndex];
        ProgressLabel.Text = $"{_currentItemIndex + 1}/{_sessionItems.Count}";

        // Clear previous drawing
        ClearCanvas();

        // Setup guide display
        if (_currentLevel == 1)
        {
            // Single character with solid guide
            GuideImage.Source = $"{currentItem}.png";
            GuideImage.IsVisible = true;
            TargetWordLayout.IsVisible = false;
            InstructionLabel.Text = $"Trace: '{currentItem}'";
        }
        else if (_currentLevel == 2)
        {
            // Word with dotted guide
            DisplayWordAsImages(currentItem, true);
            GuideImage.IsVisible = false;
            TargetWordLayout.IsVisible = true;
            InstructionLabel.Text = $"Trace the word: '{currentItem}'";
        }
        else
        {
            // Free writing - no guide
            DisplayWordAsImages(currentItem, false);
            GuideImage.IsVisible = false;
            TargetWordLayout.IsVisible = true;
            InstructionLabel.Text = $"Write: '{currentItem}' (no guide)";
        }

        // Reset scoring
        _currentSpeedPoints = 0;
        _currentAccuracyPoints = 0;
        SpeedLabel.Text = "0 pts";
        AccuracyLabel.Text = "0%";
        TimerLabel.Text = "0.0s";

        // Reset buttons
        CheckButton.IsEnabled = true;
        CheckButton.Opacity = 1;
        CheckButton.Text = "✓ Check";
        NextButton.IsEnabled = false;
        NextButton.Opacity = 0.5;

        _timerRunning = false;
    }

    private void DisplayWordAsImages(string word, bool showGuide)
    {
        TargetWordLayout.Children.Clear();

        var chars = ConvertToBaybayinList(word);
        foreach (var ch in chars)
        {
            var image = new Image
            {
                Source = $"{ch}.png",
                WidthRequest = 50,
                HeightRequest = 60,
                Aspect = Aspect.AspectFit,
                Margin = new Thickness(2),
                Opacity = showGuide ? 0.2 : 1.0
            };
            TargetWordLayout.Children.Add(image);
        }
    }

    private List<string> ConvertToBaybayinList(string word)
    {
        var result = new List<string>();
        int i = 0;

        while (i < word.Length)
        {
            if (i + 1 < word.Length && word.Substring(i, 2) == "ng")
            {
                if (i + 2 < word.Length)
                {
                    string ngSyllable = "ng" + word[i + 2];
                    result.Add(ngSyllable);
                    i += 3;
                    continue;
                }
                result.Add("nga");
                i += 2;
                continue;
            }

            if (i + 1 < word.Length)
            {
                string syllable = word.Substring(i, 2);
                result.Add(syllable);
                i += 2;
                continue;
            }

            result.Add(word[i].ToString());
            i++;
        }

        return result;
    }

    // Touch/Mouse Interaction Handlers
    private void OnStartInteraction(object sender, TouchEventArgs e)
    {
        _isDrawing = true;
        _currentPath = new List<PointF>();

        PointF point = e.Touches[0];
        _currentPath.Add(point);

        if (!_timerRunning)
        {
            _startTime = DateTime.Now;
            _timerRunning = true;
            StartTimer();
        }

        DrawingCanvas.Invalidate();
    }

    private void OnDragInteraction(object sender, TouchEventArgs e)
    {
        if (!_isDrawing) return;

        PointF point = e.Touches[0];
        _currentPath.Add(point);

        DrawingCanvas.Invalidate();
    }

    private void OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (_isDrawing && _currentPath.Count > 0)
        {
            _allPaths.Add(new List<PointF>(_currentPath));
            _currentPath.Clear();
        }
        _isDrawing = false;
        DrawingCanvas.Invalidate();
    }

    private async void StartTimer()
    {
        while (_timerRunning)
        {
            await Task.Delay(100);
            if (_timerRunning)
            {
                double elapsed = (DateTime.Now - _startTime).TotalSeconds;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TimerLabel.Text = $"{elapsed:F1}s";
                });
            }
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        ClearCanvas();
        _timerRunning = false;
        TimerLabel.Text = "0.0s";
    }

    private void ClearCanvas()
    {
        _allPaths.Clear();
        _currentPath.Clear();
        _drawable.SetPaths(_allPaths);
        DrawingCanvas.Invalidate();
    }

    private void OnCheckClicked(object sender, EventArgs e)
    {
        if (_allPaths.Count == 0 || _allPaths.Sum(p => p.Count) < 10)
        {
            DisplayAlert("Empty Drawing", "Please draw something first!", "OK");
            return;
        }

        _timerRunning = false;

        // Calculate scores
        double elapsed = (DateTime.Now - _startTime).TotalSeconds;
        CalculateScores(elapsed);

        // Update UI
        SpeedLabel.Text = $"{_currentSpeedPoints} pts";
        AccuracyLabel.Text = $"{_currentAccuracyPoints}%";

        int totalItemPoints = _currentSpeedPoints + (_currentAccuracyPoints / 2);
        _sessionScore += totalItemPoints;
        TotalPointsLabel.Text = _sessionScore.ToString();

        // Show feedback
        string feedback = totalItemPoints >= 80 ? "Excellent!" :
                         totalItemPoints >= 50 ? "Good job!" : "Keep practicing!";
        CheckButton.Text = feedback;
        CheckButton.IsEnabled = false;
        CheckButton.Opacity = 0.7;

        NextButton.IsEnabled = true;
        NextButton.Opacity = 1;
    }

    private void CalculateScores(double elapsedSeconds)
    {
        // Speed scoring: optimal is 4s for char, 6s for word
        string currentItem = _sessionItems[_currentItemIndex];
        double optimalTime = currentItem.Length <= 2 ? 4.0 : 6.0;

        if (elapsedSeconds <= optimalTime)
            _currentSpeedPoints = 50;
        else if (elapsedSeconds <= optimalTime * 1.5)
            _currentSpeedPoints = 35;
        else if (elapsedSeconds <= optimalTime * 2)
            _currentSpeedPoints = 20;
        else
            _currentSpeedPoints = 10;

        // Accuracy scoring based on stroke count and density
        int totalPoints = _allPaths.Sum(p => p.Count);
        int pathCount = _allPaths.Count;

        // Simple heuristic: more points = more effort, but not too many paths
        if (totalPoints >= 50 && totalPoints <= 300 && pathCount >= 1 && pathCount <= 10)
            _currentAccuracyPoints = 50 + _random.Next(0, 20);
        else if (totalPoints >= 30 && pathCount >= 1)
            _currentAccuracyPoints = 30 + _random.Next(0, 20);
        else
            _currentAccuracyPoints = 15 + _random.Next(0, 15);

        if (_currentAccuracyPoints > 100) _currentAccuracyPoints = 100;
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        _currentItemIndex++;
        LoadCurrentItem();
    }

    private async void ShowSessionComplete()
    {
        _timerRunning = false;

        // Update progress
        ProgressService.UpdateWriteScore(_sessionScore);

        string message = _sessionScore >= 400 ? "Outstanding! You're a Baybayin master!" :
                        _sessionScore >= 250 ? "Great work! Keep practicing!" :
                        "Good effort! Practice makes perfect!";

        await DisplayAlert("Session Complete!",
            $"{message}\n\nTotal Score: {_sessionScore} points", "OK");

        await Navigation.PopAsync();
    }
}

// Custom Drawable for rendering drawing paths
public class DrawingDrawable : IDrawable
{
    private List<List<PointF>> _paths = new();

    public void SetPaths(List<List<PointF>> paths)
    {
        _paths = paths;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Color.FromArgb("#654321");
        canvas.StrokeSize = 4;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;

        foreach (var path in _paths)
        {
            if (path.Count < 2) continue;

            var pathF = new PathF();
            pathF.MoveTo(path[0]);

            for (int i = 1; i < path.Count; i++)
            {
                pathF.LineTo(path[i]);
            }

            canvas.DrawPath(pathF);
        }
    }
}
