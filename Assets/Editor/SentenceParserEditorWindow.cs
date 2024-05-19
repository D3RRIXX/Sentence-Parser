using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class SentenceParserEditorWindow : EditorWindow
{
	private string _inputSentence;
	private List<ParsingCode> _parsingCodes = new();
	private ReorderableList _list;
	private bool _inputEmpty;

	[MenuItem("Tools/Sentence Parser", priority = -100)]
	private static void GetWindow()
	{
		GetWindow<SentenceParserEditorWindow>("Sentence Parser");
	}

	private void OnEnable()
	{
		_list = new ReorderableList(_parsingCodes, typeof(ParsingCode))
		{
			drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Parsing Codes"),
			drawElementCallback = DrawElement,
			elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing,
		};
	}

	private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
	{
		const int fieldSpacing = 5;
		float singleLineHeight = EditorGUIUtility.singleLineHeight;

		ParsingCode parsingCode = _parsingCodes[index];
		rect.y += 2;

		var prefixLabel = new GUIContent("Code");
		EditorGUI.PrefixLabel(rect, prefixLabel);
		Vector2 size = GUI.skin.label.CalcSize(prefixLabel);

		// rect.x += size.x + fieldSpacing;

		GUIStyle style = GUI.skin.textField;
		
		parsingCode.Code = EditorGUI.TextField(new Rect(rect.x + size.x + fieldSpacing, rect.y, rect.width - size.x - fieldSpacing, singleLineHeight), parsingCode.Code, style);
		parsingCode.Priority = EditorGUI.IntField(new Rect(rect.x, rect.y + EditorGUIUtility.standardVerticalSpacing + singleLineHeight, rect.width, singleLineHeight), "Priority", parsingCode.Priority);
	}

	private void OnGUI()
	{
		var style = GUI.skin.textArea;
		style.wordWrap = true;

		EditorGUILayout.PrefixLabel("Input Sentence");
		
		_inputSentence = EditorGUILayout.TextArea(_inputSentence, style);

		EditorGUILayout.Space(10);

		_list.DoLayoutList();
		
		EditorGUILayout.Space(10);

		if (GUILayout.Button("Parse Sentence", GUILayout.Height(30)))
		{
			if (string.IsNullOrEmpty(_inputSentence))
			{
				_inputEmpty = true;
				Debug.LogError("Input sentence is empty!");
				return;
			}

			string parseResults = SentenceParserUtils.ParseSentence(_inputSentence, _parsingCodes);
			EditorUtility.DisplayDialog("Parsing Results", string.IsNullOrEmpty(parseResults) ? "No results found." : parseResults, "OK");
		}
	}
}