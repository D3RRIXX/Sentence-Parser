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