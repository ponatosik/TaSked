using TaSked.Domain;

namespace TaSked.Application;

public interface IJwtProvider
{
	string Generate(User user);
}
