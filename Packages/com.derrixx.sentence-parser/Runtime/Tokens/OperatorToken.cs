﻿namespace SentenceParser.Tokens
{
	public abstract class OperatorToken : Token
	{
		public abstract int Precedence { get; }
	}
}