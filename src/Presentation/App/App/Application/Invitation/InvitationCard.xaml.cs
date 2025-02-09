using TaSked.Domain;

namespace TaSked.App.Components;

public partial class InvitationCard : ContentView
{
    public static readonly BindableProperty InvitationModelProperty =
        BindableProperty.Create(nameof(InvitationModel), typeof(Invitation), typeof(InvitationCard));

    public Invitation InvitationModel
    {
        get => (Invitation)GetValue(InvitationModelProperty);
    }

    public InvitationCard()
	{
		InitializeComponent();
	}

    public void CopyInvitation_Clicked(object sender, EventArgs e)
    {
        if (InvitationModel != null)
        {
            Clipboard.SetTextAsync(InvitationModel.Id.ToString());
        }
    }
    
    public void GenerateLinkAndCopy(object sender, EventArgs e)
    {
        if (InvitationModel != null)
        {
            string link = $"https://tasked.com/group/join?invitationId={InvitationModel.Id}";
            Clipboard.SetTextAsync(link);
        }
    }
}