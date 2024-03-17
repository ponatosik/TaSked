namespace TaSked.App;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("MainPage/CreateGroupPage", typeof(CreateGroupPage));
		Routing.RegisterRoute("MainPage/JoinGroupPage", typeof(JoinGroupPage));
		Routing.RegisterRoute("ReportPage/CreateReportPage", typeof(CreateReportPage));
	}
}
