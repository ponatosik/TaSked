using TaSked.Domain;

namespace TaSked.App.Components;

public partial class AnnouncementCard : ContentView
{
	public static readonly BindableProperty AnnouncementModelProperty =
		BindableProperty.Create(nameof(AnnouncementModel), typeof(Announcement), typeof(AnnouncementCard));

	public Announcement AnnouncementModel
	{
		get => (Announcement)GetValue(AnnouncementModelProperty);
		set => SetValue(AnnouncementModelProperty, value);
	}

	public AnnouncementCard()
	{
		InitializeComponent();
	}
}