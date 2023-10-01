namespace blazor_dashboard;

// Feels a bit weird having this code in a helper class but didn't really
// have anywhere better to put it.
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

