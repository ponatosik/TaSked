namespace TaSked.App.Common;

public class AuthenticationException : Exception
{
	public AuthenticationException(string message) : base(message) { }
}