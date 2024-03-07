namespace TaSked.Domain;

public class Invitation
{
	public Guid Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string? Caption { get; private set; }
	public bool IsExpired {  get; private set; }
	public DateTime? ExpirationDate { get; private set; }
	public int? MaxActivations {  get; private set; }
	public int ActivationCount { get; private set; } = 0;

	private Invitation() { }
	internal Invitation(Guid id, Guid groupId, string? caption = null) 
	{
		Id = id;
		GroupId = groupId;
		Caption = caption;
	}

	internal void ActivateOne()
	{
		if (CheckExpired()) 
		{
			throw new Exception("Invitation is exired");
		}
		ActivationCount++;
	}

	public bool CheckExpired() 
	{
		if (IsExpired) 
		{
			return true;
		}
		if (MaxActivations is not null && ActivationCount >= MaxActivations)
		{
			Expire();
			return true;
		}
		if (DateTime.UtcNow > ExpirationDate)
		{
			Expire();
			return true;
		}
		return false;
	}

	public void Expire() 
	{
		IsExpired = true;
	}
}
