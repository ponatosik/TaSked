using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App.Components;

public partial class SubjectCard : ContentView
{
	public static readonly BindableProperty ViewModelProperty =
		BindableProperty.Create(nameof(ViewModel), typeof(SubjectViewModel), typeof(SubjectCard));

	public SubjectViewModel ViewModel
	{
		get => (SubjectViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public SubjectCard()
	{
		InitializeComponent();
	}

}
