using TaSked.Domain;

namespace TaSked.App.Components;

public partial class RoleCard : ContentView
{
	public static readonly BindableProperty RoleModelProperty =
		BindableProperty.Create(nameof(RoleModel), typeof(User), typeof(RoleCard));

	public User RoleModel
	{
		get => (User)GetValue(RoleModelProperty);
	}

	public RoleCard()
	{
		InitializeComponent();
	}
}