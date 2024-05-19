namespace Tokens
{
	public abstract class Token
	{
		public abstract bool Evaluate();
	}

	public abstract class OperatorToken : Token
	{
		public abstract int Precedence { get; }
	}

	public class OrToken : OperatorToken
	{
		public override int Precedence => 1;
		public Token Left { get; set; }
		public Token Right { get; set; }
		public override string ToString() => "|";
		public override bool Evaluate() => Left.Evaluate() || Right.Evaluate();
	}
	
	public class NotToken : OperatorToken
	{
		public Token Inner { get; set; }
		public override int Precedence => 2;
		public override bool Evaluate() => !Inner.Evaluate();
		public override string ToString() => "!";
	}
	
	public class OpenParenToken : OperatorToken
	{
		public override int Precedence => 0;
		public override string ToString() => "(";
		public override bool Evaluate() => true;
	}

	public class ValueToken : Token
	{
		private readonly bool _value;
		private readonly string _input;

		public ValueToken(string input, bool value)
		{
			_value = value;
			_input = input;
		}

		public override bool Evaluate() => _value;
		public override string ToString() => _input;
	}
}