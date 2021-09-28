using System.Threading;

public class LoggerManager 
{
	private static FileLogger instance;
	private static string fileName = "log.txt";
	private static Thread thread;
	public static FileLogger GetInstance() {
		if (instance == null) {
			instance = new FileLogger(fileName);
			thread = new Thread(instance.WriteLog);
			thread.Start();
		}
		return instance;
	}
	public static void Stop() {
		instance.Stop();
		thread.Join();
	}
}
