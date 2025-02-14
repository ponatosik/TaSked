using Android.Gms.Common.Api.Internal;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using TaSked.Domain;

namespace TaSked.App.Components;

public partial class LinkItemComponent : ContentView, INotifyPropertyChanged
{
	public static readonly BindableProperty DisplayLinkProperty =
		BindableProperty.Create(nameof(DisplayLink), typeof(RelatedLink), typeof(LinkItemComponent), propertyChanged: OnDisplayLinkChanged);

	static void OnDisplayLinkChanged (BindableObject bindable, object oldValue, object newValue)
	{
		var component = (LinkItemComponent)bindable;
		component.OnPropertyChanged(nameof(DisplayText));
	}
	
	public string DisplayText => 
		!string.IsNullOrWhiteSpace(DisplayLink?.Title) ? DisplayLink.Title :
			DisplayLink?.Url?.Host ?? "null";
	
	public RelatedLink DisplayLink
	{
		get => (RelatedLink)GetValue(DisplayLinkProperty);
		set
		{
			SetValue(DisplayLinkProperty, value);
		}
	}
	
	private void OnLinkTapped(object sender, EventArgs e)
	{
		OpenLink();
	}

	public LinkItemComponent()
	{
		InitializeComponent();
	}
	
	private void OpenLink()
	{
		if (DisplayLink?.Url != null)
		{
			try
			{
				Browser.Default.OpenAsync(DisplayLink.Url);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to open URL: {ex.Message}");
			}
		}
	}
}