using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tokens;
using UnityEngine;

public static class SentenceParserUtils
{
	public static bool ParseSentence(string inputSentence, IEnumerable<ParsingCode> parsingCodes, out string resultingCode)
	{
		foreach (ParsingCode parsingCode in parsingCodes.OrderByDescending(x => x.Priority))
		{
			if (string.IsNullOrEmpty(parsingCode.Code))
				continue;
			
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
		var postfix = ConvertWriting(sentence, code);
		var list = new List<Token>(postfix);
		Debug.Log($"Converted writing: {string.Join(", ", list.Select(x => x.ToString()))}");
		bool output = false;
		
		// while (postfix.Count > 0)
		// {
		// 	output |= ProcessToken(postfix.Pop(), postfix);
		// }
		//
		// return output;
		
		var stack = new Stack<Token>();

		foreach (var token in postfix)
		{
			switch (token)
			{
				case ValueToken:
					stack.Push(token);
					break;
				case NotToken notToken:
				{
					Token operand = stack.Pop();
					notToken.Inner = operand;
					stack.Push(notToken);
					break;
				}
				case OrToken orToken:
				{
					Token right = stack.Pop();
					Token left = stack.Pop();
					orToken.Left = left;
					orToken.Right = right;
					stack.Push(orToken);
					break;
				}
			}
		}

		return stack.Pop().Evaluate();
	}

	private static bool ProcessToken(Token token, Stack<Token> tokens)
	{
		switch (token)
		{
			case NotToken notToken:
				return notToken.Evaluate(ProcessToken(tokens.Pop(), tokens));
			case OrToken orToken:
			{
				bool right = ProcessToken(tokens.Pop(), tokens);
				bool left = ProcessToken(tokens.Pop(), tokens);
				return orToken.Evaluate(left, right);
			}
			case ValueToken valueToken:
				return valueToken.Evaluate();
			default:
				throw new ArgumentOutOfRangeException(nameof(token));
		}
	}

	private static Stack<Token> ConvertWriting(string sentence, string code)
	{
		// Find all groups in code
		// Evaluate their internals
		// Process their sign
		// Do the rest normally

		const string tokenPattern = @"&?([()!|]{1}|[\w\[\]\/]+)";
		Stack<Token> tokens = new();
		Stack<Token> operatorTokens = new();

		int i = 0;
		foreach (Match match in Regex.Matches(code, tokenPattern))
		{
			string matchValue = match.Groups[1].Value;
			Debug.Log($"[{i++}] {matchValue}");
			switch (matchValue)
			{
				case "(":
				{
					operatorTokens.Push(new OpenParenToken());
					break;
				}
				case ")":
				{
					while (operatorTokens.Count > 0 && operatorTokens.Peek() is not OpenParenToken)
					{
						tokens.Push(operatorTokens.Pop());
					}

					operatorTokens.Pop();
					break;
				}
				case "|":
				{
					while (operatorTokens.Count > 0 && operatorTokens.Peek() is OrToken)
					{
						tokens.Push(operatorTokens.Pop());
					}
					
					operatorTokens.Push(new OrToken());
					break;
				}
				case "!":
				{
					operatorTokens.Push(new NotToken());
					break;
				}
				default:
				{
					string pattern = ConvertToRegex(matchValue);
					tokens.Push(new ValueToken(matchValue, Regex.IsMatch(sentence, pattern)));
					break;
				}
			}
		}
		
		while (operatorTokens.Count > 0)
		{
			tokens.Push(operatorTokens.Pop());
		}

		return tokens;
	}

	public static string ConvertToRegex(string parseOperator)
	{
		string regex = parseOperator;

		regex = regex.Replace('/', '|');
		regex = Regex.Replace(regex, @"(\w+)\[(.*)\]", @"$1(?:$2)?");
		regex = Regex.Replace(regex, @"([^&].*)", @"(\b$1\b)");

		return regex;
	}
}