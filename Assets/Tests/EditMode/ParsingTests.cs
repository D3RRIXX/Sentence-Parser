using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
	public class ParsingTests
	{
		private const string INPUT_SENTENCE = "I like bananas, but I prefer strawberries";

		[TestCase("&like")]
		[TestCase("&prefer")]
		[TestCase("&like &prefer")]
		[TestCase("&I")]
		public void Test_And_Sign_Matches(string code)
		{
			Assert.IsTrue(SentenceParserUtils.ParseSentence(INPUT_SENTENCE, new[] { new ParsingCode(code) }, out _));
		}

		[TestCase("&banana[s]")]
		[TestCase("&prefer[s]")]
		[TestCase("&like[ness]")]
		public void Test_Matches_Optional_Chars(string code)
		{
			Assert.IsTrue(SentenceParserUtils.ParseSentence(INPUT_SENTENCE, new[] { new ParsingCode(code) }, out _));
		}

		[TestCase("&strawberr[y/ies]")]
		public void Test_Matches_Optional_Chars_Multiple(string code)
		{
			Assert.IsTrue(SentenceParserUtils.ParseSentence(INPUT_SENTENCE, new[] { new ParsingCode(code) }, out _));
		}
	}
}