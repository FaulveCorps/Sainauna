// BaybayinPage.xaml.cs
// Shows the Baybayin alphabet with SYMBOL-ONLY images and animations - VINTAGE THEME

using Microsoft.Maui.Controls.Shapes;

namespace Sainauna;

public partial class BaybayinPage : ContentPage
{
    // Class to hold each Baybayin character data
    public class BaybayinCharacter
    {
        public string ImageName { get; set; } = string.Empty;
        public string Pronunciation { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public BaybayinPage()
    {
        InitializeComponent();
        LoadBaybayinCharacters();

        // Run animations when page loads
        this.Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        await RunEntryAnimations();
    }

    private async Task RunEntryAnimations()
    {
        // Staggered fade-in and slide-up for each section
        await VowelsBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await VowelsBorder.FadeTo(1, 300);

        await ConsonantsABorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await ConsonantsABorder.FadeTo(1, 300);

        await ConsonantsEIBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await ConsonantsEIBorder.FadeTo(1, 300);

        await ConsonantsOUBorder.TranslateTo(0, 0, 400, Easing.CubicOut);
        await ConsonantsOUBorder.FadeTo(1, 300);
    }

    private void LoadBaybayinCharacters()
    {
        // Vowels
        var vowels = new List<BaybayinCharacter>
        {
            new BaybayinCharacter { ImageName = "a.png", Pronunciation = "a", Category = "vowel" },
            new BaybayinCharacter { ImageName = "e_i.png", Pronunciation = "e/i", Category = "vowel" },
            new BaybayinCharacter { ImageName = "o_u.png", Pronunciation = "o/u", Category = "vowel" }
        };

        // Consonants + a
        var consonantsA = new List<BaybayinCharacter>
        {
            new BaybayinCharacter { ImageName = "ba.png", Pronunciation = "ba", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "ka.png", Pronunciation = "ka", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "da_ra.png", Pronunciation = "da/ra", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "ga.png", Pronunciation = "ga", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "ha.png", Pronunciation = "ha", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "la.png", Pronunciation = "la", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "ma.png", Pronunciation = "ma", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "na.png", Pronunciation = "na", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "nga.png", Pronunciation = "nga", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "pa.png", Pronunciation = "pa", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "sa.png", Pronunciation = "sa", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "ta.png", Pronunciation = "ta", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "wa.png", Pronunciation = "wa", Category = "consonant_a" },
            new BaybayinCharacter { ImageName = "ya.png", Pronunciation = "ya", Category = "consonant_a" }
        };

        // Consonants + e/i
        var consonantsEI = new List<BaybayinCharacter>
        {
            new BaybayinCharacter { ImageName = "be_bi.png", Pronunciation = "be/bi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "ke_ki.png", Pronunciation = "ke/ki", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "de_di_re_ri.png", Pronunciation = "de/di/re/ri", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "ge_gi.png", Pronunciation = "ge/gi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "he_hi.png", Pronunciation = "he/hi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "le_li.png", Pronunciation = "le/li", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "me_mi.png", Pronunciation = "me/mi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "ne_ni.png", Pronunciation = "ne/ni", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "nge_ngi.png", Pronunciation = "nge/ngi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "pe_pi.png", Pronunciation = "pe/pi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "se_si.png", Pronunciation = "se/si", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "te_ti.png", Pronunciation = "te/ti", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "we_wi.png", Pronunciation = "we/wi", Category = "consonant_ei" },
            new BaybayinCharacter { ImageName = "ye_yi.png", Pronunciation = "ye/yi", Category = "consonant_ei" }
        };

        // Consonants + o/u
        var consonantsOU = new List<BaybayinCharacter>
        {
            new BaybayinCharacter { ImageName = "bo_bu.png", Pronunciation = "bo/bu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "ko_ku.png", Pronunciation = "ko/ku", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "do_du_ro_ru.png", Pronunciation = "do/du/ro/ru", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "go_gu.png", Pronunciation = "go/gu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "ho_hu.png", Pronunciation = "ho/hu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "lo_lu.png", Pronunciation = "lo/lu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "mo_mu.png", Pronunciation = "mo/mu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "no_nu.png", Pronunciation = "no/nu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "ngo_ngu.png", Pronunciation = "ngo/ngu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "po_pu.png", Pronunciation = "po/pu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "so_su.png", Pronunciation = "so/su", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "to_tu.png", Pronunciation = "to/tu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "wo_wu.png", Pronunciation = "wo/wu", Category = "consonant_ou" },
            new BaybayinCharacter { ImageName = "yo_yu.png", Pronunciation = "yo/yu", Category = "consonant_ou" }
        };

        // Add to layouts
        AddCharactersToLayout(VowelsLayout, vowels);
        AddCharactersToLayout(ConsonantsALayout, consonantsA);
        AddCharactersToLayout(ConsonantsEILayout, consonantsEI);
        AddCharactersToLayout(ConsonantsOULayout, consonantsOU);
    }

    private void AddCharactersToLayout(FlexLayout layout, List<BaybayinCharacter> characters)
    {
        foreach (var character in characters)
        {
            var frame = new Border
            {
                BackgroundColor = Color.FromArgb("#FFF8DC"),
                Stroke = Color.FromArgb("#8B5A2B"),
                StrokeThickness = 2,
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                WidthRequest = 80,
                HeightRequest = 90,
                StrokeShape = new RoundRectangle { CornerRadius = 10 }
            };

            var stack = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // SYMBOL-ONLY image (no pronunciation label)
            var image = new Image
            {
                Source = character.ImageName,
                WidthRequest = 65,
                HeightRequest = 65,
                Aspect = Aspect.AspectFit
            };

            stack.Children.Add(image);
            frame.Content = stack;

            // Add tap gesture with animation
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await AnimateCharacterTap(frame);
                await OnCharacterTapped(character.Pronunciation);
            };
            frame.GestureRecognizers.Add(tapGesture);

            layout.Children.Add(frame);
        }
    }

    private async Task AnimateCharacterTap(Border border)
    {
        await border.ScaleTo(0.9, 100);
        await border.ScaleTo(1, 100);
    }

    private async Task OnCharacterTapped(string pronunciation)
    {
        await DisplayAlert("Pronunciation", $"This character is pronounced: '{pronunciation}'", "OK");
    }
}
