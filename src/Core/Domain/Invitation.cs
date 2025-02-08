using Domain.Exceptions;

namespace TaSked.Domain;

public class Invitation
{
	public Guid Id { get; init; }
	public Guid GroupId { get; init; }
	public string? Caption { get; set; }
	public bool IsExpired { get; private set; }
	public DateTime? ExpirationDate { get; init; }
	public int? MaxActivations { get; init; }
	public int ActivationCount { get; private set; }

	private Invitation() { }

	private Invitation(Guid id)
	{
		Id = id;
	}

	internal Invitation(Guid id, Guid groupId, string? caption = null, int? maxActivation = null, DateTime? expirationDate = null)
		: this(id)
	{
		GroupId = groupId;
		Caption = caption;
		MaxActivations = maxActivation;
		ExpirationDate = expirationDate?.ToUniversalTime();
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

		if (ActivationCount >= MaxActivations || DateTime.UtcNow > ExpirationDate)
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
