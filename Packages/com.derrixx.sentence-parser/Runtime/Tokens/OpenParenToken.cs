namespace SentenceParser.Tokens
{
	public class OpenParenToken : OperatorToken
	{
		public override int Precedence => 0;
		public override string ToString() => "(";
		public override bool Evaluate() => true;
	}
}