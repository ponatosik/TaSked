using CommunityToolkit.Mvvm.Input;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App.Components;

public partial class SubjectCard: ContentView
{
	public static readonly BindableProperty SubjectDTOModelProperty =
		BindableProperty.Create(nameof(SubjectDTOModel), typeof(SubjectDTO), typeof(SubjectCard));

	public SubjectDTO SubjectDTOModel
	{
		get => (SubjectDTO)GetValue(SubjectDTOModelProperty);
		set => SetValue(SubjectDTOModelProperty, value);
	}

	public SubjectCard()
	{
		InitializeComponent();
	}

	private void Update_Clicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("UpdateSubjectPage", new Dictionary<string, object>
		{
			["subject"] = SubjectDTOModel
		});	
	}
}