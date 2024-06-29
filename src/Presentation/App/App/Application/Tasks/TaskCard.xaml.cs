using TaSked.Domain;

namespace TaSked.App.Components;

public partial class TaskCard : ContentView
{
	public static readonly BindableProperty ModelProperty =
		BindableProperty.Create(nameof(Model), typeof(TaskViewModel), typeof(TaskCard), propertyChanged: OnModelChanged);

	public TaskViewModel Model
	{
		get => (TaskViewModel)GetValue(ModelProperty);
		set => SetValue(ModelProperty, value);
	}

	public TaskCard()
	{
		InitializeComponent();
	}

	static void OnModelChanged(BindableObject bindable, object oldValue, object newValue)
	{
		TaskCard? card = bindable as TaskCard;
		TaskViewModel? taskView = newValue as TaskViewModel;
		if (card is not null && taskView is not null) 
		{
			card.Opacity = taskView.Task.Completed ? 0.6 : 1;
		}
	}

	private void SwipeView_SwipeChanging(object sender, SwipeChangingEventArgs e)
	{
		var offset = Math.Clamp(e.Offset, 0, 80);
        Opacity = (1 - offset / 100);
		Rotation = offset / 16;
		RotationY = -offset / 16;
		TranslationX = offset / 4;
	}

	private void SwipeView_SwipeEnded(object sender, SwipeEndedEventArgs e)
	{
		Opacity = Model.Task.Completed ? 0.6 : 1;
		Rotation = 0;
		RotationY = 0;
		TranslationX = 0;
	}

}