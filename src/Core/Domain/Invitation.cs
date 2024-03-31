using Domain.Exceptions;

namespace TaSked.Domain;

public class Invitation
{
	public Guid Id { get; set; }
	public Guid GroupId { get; set; }
	public string? Caption { get; set; }
	public bool IsExpired {  get; private set; }
	public DateTime? ExpirationDate { get; private set; }
	public int? MaxActivations {  get; private set; }
	public int ActivationCount { get; private set; } = 0;

	private Invitation() { }
	internal Invitation(Guid id, Guid groupId, string? caption = null, int? maxActivation = null, DateTime? expirationDate = null) 
	{
		Id = id;
		GroupId = groupId;
		Caption = caption;
		MaxActivations = maxActivation;
		ExpirationDate = expirationDate;
	}

	internal void ActivateOne()
	{
		if (CheckExpired()) 
		{
			throw new InvitationExpiredException(this);
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
