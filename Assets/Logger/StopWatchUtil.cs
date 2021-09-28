
using System;
using System.Diagnostics;

public class StopWatchUtil
{
  private    Stopwatch stopWatch;
    private string name;
    public StopWatchUtil(string name) {
        stopWatch = new Stopwatch();
        this.name = name;
    }

    public void Start(string message = "") {
        if (message != "") {
            LoggerManager.GetInstance().Log(name + " " + TakeFormattedTime() + ": " + message);
        }
        stopWatch.Start();
    }
    public void Stop(string message = "") {
        if (message != "")
        {
            LoggerManager.GetInstance().Log(name + " " + TakeFormattedTime() + ": " +message);
        }
        stopWatch.Stop();
    }
    public void Reset(string message ="") {
        if (message != "")
        {
            LoggerManager.GetInstance().Log(name + " " + TakeFormattedTime() + ": " + message);
        }
        LoggerManager.GetInstance().Log(name + " " + "stopwatch resetting");
        stopWatch.Stop();
        stopWatch = new Stopwatch();
    }
    private string TakeFormattedTime() {

         TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        return string.Format("{0:00}h {1:00}m {2:00}s {3:00}ms",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
    }
}
