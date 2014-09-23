using System;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Timers
{
    public interface ITimer
    {
        void StartTimer(TimeSpan timeSpan, Func<Task<bool>> timerFunc);
    }
}