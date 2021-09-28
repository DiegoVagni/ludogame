

using System.Collections.Generic;

public abstract class Logger 
{
	protected Queue<string> messages;

	public Logger() {
		messages = new Queue<string>();
	}
	public abstract void WriteLog();
	public abstract void Log(object message);
}
