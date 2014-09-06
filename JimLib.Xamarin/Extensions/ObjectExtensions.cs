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
        public static async Task<T> InvokeOnMainThreadAsync<T>(this object o, Func<Task<T>> func)
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

        // ReSharper disable once UnusedParameter.Global
        public static async Task InvokeOnMainThreadAsync(this object o, Func<Task> func)
        {
            await Task.Run(() =>
            {
                var done = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await func();
                    done = true;
                });

                new object().WaitForAsync(() => done, int.MaxValue);
            });
        }

        // ReSharper disable once UnusedParameter.Global
        public static void InvokeOnMainThread(this object o, Action action)
        {
            var done = false;
            Device.BeginInvokeOnMainThread(() =>
                {
                    action();
                    done = true;
                });

            new object().WaitForAsync(() => done, int.MaxValue);
        }

        // ReSharper disable once UnusedParameter.Global
        public static void BeginInvokeOnMainThread(this object o, Action action)
        {
            Task.Run(() => o.InvokeOnMainThread(action));
        }
    }
}
