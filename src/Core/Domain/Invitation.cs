namespace TaSked.Domain;

public class Invinatation
{
	public int Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string? Caption { get; private set; }
	public bool IsExpired {  get; private set; }
	public DateTime ExpirationData { get; private set; }
	public int MaxActivations {  get; private set; }
	public int ActivationCount { get; set; } = 0;
	private Invinatation() { }
}
