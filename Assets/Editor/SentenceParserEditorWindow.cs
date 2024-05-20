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
	
	[SerializeField] private List<ParsingCode> _parsingCodes = new();
	[SerializeField] private string _inputSentence;

	private ReorderableList _list;
	private bool _inputEmpty;
	private TextField _textField;
	private SerializedObject _serializedObject;

	[MenuItem("Tools/Sentence Parser", priority = -100)]
	private static void GetWindow()
	{
		GetWindow<SentenceParserEditorWindow>("Sentence Parser");
	}

	private void OnEnable()
	{
		_serializedObject = new SerializedObject(this);
	}

	private void OnDisable()
	{
		_serializedObject.Dispose();
	}

	private void CreateGUI()
	{
		_editorUxml.CloneTree(rootVisualElement);

		_textField = rootVisualElement.Q<TextField>("input-sentence");

		var listView = rootVisualElement.Q<ListView>();
		listView.makeItem = _parsingCodeItemUxml.CloneTree;
		
		rootVisualElement.Q<Button>("button-parse").clicked += OnParseSentence;
		
		rootVisualElement.Bind(_serializedObject);
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