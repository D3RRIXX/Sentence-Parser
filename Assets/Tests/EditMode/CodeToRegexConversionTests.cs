using NUnit.Framework;

namespace Tests.EditMode
{
	public class CodeToRegexConversionTests
	{
		[TestCase("&prefer", @"(\bprefer\b)")]
		[TestCase("&like", @"(\blike\b)")]
		public void Test_Ampersand(string code, string regex)
		{
			Assert.IsTrue(SentenceParserUtils.ConvertToRegex(code) == regex);
		}

		[TestCase("&like[ness]", @"(\blike(?:ness)?\b)")]
		public void Test_Optional(string code, string regex)
		{
			Assert.AreEqual(regex, SentenceParserUtils.ConvertToRegex(code));
		}
	}
}