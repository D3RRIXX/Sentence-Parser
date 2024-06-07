namespace SentenceParser.Tokens
{
	public class OrToken : OperatorToken
	{
		public override int Precedence => 1;
		public Token Left { get; set; }
		public Token Right { get; set; }
		public override string ToString() => "|";
		public override bool Evaluate() => Left.Evaluate() || Right.Evaluate();
	}
}