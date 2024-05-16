namespace TaSked.App.Components;

public partial class Divider : ContentView
{
	public static readonly BindableProperty LabelTextProperty = 
		BindableProperty.Create(nameof(LabelText), typeof(string), typeof(Divider), string.Empty);

	public string LabelText
	{
		get => (string)GetValue(LabelTextProperty);
		set => SetValue(LabelTextProperty, value);
	}

	public Divider()
	{
		InitializeComponent();
	}
}