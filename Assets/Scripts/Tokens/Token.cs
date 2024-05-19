namespace Tokens
{
	public abstract class Token
	{
	}

	public class OrToken : Token
	{
		public OrToken()
		{
		}
		
		public bool Evaluate(bool left, bool right) => left || right;
		public override string ToString() => "|";
	}
	
	public class NotToken : Token
	{
		public bool Evaluate(bool value) => value;
		public override string ToString() => "!";
	}
	
	public class OpenParenToken : Token
	{
		public override string ToString() => "(";
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

		public bool Evaluate() => _value;
		public override string ToString() => _input;
	}
}