using LocalizationResourceManager.Maui;
using System.Globalization;
using TaSked.App.Common;

namespace TaSked.App.Components;

public partial class LanguageSelector : ContentView
{
	private readonly ILocalizationResourceManager _localization;

	public CultureInfo DefaultCulture => _localization.DefaultCulture;
	public string DefaultLanguage => DefaultCulture.TwoLetterISOLanguageName;
	public string SelectedLanguage => _localization.CurrentCulture.TwoLetterISOLanguageName;

	public LanguageSelector()
	{
		_localization = ServiceHelper.GetService<ILocalizationResourceManager>();
		InitializeComponent();
	}

	public void SelectLanguage(CultureInfo? culture)
	{
		_localization.CurrentCulture = culture ?? DefaultCulture;
	}

	private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (e.Value == false)
		{
			return;
		}
		string? cultureString = (string?)((sender as RadioButton)?.Value);
		CultureInfo? culture = cultureString is not null ? new CultureInfo(cultureString) : null;
		SelectLanguage(culture);
    }

	private void DefaultLanguageTapped(object sender, TappedEventArgs e)
	{
		SelectLanguage(_localization.DefaultCulture);
		defaultLanguageOption.BackgroundColor = Colors.Black;
		englishLanguageOption.BackgroundColor = null;
		ukrainianLanguageOption.BackgroundColor = null;
	}

	private void EnglishLanguageTapped(object sender, TappedEventArgs e)
	{
		SelectLanguage(CultureInfo.GetCultureInfo("en"));
		defaultLanguageOption.BackgroundColor = null;
		englishLanguageOption.BackgroundColor = Colors.Black;
		ukrainianLanguageOption.BackgroundColor = null;
	}

	private void UkrainianLanguageTapped(object sender, TappedEventArgs e)
	{
		SelectLanguage(CultureInfo.GetCultureInfo("uk"));
		defaultLanguageOption.BackgroundColor = null;
		englishLanguageOption.BackgroundColor = null;
		ukrainianLanguageOption.BackgroundColor = Colors.Black;
	}
}