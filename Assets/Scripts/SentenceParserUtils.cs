using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class SentenceParserUtils
{
	public static string ParseSentence(string inputSentence, IEnumerable<ParsingCode> parsingCodes)
	{
		foreach (var parsingCode in parsingCodes.OrderByDescending(x => x.Priority))
		{
			Match match = EvaluateParsingCode(inputSentence, parsingCode.Code);
			return match.Value;
		}

		return null;
	}

	private static Match EvaluateParsingCode(string sentence, string code)
	{
		string pattern = ConvertToRegex(code);
		return Regex.Match(sentence, pattern);
	}

	private static string ConvertToRegex(string parsingCode)
	{
		string regex = parsingCode;

		regex = regex.Replace(" ", @"\s*");
		regex = Regex.Replace(regex, @"&(\w+)", @"(?=.*\b$1\b)");
		regex = Regex.Replace(regex, @"\[(\w+)\]", @"$1?");
		regex = Regex.Replace(regex, @"\(([^)]+)\)", m =>
		{
			string[] options = m.Groups[1].Value.Split('/');
			return "(" + string.Join("|", options) + ")";
		});
		regex = Regex.Replace(regex, @"!(\w+)", @"(?!.*\b$1\b)");

		return regex;
	}
}