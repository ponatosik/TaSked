using TaSked.Application.Common;
using TaSked.Domain;

namespace TaSked.Application;

public class PartialUpdateSubjectDTO
{
	public Guid Id { get; private set; }
	public Optional<Guid> GroupId { get; private set; }
	public Optional<string> Name { get; set; }
	public Optional<Teacher> Teacher { get; set; }
}
