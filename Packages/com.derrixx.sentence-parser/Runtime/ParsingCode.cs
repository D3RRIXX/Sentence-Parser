using System;

namespace SentenceParser
{
	[Serializable]
	public class ParsingCode
	{
		public string Code;
		public int Priority;
	
		public ParsingCode() {}

		public ParsingCode(string code, int priority = 0)
		{
			Code = code;
			Priority = priority;
		}
	}
}