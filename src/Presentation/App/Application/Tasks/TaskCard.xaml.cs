using TaSked.Domain;

namespace TaSked.App.Components;

public partial class TaskCard : ContentView
{
	public static readonly BindableProperty ModelProperty =
		BindableProperty.Create(nameof(Model), typeof(TaskViewModel), typeof(TaskCard));

	public TaskViewModel Model
	{
		get => (TaskViewModel)GetValue(ModelProperty);
		set => SetValue(ModelProperty, value);
	}

	public TaskCard()
	{
		InitializeComponent();
	}
}