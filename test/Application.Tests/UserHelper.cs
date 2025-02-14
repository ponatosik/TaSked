using System.Runtime.CompilerServices;

namespace Application.Tests;

public static class UserHelper
{
	private static int _counter;

	public static string GenerateUniqueUserName(
		string userName = "Test user",
		[CallerMemberName] string caller = "",
		[CallerFilePath] string callerFile = "")
	{
		var counter = Interlocked.Increment(ref _counter);
		return $"{userName} ({callerFile}-{caller}-{counter})";
	}
}