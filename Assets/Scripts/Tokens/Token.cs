namespace Tokens
{
	public abstract class Token
	{
		public abstract bool Evaluate();
	}

	public class OrToken : Token
	{
		private readonly Token _left;
		private readonly Token _right;

		public OrToken(Token left, Token right)
		{
			_left = left;
			_right = right;
		}

		public override bool Evaluate() => _left.Evaluate() || _right.Evaluate();
	}
	
	public class NotToken : Token
	{
		private readonly Token _inner;

		public NotToken(Token inner)
		{
			_inner = inner;
		}
		
		public override bool Evaluate() => !_inner.Evaluate();
	}
	
	public class OpenParenToken : Token
	{
		public override bool Evaluate() => true;
	}
	
	public class CloseParenToken : Token
	{
		public override bool Evaluate() => false;
	}
	
	public class ValueToken : Token
	{
		private readonly bool _value;

		public ValueToken(bool value)
		{
			_value = value;
		}
		
		public override bool Evaluate() => _value;
	}
}