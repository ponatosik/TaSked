using TaSked.Domain;

namespace TaSked.App.Components;

public partial class ReportCard : ContentView
{
	public static readonly BindableProperty ReportModelProperty =
		BindableProperty.Create(nameof(ReportModel), typeof(Report), typeof(ReportCard));

	public Report ReportModel
	{
		get => (Report)GetValue(ReportModelProperty);
		set => SetValue(ReportModelProperty, value);
	}

	public ReportCard()
	{
		InitializeComponent();
	}
}