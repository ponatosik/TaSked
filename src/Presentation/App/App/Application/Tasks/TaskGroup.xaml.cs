using CommunityToolkit.Maui.Views;

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

    private T? FirstChild<T>(Expander expander)
    {
        List<IVisualTreeElement> listElement = (List<IVisualTreeElement>)expander.GetVisualTreeDescendants();
        foreach (IVisualTreeElement element in listElement)
        {
            if (element is T)
            {
                return (T)element;
            }
        }
        return default(T);
    }

    private void ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        if (expander.IsExpanded == true)
        {
            var collection = FirstChild<CollectionView>(expander);
            new ScaleAnimation().Animate(collection, true);
        }
        if (expander.IsExpanded == false)
        {
            var collection = FirstChild<CollectionView>(expander);
            new ScaleAnimation().Animate(collection, false);
        }
    }
}