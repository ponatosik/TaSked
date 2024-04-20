using TaSked.Domain;

namespace TaSked.App.Components;

public partial class TaskGroup : ContentView
{
	public static readonly BindableProperty TaskGroupModelProperty =
		BindableProperty.Create(nameof(TaskGroupModel), typeof(TaskGroupModel), typeof(TaskGroup));

	public TaskGroupModel TaskGroupModel
	{
		get => (TaskGroupModel)GetValue(TaskGroupModelProperty);
		set => SetValue(TaskGroupModelProperty, value);
	}
	
	public TaskGroup()
	{
		InitializeComponent();
	}
}