namespace SentenceParser.Tokens
{
	public class NotToken : OperatorToken
	{
		public Token Inner { get; set; }
		public override int Precedence => 2;
		public override bool Evaluate() => !Inner.Evaluate();
		public override string ToString() => "!";
	}
}