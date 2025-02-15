using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace TaSked.Infrastructure.Persistence.AzureMySqlInApp;

public static class DbContextOptionBuilderExtensions
{
	public static void UseAzureMysqlInApp(this DbContextOptionsBuilder options)
	{
		string? connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ApplicationException("MYSQLCONNSTR_localdb is not set");
		}
	
		string formattedConnectionString = ReformatConnectionString(connectionString);
		options.UseMySQL(formattedConnectionString);
	}

	private static string ReformatConnectionString(string connectionString)
	{
		DbConnectionStringBuilder inDbConnectionStringBuilder = new DbConnectionStringBuilder();
		inDbConnectionStringBuilder.ConnectionString = connectionString;

		string database = (string)inDbConnectionStringBuilder["Database"];
		string sourceIp = ((string)inDbConnectionStringBuilder["Data Source"])[..^6];
		string sourcePort = ((string)inDbConnectionStringBuilder["Data Source"])[^5..];
		string user = (string)inDbConnectionStringBuilder["User Id"];
		string password = (string)inDbConnectionStringBuilder["Password"];

		DbConnectionStringBuilder outDbConnectionStringBuilder = new DbConnectionStringBuilder
		{
			{ "Data Source", sourceIp },
			{ "Port", sourcePort },
			{ "Database", database },
			{ "User Id", user },
			{ "Password", password },
			{ "CharSet", "utf8mb4" }
		};

		return outDbConnectionStringBuilder.ConnectionString;
	}
}
