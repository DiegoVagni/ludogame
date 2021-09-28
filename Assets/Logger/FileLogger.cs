using System.IO;
using System.Threading;
//si lo so servirebbe fare un parent;
public class FileLogger : Logger
{
	
	private bool logging = true;
	private StreamWriter w;

	public FileLogger(string filename) : base() {
		
		w = File.AppendText(filename);
	}
	public override void WriteLog() {
	
		while (logging || messages.Count > 0) {
			lock (messages) { 
			while (messages.Count > 0) { 
			w.WriteLine(messages.Dequeue() + "\n");
			}
			}
				Thread.Sleep(250);
		}
		w.Dispose();
	}

	public override void Log(object message) {

		lock (messages)
		{
			messages.Enqueue(message.ToString());
		}
	}
	public void Stop() {
		logging = false;
	}
}
