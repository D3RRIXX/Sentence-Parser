using System;
using NUnit.Framework;

namespace Tests.EditMode
{
	public class CodeToRegexConversionTests
	{
		[TestCase("&prefer", @"(\bprefer\b)")]
		[TestCase("&like", @"(\blike\b)")]
		public void Test_Ampersand(string code, string regex)
		{
			string pattern = SentenceParserUtils.ConvertToRegex(code);
			Assert.AreEqual(regex, pattern);
		}

		[TestCase("&like[ness]", @"(\blike(?:ness)?\b)")]
		public void Test_Optional(string code, string regex)
		{
			string pattern = SentenceParserUtils.ConvertToRegex(code);
			Assert.AreEqual(regex, pattern);
		}

		[Test]
		public void Test_Should_Throw_On_Invalid_Input()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => SentenceParserUtils.ConvertToRegex("no-sign"));
		}
	}
}