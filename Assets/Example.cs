using SentenceParser;
using UnityEngine;

public class Example : MonoBehaviour
{
	void Start()
	{
		const string sentence = "A quick brown fox jumps over a lazy dog";
		var codes = new ParsingCode[]
		{
			new("&quick"),
			new("&bro[wn/ad]"),
			new("!red"),
			new("!brown | !red", priority: 10),
			new("!(!brown &red) &dog[s] | &fast"),
		};

		if (SentenceParserTool.ParseSentence(sentence, codes, out string resultingCode))
		{
			Debug.Log($"Found match for parsing code '{resultingCode}'");
		}
		else
		{
			Debug.LogError("No matching code found!");
		}
	}
}