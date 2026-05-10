namespace Sainauna;

public static class ProgressService
{
    private static int _totalPoints = 0;
    private static int _maxPoints = 1000;
    private static int _bestMemorizeScore = 0;
    private static int _bestQuizScore = 0;
    private static int _bestTestScore = 0;
    private static int _bestWriteScore = 0;
    private static int _totalWriteSpeedPoints = 0;
    private static int _totalWriteAccuracyPoints = 0;
    private static int _writeSessionsCompleted = 0;

    public static event Action? ProgressChanged;

    public static int TotalPoints => _totalPoints;
    public static int MaxPoints => _maxPoints;
    public static double ProgressPercentage => (_totalPoints / (double)_maxPoints) * 100;
    public static int BestMemorizeScore => _bestMemorizeScore;
    public static int BestQuizScore => _bestQuizScore;
    public static int BestTestScore => _bestTestScore;
    public static int BestWriteScore => _bestWriteScore;

    public static void UpdateMemorizeScore(int newScore)
    {
        if (newScore > _bestMemorizeScore)
        {
            _totalPoints -= _bestMemorizeScore;
            _bestMemorizeScore = newScore;
            _totalPoints += _bestMemorizeScore;
            ProgressChanged?.Invoke();
        }
    }

    public static void UpdateQuizScore(int newScore)
    {
        if (newScore > _bestQuizScore)
        {
            _totalPoints -= _bestQuizScore;
            _bestQuizScore = newScore;
            _totalPoints += _bestQuizScore;
            ProgressChanged?.Invoke();
        }
    }

    public static void UpdateTestScore(int newScore)
    {
        if (newScore > _bestTestScore)
        {
            _totalPoints -= _bestTestScore;
            _bestTestScore = newScore;
            _totalPoints += _bestTestScore;
            ProgressChanged?.Invoke();
        }
    }

    public static void UpdateWriteScore(int totalPoints)
    {
        int speedPortion = totalPoints / 2;
        int accuracyPortion = totalPoints - speedPortion;

        _totalWriteSpeedPoints += speedPortion;
        _totalWriteAccuracyPoints += accuracyPortion;
        _writeSessionsCompleted++;

        if (totalPoints > _bestWriteScore)
        {
            _totalPoints -= _bestWriteScore;
            _bestWriteScore = totalPoints;
            _totalPoints += _bestWriteScore;
            ProgressChanged?.Invoke();
        }
        else
        {
            ProgressChanged?.Invoke();
        }
    }

    public static void ResetProgress()
    {
        _totalPoints = 0;
        _bestMemorizeScore = 0;
        _bestQuizScore = 0;
        _bestTestScore = 0;
        _bestWriteScore = 0;
        _totalWriteSpeedPoints = 0;
        _totalWriteAccuracyPoints = 0;
        _writeSessionsCompleted = 0;
        ProgressChanged?.Invoke();
    }
}