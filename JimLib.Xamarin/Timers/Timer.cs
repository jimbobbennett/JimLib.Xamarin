using System;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Timers
{
    public class Timer : ITimer
    {
        public void StartTimer(TimeSpan timeSpan, Func<Task<bool>> timerFunc)
        {
            Task.Factory.StartNew(async () =>
                {
                    while (await timerFunc())
                        await Task.Delay(timeSpan);
                });
        }
    }
}
