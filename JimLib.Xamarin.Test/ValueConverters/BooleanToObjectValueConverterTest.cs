using FluentAssertions;
using JimBobBennett.JimLib.Xamarin.ValueConverters;
using NUnit.Framework;

namespace JimLib.Xamarin.Test.ValueConverters
{
    [TestFixture]
    public class BooleanToObjectValueConverterTest
    {
        private static readonly object[] ConvertCases =
        {
            new object[]{ "Hello", "World", true, "Hello"},
            new object[]{ "Hello", "World", false, "World"},
            new object[]{ 1, 2, true, 1},
            new object[]{ 1, 2, false, 2}
        };

        [Test, TestCaseSource("ConvertCases")]
        public void Convert(object trueValue, object falseValue, bool value, object expected)
        {
            var converter = new BooleanToObjectValueConverter
            {
                TrueValue = trueValue,
                FalseValue = falseValue
            };
            converter.Convert(value, null, null, null).Should().Be(expected);
        }

        [Test, TestCaseSource("ConvertCases")]
        public void ConvertBack(object trueValue, object falseValue, bool expected, object value)
        {
            var converter = new BooleanToObjectValueConverter
            {
                TrueValue = trueValue,
                FalseValue = falseValue
            };
            converter.ConvertBack(value, null, null, null).Should().Be(expected);
        }
    }
}
