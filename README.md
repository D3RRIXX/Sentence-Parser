# Unity Sentence Parser Tool
## Project Description
The Unity Sentence Parser Tool is designed to parse input sentences using a set of customizable codes. These codes allow for flexible and complex parsing conditions, making this tool a powerful asset for developers working with natural language processing within Unity.

## Features
* Parse sentences using a variety of codes.
* Combine and prioritize parsing codes.
* Return the code with the highest priority that meets its condition.

## Syntax for Parsing Codes
The syntax for the parsing codes is as follows:

* `&word` - The word must be present in the sentence.
* `word[xy/z]` - Placed at the end of the word. Letters in square brackets are optional within the word, and multiple optional variants can be specified within the brackets, separated by '/'.
* `!word` - The word must NOT be present in the sentence.
* `condition1 | condition2` - Any of the conditions must be met.
* `!(&word !another)` - Parentheses group conditions together into one.

You can combine codes in any way and assign priorities to them. The tool will return the code with the highest priority whose condition is met.

## Example
Consider the sentence: "A quick brown fox jumps over a lazy dog"

Given the following codes:

* `&quick`
* `&bro[wn/ad]`
* `!red`
* `!brown | !red`
* `!(!brown &red) &dog[s] | &fast`

All the codes listed above are true, meaning the conditions specified within them are met in the given sentence.

## Getting Started

### Prerequisites
* Unity 2021.1 or later

### Installation
Install via Unity Package Manager:
   1. Open UPM
   2. Select `Add package > Add package from git URL`
   3. Paste `https://github.com/D3RRIXX/Sentence-Parser.git?path=Packages/com.derrixx.sentence-parser`

### Usage
1. Import the Sentence Parser Tool into your Unity project.
2. Use the provided API to define your parsing codes and input sentences.
3. Execute the parser to retrieve the highest priority code whose condition is met.

## Example Code
Here’s a basic example of how to use the Sentence Parser Tool:

```cs
using SentenceParser;
using UnityEngine;

public class Example : MonoBehaviour
{
	void Start()
	{
		string sentence = "A quick brown fox jumps over a lazy dog";
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
```
## Contributing
We welcome contributions to improve the Unity Sentence Parser Tool. Please fork the repository and submit pull requests.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.