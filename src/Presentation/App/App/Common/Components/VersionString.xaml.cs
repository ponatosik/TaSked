using TaSked.App.Common;

namespace TaSked.App.Components;

public partial class VersionString : ContentView
{
	public VersionString()
	{
		var versionTracking = ServiceHelper.GetService<IVersionTracking>();
		InitializeComponent();
		VersionStringLabel.Text = $"v{versionTracking.CurrentVersion}";
	}
}