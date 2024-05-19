using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class SentenceParserUtils
{
	public static bool ParseSentence(string inputSentence, IEnumerable<ParsingCode> parsingCodes, out string resultingCode)
	{
		foreach (ParsingCode parsingCode in parsingCodes.OrderByDescending(x => x.Priority))
		{
			if (EvaluateParsingCode(inputSentence, parsingCode.Code))
			{
				resultingCode = parsingCode.Code;
				return true;
			}
		}

		resultingCode = null;
		return false;
	}

	private static bool EvaluateParsingCode(string sentence, string code)
	{
		// Find all groups in code
		// Evaluate their internals
		// Process their sign
		// Do the rest normally

		const string parseGroupsPattern = @"!?\(([^)]+)\)";
		MatchCollection groups = Regex.Matches(code, parseGroupsPattern);
		foreach (Match group in groups)
		{
			bool shouldMatch = group.Value[0] is not '!';
			string capture = group.Groups[1].Value;

			if (AllOperatorsMatch(sentence, capture) != shouldMatch)
				return false;
		}

		return AllOperatorsMatch(sentence, code);
	}

	private static bool AllOperatorsMatch(string sentence, string code)
	{
		var isMatch = code.Split(' ').Select(ConvertToRegex).All(tuple => Regex.IsMatch(sentence, tuple.pattern) == tuple.shouldMatch);
		return isMatch;
	}

	public static (string pattern, bool shouldMatch) ConvertToRegex(string parseOperator)
	{
		string regex = parseOperator;
		bool shouldMatch = parseOperator[0] switch
		{
			'&' => true,
			'!' => false,
			_ => throw new ArgumentOutOfRangeException()
		};

		regex = Regex.Replace(regex, @"(\w+)\[(.*)\]", @"$1(?:$2)?");
		regex = Regex.Replace(regex, @"[&!]([^&].*)", @"(\b$1\b)");
		// regex = Regex.Replace(regex, @"\\\[(\w+)\]", @"$1?s?");
		// regex = Regex.Replace(regex, @"\(([^)]+)\)", m =>
		// {
		// 	string[] options = m.Groups[1].Value.Split('/');
		// 	return "(" + string.Join("|", options) + ")";
		// });
		// regex = Regex.Replace(regex, @"!(\w+)", @"(?!.*\b$1\b)");

		return (pattern: regex, shouldMatch);
	}
}