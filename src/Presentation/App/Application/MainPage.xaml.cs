namespace TaSked.App;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private void JoinGroupTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//JoinGroupPage");
    }

    private void CreateGroupTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CreateGroupPage");
    }
}
