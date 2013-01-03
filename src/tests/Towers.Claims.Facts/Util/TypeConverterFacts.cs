using Towers.Claims.Util;
using Xunit;

namespace Towers.Claims.Facts.Util
{
    public class TypeConverterFacts
    {
        [Fact]
        public void ShouldReportConversionIsPossible()
        {
            Assert.True(TypeConverter.CanConvert(typeof(int), "5"));
        }
        [Fact]
        public void ShouldReportConversionIsPossibleUsingGenericOverload()
        {
            Assert.True(TypeConverter.CanConvert<int>("5"));
        }
        [Fact]
        public void ShouldConvert()
        {
            Assert.Equal(5, TypeConverter.Convert(typeof(int), "5"));
        }
        [Fact]
        public void ShouldConvertUsingGenericOverload()
        {
            Assert.Equal(5, TypeConverter.Convert<int>("5"));
        }
        [Fact]
        public void ShouldReportConversionIsNotPossible()
        {
            Assert.False(TypeConverter.CanConvert(typeof(int), "5,8"));
        }
        [Fact]
        public void ShouldCheckIfConversionIsPossibleUsingGenericOverload()
        {
            Assert.False(TypeConverter.CanConvert<int>("5,8"));
        }
    }
}
