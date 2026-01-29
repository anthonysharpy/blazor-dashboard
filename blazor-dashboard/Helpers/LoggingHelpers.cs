namespace blazor_dashboard;

public static class LoggingHelpers
{
	public static void LogEvent(string message)
	{
		PretendDatabase.LogEvent(new EventLogEntry
		{
			EventTime = DateTime.Now,
			Description = message
		});
	}
}

