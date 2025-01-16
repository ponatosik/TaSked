using TaSked.Domain;

namespace TaSked.App.Components;

public partial class InvintationCard : ContentView
{
    public static readonly BindableProperty InvintationModelProperty =
        BindableProperty.Create(nameof(InvintationModel), typeof(Invitation), typeof(InvintationCard));

    public Invitation InvintationModel
    {
        get => (Invitation)GetValue(InvintationModelProperty);
    }

    public InvintationCard()
	{
		InitializeComponent();
	}

    public void CopyInvitation_Clicked(object sender, EventArgs e)
    {
        if (InvintationModel != null)
        {
            Clipboard.SetTextAsync(InvintationModel.Id.ToString());
        }
    }
    
    public void GenerateLinkAndCopy(object sender, EventArgs e)
    {
        if (InvintationModel != null)
        {
            string link = $"https://tasked.com/group/join?invitationId={InvintationModel.Id}";
            Clipboard.SetTextAsync(link);
        }
    }
}