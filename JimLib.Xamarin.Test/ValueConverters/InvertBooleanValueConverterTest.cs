using FluentAssertions;
using JimBobBennett.JimLib.Xamarin.ValueConverters;
using NUnit.Framework;

namespace JimLib.Xamarin.Test.ValueConverters
{
    [TestFixture]
    public class InvertBooleanValueConverterTest
    {
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase("", false)]
        [TestCase(14, false)]
        [TestCase(14.0, false)]
        public void Convert(object value, bool expected)
        {
            new InvertBooleanValueConverter().Convert(value, null, null, null).Should().Be(expected);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase("", false)]
        [TestCase(14, false)]
        [TestCase(14.0, false)]
        public void ConvertBack(object value, bool expected)
        {
            new InvertBooleanValueConverter().ConvertBack(value, null, null, null).Should().Be(expected);
        }
    }
}
