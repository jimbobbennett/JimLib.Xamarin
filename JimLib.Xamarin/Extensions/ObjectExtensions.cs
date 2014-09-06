using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Extensions;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Extensions
{
    [Pure]
    public static class ObjectExtensions
    {
// ReSharper disable once UnusedParameter.Global
        public static async Task<T> RunOnMainThreadAsync<T>(this object o, Func<Task<T>> func)
        {
            var retVal = default(T);

            await Task.Run(() =>
            {
                var done = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    retVal = await func();
                    done = true;
                });

                retVal.WaitForAsync(() => done, int.MaxValue);
            });

            return retVal;
        }
    }
}
