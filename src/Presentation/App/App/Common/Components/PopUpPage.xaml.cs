using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Common.Components;

// This code is written mostly by following this article: https://redth.codes/popups-with-net-maui-no-plugin-nuget-needed
// Author of original article: @Redth

public partial class PopUpPage : ContentPage
{
	new protected INavigation Navigation => Shell.Current.Navigation;

	public PopUpPage()
	{
		InitializeComponent();
		Loaded += Page_Loaded;
	}

	public async Task Dispaly()
	{
		if (!Navigation.ModalStack.Contains(this))
		{
			await Navigation.PushModalAsync(this, animated: false);
		}
	}

	public async Task Close()
	{
		if (Navigation.ModalStack.Contains(this))
		{
			await PoppingOut();
			await Navigation.PopModalAsync(animated: false);
		}
	}

	public async Task IndicateTaskRunningAsync(Func<Task> action)
	{
		await Dispaly();
		try
		{
			await action();
		}
		catch
		{
			throw;
		}
		finally
		{
			await this.Close();
		}
	}

	void Page_Loaded(object? sender, EventArgs e)
	{
		Loaded -= Page_Loaded;

		PoppingIn();
	}

	private void PoppingIn()
	{
		var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
		var contentHeight = contentSize.Request.Height;

		this.Content.TranslationY = contentHeight;

		this.Animate("Background",
			callback: v => this.Background = new SolidColorBrush(Colors.Black.WithAlpha((float)v)),
			start: 0d,
			end: 0.7d,
			rate: 32,
			length: 350,
			easing: Easing.CubicOut,
			finished: (v, k) =>
				this.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.7f)));

		this.Animate("Content",
			callback: v => this.Content.TranslationY = (int)(contentHeight - v),
			start: 0,
			end: contentHeight,
			length: 500,
			easing: Easing.CubicInOut,
			finished: (v, k) => this.Content.TranslationY = 0);
	}

	public Task PoppingOut()
	{
		var done = new TaskCompletionSource();

		var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
		var windowHeight = contentSize.Request.Height;

		this.Animate("Background",
			callback: v => this.Background = new SolidColorBrush(Colors.Black.WithAlpha((float)v)),
			start: 0.7d,
			end: 0d,
			rate: 32,
			length: 350,
			easing: Easing.CubicIn,
			finished: (v, k) => this.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.0f)));

		this.Animate("Content",
			callback: v => this.Content.TranslationY = (int)(windowHeight - v),
			start: windowHeight,
			end: 0,
			length: 500,
			easing: Easing.CubicInOut,
			finished: (v, k) =>
			{
				this.Content.TranslationY = windowHeight;
				// Important: Set our completion source to done!
				done.TrySetResult();
			});

		return done.Task;
	}

}