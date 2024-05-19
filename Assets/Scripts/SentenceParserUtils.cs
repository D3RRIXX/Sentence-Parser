﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
		var isMatch = code.Split(' ').Select(ConvertToRegex).All(tuple => Regex.IsMatch(sentence, tuple.pattern) == tuple.shouldMatch);
		return isMatch;
	}

	public static (string pattern, bool shouldMatch) ConvertToRegex(string parsingCode)
	{
		string regex = parsingCode;
		bool shouldMatch = parsingCode[0] switch
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