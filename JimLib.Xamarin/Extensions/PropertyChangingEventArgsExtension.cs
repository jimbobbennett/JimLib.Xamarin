using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using JimBobBennett.JimLib.Extensions;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Extensions
{
    public static class PropertyChangingEventArgsExtension
    {
        /// <summary>
        /// Gets if a property change matches the given property name.
        /// This will also match any property to string.Empty as this is the standard way
        /// to indicate all properties have changed
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="args"></param>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        [Pure]
        public static bool PropertyNameMatches<TValue>(this PropertyChangingEventArgs args, Expression<Func<TValue>> propertyExpression)
        {
            return args.PropertyName == string.Empty || args.PropertyName == args.ExtractPropertyName(propertyExpression);
        }
    }
}
