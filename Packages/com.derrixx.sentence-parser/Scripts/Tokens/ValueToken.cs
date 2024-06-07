namespace Tokens
{
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