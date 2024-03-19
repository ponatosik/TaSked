namespace TaSked.App;

public partial class RolePage : ContentPage
{
    private RoleViewModel _viewModel;
    public RolePage(RoleViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _viewModel.ReloadRole();

        base.OnNavigatedTo(args);
    }
}