namespace TaSked.Application.Exceptions;

public class EntityNotFoundException : ApplicationException
{
	public Guid EntityId { get; private set; }
	public string EntityName { get; private set; }
	
	private static string GenerateMessage(Guid id, string name) => 
		$"{name} with id \"{id}\" was not found.";

	internal EntityNotFoundException(Guid entityId, string entityName) 
		: base(GenerateMessage(entityId, entityName))
	{
		EntityId = entityId;
		EntityName = entityName;
	}
	internal EntityNotFoundException(Guid entityId, string entityName, Exception inner)
		: base(GenerateMessage(entityId, entityName), inner)
	{
		EntityId = entityId;
		EntityName = entityName;
	}
}
