using TaSked.Domain;

namespace TaSked.App.Components;

public partial class RoleCard : ContentView
{
	public static readonly BindableProperty RoleModelProperty =
		BindableProperty.Create(nameof(RoleModel), typeof(Group), typeof(RoleCard));

	public Group RoleModel
	{
		get => (Group)GetValue(RoleModelProperty);
	}

	public RoleCard()
	{
		InitializeComponent();
	}
}