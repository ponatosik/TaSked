using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace TaSked.Infrastructure.Persistence.AmazonBeanstalkRds;

public static class DbContextOptions
{
	public static void UseAmazonRds(this DbContextOptionsBuilder options, IConfiguration configuration)
	{
		var dbname = configuration["RDS_DB_NAME"]!;
		var username = configuration["RDS_USERNAME"]!;
		var password = configuration["RDS_PASSWORD"]!;
		var hostname = configuration["RDS_HOSTNAME"]!;
		var port = configuration["RDS_PORT"]!;
		options.UseNpgsql(FormatConnectionString(dbname, username, password, hostname, port));
	}

	public static bool IsRdsConfigured(this IConfiguration configuration)
	{
		return configuration["RDS_DB_NAME"] is not null;
	}

	private static string FormatConnectionString(
		string rdsDbName,
		string rdsUser,
		string rdsPassword,
		string rdsHost,
		string rdsPort)
	{
		var connectionStringBuilder = new DbConnectionStringBuilder
		{
			{ "Data Source", rdsHost },
			{ "Initial Catalog", rdsDbName },
			{ "User Id", rdsUser },
			{ "Password", rdsPassword },
			{ "Port", rdsPort }
		};
		return connectionStringBuilder.ToString();
	}
}