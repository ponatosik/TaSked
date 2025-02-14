using TaSked.Api.ApiClient;
using TaSked.App.Common.Models;

namespace TaSked.App;

public partial class UpdateTaskPage : ContentPage
{
	private readonly UpdateTaskViewModel _viewModel;
	
	public UpdateTaskPage(UpdateTaskViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
	
	protected override void OnAppearing()
	{
		_viewModel.RelatedLinkInputs = [.._viewModel.Homework.RelatedLinks.Select(link => new RelatedLinkModel(link.Title, link.Url.ToString()))];
		base.OnAppearing();
	}
}