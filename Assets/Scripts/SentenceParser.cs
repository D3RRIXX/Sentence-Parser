using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tokens;
using UnityEngine;

public static class SentenceParser
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
		List<Token> postfix = ConvertWritingToPostfix(sentence, code);
		return EvaluatePostfixWriting(postfix);
	}

	public static bool EvaluatePostfixWriting(IEnumerable<Token> postfix)
	{
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

		bool output = true;
		while (stack.TryPop(out Token token))
		{
			bool b = token.Evaluate();
			output &= b;
		}
		return output;
	}

	private static List<Token> ConvertWritingToPostfix(string sentence, string code)
	{
		const string tokenPattern = @"&?([()!|]{1}|[\w\[\]\/]+)";
		List<Token> tokens = new();
		Stack<OperatorToken> operators = new();

		foreach (Match match in Regex.Matches(code, tokenPattern))
		{
			string matchValue = match.Groups[1].Value;
			switch (matchValue)
			{
				case "(":
				{
					operators.Push(new OpenParenToken());
					break;
				}
				case ")":
				{
					while (operators.Count > 0 && operators.Peek() is not OpenParenToken)
					{
						tokens.Add(operators.Pop());
					}

					operators.Pop();
					break;
				}
				case "|":
				{
					var orToken = new OrToken();
					while (operators.TryPeek(out OperatorToken token) && token is not OpenParenToken && orToken.Precedence <= token.Precedence)
					{
						tokens.Add(operators.Pop());
					}

					operators.Push(orToken);
					break;
				}
				case "!":
				{
					var notToken = new NotToken();
					while (operators.TryPeek(out OperatorToken token) && token is not OpenParenToken && notToken.Precedence <= token.Precedence)
					{
						tokens.Add(operators.Pop());
					}
					operators.Push(notToken);
					break;
				}
				default:
				{
					string pattern = ConvertToRegex(matchValue);
					tokens.Add(new ValueToken(matchValue, Regex.IsMatch(sentence, pattern)));
					if (operators.TryPeek(out OperatorToken operatorToken) && operatorToken is NotToken)
						tokens.Add(operators.Pop());
						
					break;
				}
			}
		}
		
		while (operators.Count > 0)
		{
			tokens.Add(operators.Pop());
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