namespace TaSked.App;

public partial class InvintationsPage : ContentPage
{
    private InvintationViewModel _viewModel;
    public InvintationsPage(InvintationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _viewModel.ReloadInvintation();
        base.OnNavigatedTo(args);
    }
}