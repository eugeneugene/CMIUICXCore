using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace TestF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] Strings = new string[] { "1001 -> 1002", "1004 -> 9106101F", "", "1003-> 9106101F ", "9999 -> 8888" };
            foreach (var s in Strings)
            {
                string[] numbers = s.Split("->")
                    .Select(item1 => item1?.Trim() ?? string.Empty)
                    .Select(item2 => item2.TrimEnd('F', 'f'))
                    .Select(item => item.Equals("9999", StringComparison.InvariantCultureIgnoreCase) ? "XXXX" : item)
                    .ToArray();

                string Number1 = numbers.Length >= 1 ? numbers[0] : string.Empty;
                string Number2 = numbers.Length >= 2 ? numbers[1] : string.Empty;

                Debug.WriteLine($"'{Number1}' -> '{Number2}'");
                Assert.IsFalse(Number1.Contains(' '));
                Assert.IsFalse(Number2.Contains(' '));
                Assert.IsFalse(Number1.Contains('F'));
                Assert.IsFalse(Number2.Contains('F'));
                Assert.IsFalse(Number1.Contains('f'));
                Assert.IsFalse(Number2.Contains('f'));
                Assert.IsFalse(Number1.Equals("9999", StringComparison.InvariantCultureIgnoreCase));
                Assert.IsFalse(Number2.Equals("9999", StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}
