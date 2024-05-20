using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class SentenceParserEditorWindow : EditorWindow
{
	private const string STYLE_ERROR_FIELD = "error-field";
	
	[SerializeField] private VisualTreeAsset _editorUxml;
	[SerializeField] private VisualTreeAsset _parsingCodeItemUxml;

	private readonly List<ParsingCode> _parsingCodes = new();
	private string _inputSentence;
	private ReorderableList _list;
	private bool _inputEmpty;
	private TextField _textField;

	[MenuItem("Tools/Sentence Parser", priority = -100)]
	private static void GetWindow()
	{
		GetWindow<SentenceParserEditorWindow>("Sentence Parser");
	}

	private void CreateGUI()
	{
		_editorUxml.CloneTree(rootVisualElement);

		_textField = rootVisualElement.Q<TextField>("input-sentence");
		_textField.value = _inputSentence;
		_textField.RegisterValueChangedCallback(evt =>
		{
			_inputSentence = evt.newValue;
			_textField.RemoveFromClassList(STYLE_ERROR_FIELD);
		});

		var listView = rootVisualElement.Q<ListView>();
		listView.itemsSource = _parsingCodes;
		listView.makeItem = _parsingCodeItemUxml.CloneTree;
		listView.bindItem = BindItem;
		listView.itemsAdded += ints =>
		{
			foreach (int i in ints)
			{
				_parsingCodes[i] = new ParsingCode();
			}
		};

		rootVisualElement.Q<Button>("button-parse").clicked += OnParseSentence;
	}

	private void BindItem(VisualElement element, int idx)
	{
		element.Q<TextField>().RegisterValueChangedCallback(evt => _parsingCodes[idx].Code = evt.newValue);
		element.Q<IntegerField>().RegisterValueChangedCallback(evt => _parsingCodes[idx].Priority = evt.newValue);
	}

	private void OnParseSentence()
	{
		if (string.IsNullOrEmpty(_inputSentence))
		{
			_textField.AddToClassList(STYLE_ERROR_FIELD);
			Debug.LogError("Input sentence is empty!");
			return;
		}

		bool parseResults = SentenceParserUtils.ParseSentence(_inputSentence, _parsingCodes, out string resultingCode);
		string log = parseResults ? $"Match found for code \"{resultingCode}\"" : "No matches found";
		Debug.Log(log);
	}
}