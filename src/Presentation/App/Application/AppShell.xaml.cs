﻿using TaSked.App.Application;
using TaSked.App.Common;

namespace TaSked.App;

public partial class AppShell : Shell
{
	private SettingsPage _settingsPage;

	public AppShell()
	{
		_settingsPage = ServiceHelper.GetService<SettingsPage>();
		InitializeComponent();
		Routing.RegisterRoute("MainPage/CreateGroupPage", typeof(CreateGroupPage));
		Routing.RegisterRoute("MainPage/JoinGroupPage", typeof(JoinGroupPage));
		Routing.RegisterRoute("ReportPage/CreateReportPage", typeof(CreateReportPage));
		Routing.RegisterRoute("SubjectPage/CreateSubjectPage", typeof(CreateSubjectPage));
		Routing.RegisterRoute("SubjectPage/UpdateSubjectPage", typeof(UpdateSubjectPage));

		Routing.RegisterRoute("UncompletedTasksPage/CreateTaskPage", typeof(CreateTaskPage));
	}

	void OnSettingsClicked(object sender, EventArgs e)
	{
		if (Navigation.NavigationStack.Last() == _settingsPage)
		{
			return;
		}

		Navigation.PushAsync(_settingsPage);
	}
}
