using System;
using FreeJira.Helpers;
using Xunit;

namespace FreeJira.tests.Helpers
{
    public class UriHelpersTests
    {
        [Fact]
        public void UriFromStringTest() {
            var host = "https://davidpires.pt";
            var part1 = "batatas";
            var part2 = "/3000";
            var url = "https://davidpires.pt/batatas/3000";

            var expectedUri = new Uri(url);
            var actualUri = UriHelpers.UriFromStrings(host, part1, part2);

            Assert.NotNull(actualUri);
            Assert.Equal(expectedUri.AbsoluteUri, actualUri.AbsoluteUri);
        }

        [Fact]
        public void UriFromStringTestException() {
            Assert.ThrowsAny<Exception>(() => UriHelpers.UriFromStrings());
        }
    }
}