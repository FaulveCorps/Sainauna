// TestPage.xaml.cs
// Test Your Baybayin - SYMBOL-ONLY images - VINTAGE THEME

namespace Sainauna;

public partial class TestPage : ContentPage
{
    // Dictionary for Latin to Baybayin conversion
    private Dictionary<string, string> _baybayinMap = new();

    // Dictionary for Baybayin to image mapping
    private Dictionary<string, string> _baybayinToImageMap = new();

    // Practice words list
    private List<string> _practiceWords = new();

    // Current practice word for reading mode
    private string _currentReadingWord = "";

    // Points earned on this page
    private int _points = 0;
    private int _correctReadings = 0;

    // Random
    private Random _random = new();

    public TestPage()
    {
        InitializeComponent();
        InitializeBaybayinMap();
        InitializeBaybayinToImageMap();
        InitializePracticeWords();
        UpdateScoreDisplay();
    }

    // Initialize the conversion dictionary
    private void InitializeBaybayinMap()
    {
        _baybayinMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Vowels
            ["a"] = "ᜀ",
            ["e"] = "ᜁ",
            ["i"] = "ᜁ",
            ["o"] = "ᜂ",
            ["u"] = "ᜂ",

            // Consonants + a
            ["ba"] = "ᜊ",
            ["ka"] = "ᜃ",
            ["da"] = "ᜇ",
            ["ra"] = "ᜇ",
            ["ga"] = "ᜄ",
            ["ha"] = "ᜑ",
            ["la"] = "ᜎ",
            ["ma"] = "ᜋ",
            ["na"] = "ᜈ",
            ["nga"] = "ᜅ",
            ["pa"] = "ᜉ",
            ["sa"] = "ᜐ",
            ["ta"] = "ᜆ",
            ["wa"] = "ᜏ",
            ["ya"] = "ᜌ",

            // Consonants + e/i
            ["be"] = "ᜊᜒ",
            ["bi"] = "ᜊᜒ",
            ["ke"] = "ᜃᜒ",
            ["ki"] = "ᜃᜒ",
            ["de"] = "ᜇᜒ",
            ["di"] = "ᜇᜒ",
            ["re"] = "ᜇᜒ",
            ["ri"] = "ᜇᜒ",
            ["ge"] = "ᜄᜒ",
            ["gi"] = "ᜄᜒ",
            ["he"] = "ᜑᜒ",
            ["hi"] = "ᜑᜒ",
            ["le"] = "ᜎᜒ",
            ["li"] = "ᜎᜒ",
            ["me"] = "ᜋᜒ",
            ["mi"] = "ᜋᜒ",
            ["ne"] = "ᜈᜒ",
            ["ni"] = "ᜈᜒ",
            ["nge"] = "ᜅᜒ",
            ["ngi"] = "ᜅᜒ",
            ["pe"] = "ᜉᜒ",
            ["pi"] = "ᜉᜒ",
            ["se"] = "ᜐᜒ",
            ["si"] = "ᜐᜒ",
            ["te"] = "ᜆᜒ",
            ["ti"] = "ᜆᜒ",
            ["we"] = "ᜏᜒ",
            ["wi"] = "ᜏᜒ",
            ["ye"] = "ᜌᜒ",
            ["yi"] = "ᜌᜒ",

            // Consonants + o/u
            ["bo"] = "ᜊᜓ",
            ["bu"] = "ᜊᜓ",
            ["ko"] = "ᜃᜓ",
            ["ku"] = "ᜃᜓ",
            ["do"] = "ᜇᜓ",
            ["du"] = "ᜇᜓ",
            ["ro"] = "ᜇᜓ",
            ["ru"] = "ᜇᜓ",
            ["go"] = "ᜄᜓ",
            ["gu"] = "ᜄᜓ",
            ["ho"] = "ᜑᜓ",
            ["hu"] = "ᜑᜓ",
            ["lo"] = "ᜎᜓ",
            ["lu"] = "ᜎᜓ",
            ["mo"] = "ᜋᜓ",
            ["mu"] = "ᜋᜓ",
            ["no"] = "ᜈᜓ",
            ["nu"] = "ᜈᜓ",
            ["ngo"] = "ᜅᜓ",
            ["ngu"] = "ᜅᜓ",
            ["po"] = "ᜉᜓ",
            ["pu"] = "ᜉᜓ",
            ["so"] = "ᜐᜓ",
            ["su"] = "ᜐᜓ",
            ["to"] = "ᜆᜓ",
            ["tu"] = "ᜆᜓ",
            ["wo"] = "ᜏᜓ",
            ["wu"] = "ᜏᜓ",
            ["yo"] = "ᜌᜓ",
            ["yu"] = "ᜌᜓ"
        };
    }

    // Initialize the Baybayin to Image mapping - SYMBOLS ONLY
    private void InitializeBaybayinToImageMap()
    {
        _baybayinToImageMap = new Dictionary<string, string>
        {
            // Vowels
            ["ᜀ"] = "a.png",
            ["ᜁ"] = "e_i.png",
            ["ᜂ"] = "o_u.png",

            // Consonants + a
            ["ᜊ"] = "ba.png",
            ["ᜃ"] = "ka.png",
            ["ᜇ"] = "da_ra.png",
            ["ᜄ"] = "ga.png",
            ["ᜑ"] = "ha.png",
            ["ᜎ"] = "la.png",
            ["ᜋ"] = "ma.png",
            ["ᜈ"] = "na.png",
            ["ᜅ"] = "nga.png",
            ["ᜉ"] = "pa.png",
            ["ᜐ"] = "sa.png",
            ["ᜆ"] = "ta.png",
            ["ᜏ"] = "wa.png",
            ["ᜌ"] = "ya.png",

            // Consonants + e/i
            ["ᜊᜒ"] = "be_bi.png",
            ["ᜃᜒ"] = "ke_ki.png",
            ["ᜇᜒ"] = "de_di_re_ri.png",
            ["ᜄᜒ"] = "ge_gi.png",
            ["ᜑᜒ"] = "he_hi.png",
            ["ᜎᜒ"] = "le_li.png",
            ["ᜋᜒ"] = "me_mi.png",
            ["ᜈᜒ"] = "ne_ni.png",
            ["ᜅᜒ"] = "nge_ngi.png",
            ["ᜉᜒ"] = "pe_pi.png",
            ["ᜐᜒ"] = "se_si.png",
            ["ᜆᜒ"] = "te_ti.png",
            ["ᜏᜒ"] = "we_wi.png",
            ["ᜌᜒ"] = "ye_yi.png",

            // Consonants + o/u
            ["ᜊᜓ"] = "bo_bu.png",
            ["ᜃᜓ"] = "ko_ku.png",
            ["ᜇᜓ"] = "do_du_ro_ru.png",
            ["ᜄᜓ"] = "go_gu.png",
            ["ᜑᜓ"] = "ho_hu.png",
            ["ᜎᜓ"] = "lo_lu.png",
            ["ᜋᜓ"] = "mo_mu.png",
            ["ᜈᜓ"] = "no_nu.png",
            ["ᜅᜓ"] = "ngo_ngu.png",
            ["ᜉᜓ"] = "po_pu.png",
            ["ᜐᜓ"] = "so_su.png",
            ["ᜆᜓ"] = "to_tu.png",
            ["ᜏᜓ"] = "wo_wu.png",
            ["ᜌᜓ"] = "yo_yu.png"
        };
    }

    // Initialize practice words
    private void InitializePracticeWords()
    {
        _practiceWords = new List<string>
        {
            "bahay", "aso", "pusa", "bata", "ina", "ama", "tubig", "pagkain",
            "kaibigan", "araw", "gabi", "buwan", "bituin", "hangin", "apoy",
            "lupa", "dagat", "bundok", "punongkahoy", "bulaklak", "prutas",
            "gulay", "isda", "manok", "kabayo", "kalabaw", "aklat", "lapis",
            "papel", "mesa", "silya", "pinto", "bintana", "kalye", "bayan"
        };
    }

    // Update score display
    private void UpdateScoreDisplay()
    {
        ScoreLabel.Text = $"Points: {_points}";
    }

    // Switch to Mode 1 (Latin to Baybayin)
    private void OnMode1Clicked(object sender, EventArgs e)
    {
        Mode1Layout.IsVisible = true;
        Mode2Layout.IsVisible = false;
        Mode1Button.BackgroundColor = Color.FromArgb("#8B5A2B");
        Mode1Button.TextColor = Color.FromArgb("#FFF8DC");
        Mode2Button.BackgroundColor = Color.FromArgb("#D2B48C");
        Mode2Button.TextColor = Color.FromArgb("#654321");
    }

    // Switch to Mode 2 (Baybayin to Latin)
    private void OnMode2Clicked(object sender, EventArgs e)
    {
        Mode1Layout.IsVisible = false;
        Mode2Layout.IsVisible = true;
        Mode1Button.BackgroundColor = Color.FromArgb("#D2B48C");
        Mode1Button.TextColor = Color.FromArgb("#654321");
        Mode2Button.BackgroundColor = Color.FromArgb("#8B5A2B");
        Mode2Button.TextColor = Color.FromArgb("#FFF8DC");

        // Setup first reading word
        SetupNewReadingWord();
    }

    // Convert Latin word to Baybayin and display as SYMBOL-ONLY images
    private void OnConvertToBaybayin(object sender, EventArgs e)
    {
        string input = LatinInput.Text?.Trim().ToLower() ?? "";

        if (string.IsNullOrEmpty(input))
        {
            DisplayResultMessage("Please enter a word");
            return;
        }

        var baybayinChars = ConvertToBaybayinList(input);
        DisplayBaybayinImages(BaybayinResultLayout, baybayinChars);
    }

    // Conversion logic - returns list of Baybayin characters
    private List<string> ConvertToBaybayinList(string word)
    {
        var result = new List<string>();
        int i = 0;

        while (i < word.Length)
        {
            // Check for 'ng' first (special case - two letters)
            if (i + 1 < word.Length && word.Substring(i, 2) == "ng")
            {
                // Get the vowel after 'ng'
                if (i + 2 < word.Length)
                {
                    string ngSyllable = "ng" + word[i + 2];
                    if (_baybayinMap.ContainsKey(ngSyllable))
                    {
                        result.Add(_baybayinMap[ngSyllable]);
                        i += 3;
                        continue;
                    }
                }
                // Just 'nga' if no vowel specified
                result.Add(_baybayinMap["nga"]);
                i += 2;
                continue;
            }

            // Try to match 2-letter syllable (consonant + vowel)
            if (i + 1 < word.Length)
            {
                string syllable = word.Substring(i, 2);
                if (_baybayinMap.ContainsKey(syllable))
                {
                    result.Add(_baybayinMap[syllable]);
                    i += 2;
                    continue;
                }
            }

            // Try single vowel
            string letter = word[i].ToString();
            if (_baybayinMap.ContainsKey(letter))
            {
                result.Add(_baybayinMap[letter]);
            }
            else
            {
                // Character not in Baybayin, add placeholder
                result.Add("?");
            }

            i++;
        }

        return result;
    }

    // Display Baybayin characters as SYMBOL-ONLY images
    private void DisplayBaybayinImages(FlexLayout layout, List<string> baybayinChars)
    {
        layout.Children.Clear();

        foreach (var ch in baybayinChars)
        {
            if (_baybayinToImageMap.ContainsKey(ch))
            {
                var image = new Image
                {
                    Source = _baybayinToImageMap[ch],
                    WidthRequest = 70,
                    HeightRequest = 70,
                    Aspect = Aspect.AspectFit,
                    Margin = new Thickness(3)
                };
                layout.Children.Add(image);
            }
            else
            {
                // Unknown character
                var label = new Label
                {
                    Text = "?",
                    FontSize = 32,
                    TextColor = Color.FromArgb("#B22222"),
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(5)
                };
                layout.Children.Add(label);
            }
        }
    }

    // Display a message in the result layout
    private void DisplayResultMessage(string message)
    {
        BaybayinResultLayout.Children.Clear();
        BaybayinResultLayout.Children.Add(new Label
        {
            Text = message,
            FontSize = 16,
            TextColor = Color.FromArgb("#8B7355"),
            FontAttributes = FontAttributes.Italic
        });
    }

    // Get a random practice word
    private void OnGetPracticeWord(object sender, EventArgs e)
    {
        string word = _practiceWords[_random.Next(_practiceWords.Count)];
        LatinInput.Text = word;
        var baybayinChars = ConvertToBaybayinList(word);
        DisplayBaybayinImages(BaybayinResultLayout, baybayinChars);
        PracticeWordLabel.Text = $"'{word}' in Baybayin:";
    }

    // Setup new reading word for Mode 2 - SYMBOLS ONLY
    private void SetupNewReadingWord()
    {
        _currentReadingWord = _practiceWords[_random.Next(_practiceWords.Count)];
        var baybayinChars = ConvertToBaybayinList(_currentReadingWord);
        DisplayBaybayinImages(BaybayinWordLayout, baybayinChars);

        ReadingAnswerEntry.Text = "";
        ReadingResultLabel.Text = "";
        NextWordButton.IsVisible = false;
        ReadingAnswerEntry.IsEnabled = true;
    }

    // Check reading answer
    private void OnCheckReading(object sender, EventArgs e)
    {
        string userAnswer = ReadingAnswerEntry.Text?.Trim().ToLower() ?? "";

        if (string.IsNullOrEmpty(userAnswer))
        {
            ReadingResultLabel.Text = "Please enter an answer";
            ReadingResultLabel.TextColor = Color.FromArgb("#D2691E");
            return;
        }

        ReadingAnswerEntry.IsEnabled = false;

        if (userAnswer == _currentReadingWord)
        {
            _correctReadings++;
            _points += 10;
            UpdateScoreDisplay();

            ReadingResultLabel.Text = "Correct! +10 points";
            ReadingResultLabel.TextColor = Color.FromArgb("#6B8E23");

            // Update progress tracker (max 100 points)
            ProgressService.UpdateTestScore(Math.Min(_points, 100));
        }
        else
        {
            ReadingResultLabel.Text = $"Wrong! It was '{_currentReadingWord}'";
            ReadingResultLabel.TextColor = Color.FromArgb("#B22222");
        }

        NextWordButton.IsVisible = true;
    }

    // Next word button
    private void OnNextWord(object sender, EventArgs e)
    {
        SetupNewReadingWord();
    }
}
