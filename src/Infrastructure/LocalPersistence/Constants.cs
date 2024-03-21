namespace TaSked.Infrastructure.LocalPersistence;

public static class Constants
{
	public const string DatabaseFilename = "HomeworkSQLite.db3";

	public const SQLite.SQLiteOpenFlags Flags =
		SQLite.SQLiteOpenFlags.ReadWrite |
		SQLite.SQLiteOpenFlags.Create |
		SQLite.SQLiteOpenFlags.SharedCache;

	public const SQLite.CreateFlags CreateFlags = 
		SQLite.CreateFlags.ImplicitPK | 
		SQLite.CreateFlags.AutoIncPK;

	public static string DatabasePath(string folder) =>
		Path.Combine(folder, DatabaseFilename);
}