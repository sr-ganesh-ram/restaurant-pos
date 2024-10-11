using System.Timers;
using Timer = System.Threading.Timer;

namespace Restaurant.Template.Utility;

public class JobExecutedEventArgs : EventArgs {}
public class JobStartEventArgs : EventArgs {}

public class PeriodicExecutor : IDisposable
{
    public PeriodicExecutor()
    {
        Console.WriteLine($"PeriodicExecutor Initialized, triggers every {Interval}, Milliseconds");
    }
    #region Event Register and Trigger

    public static int Interval = 300_000;
    public  event EventHandler<JobExecutedEventArgs> JobExecuted;
    public  event EventHandler<JobStartEventArgs> JobStarted;
    async Task OnJobExecutedAsync()
    {
        if (JobExecuted != null)
        {
            await Task.Run(() => JobExecuted.Invoke(this, new JobExecutedEventArgs()));
        }
    }
    async Task OnJobStartAsync()
    {
        if (JobStarted != null)
        {
            await Task.Run(() => JobStarted.Invoke(this, new JobStartEventArgs()));
        }
    }

    #endregion
    

    System.Timers.Timer _Timer;
    bool _Running;

    public async Task StartExecutingAsync()
    {
        if (!_Running)
        {
            // Initiate a Timer
            _Timer = new System.Timers.Timer();
            _Timer.Interval = Interval; // every 5 mins (300_000)
            _Timer.Elapsed += async (source, e) => await HandleTimerAsync(source, e);
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
            _Running = true;
            
        }
    }
    async Task HandleTimerAsync(object source, ElapsedEventArgs e)
    {
        await OnJobStartAsync();
        // Execute required job
        Console.WriteLine("Background Job Started");
        await Task.Delay(30_000);//Here we should perform some Ajax or background tasks.
        Console.WriteLine("Background Job Completed");
        // Notify any subscribers to the event
        await OnJobExecutedAsync();
    }

    public async Task DisposeAsync()
    {
        if (_Running)
        {
            // Clear up the timer
            await Task.Run(() => _Timer?.Dispose());
            _Running = false;
        }
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
}
