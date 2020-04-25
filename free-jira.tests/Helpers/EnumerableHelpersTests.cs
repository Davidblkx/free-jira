using System.Collections.Generic;
using FreeJira.Helpers;
using Xunit;

namespace FreeJira.tests.Helpers
{
    public class EnumerableHelpersTests
    {
        [Fact]
        public void JoinStringTest() {
            var expected = "batatas/3000/get";
            var case1 = new List<string>{ "batatas", "3000", "get" };
            var case2 = new List<string>{ "/batatas/", "/3000/", "/get/" };
            var case3 = new List<string>{ "//batatas//", "//3000//", "//get//" };

            Assert.Equal(expected, case1.JoinString('/'));
            Assert.Equal(expected, case2.JoinString('/'));
            Assert.Equal(expected, case3.JoinString('/'));
        }
    }
}