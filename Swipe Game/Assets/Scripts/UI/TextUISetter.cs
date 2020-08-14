using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUISetter : MonoBehaviour
{
    [SerializeField] private bool IsVariableSetter = false;

    [SerializeField] private int countOfString = -1;

    private List<string> stringValues = new List<string>();

    private Text _text = null;

    private void Start()
    {
        if (!IsVariableSetter)
        {
            if (!_text) _text = GetComponent<Text>();
            if (!_text) Log.WriteLog("Can not get Text.", Log.LevelsOfLogs.ERROR, "TextUISetter");
        }
    }

    public void ClearStringArray()
    {
        stringValues.Clear();
    }

    public void SetVariableValue(string value)
    {
        stringValues.Add(value);
    }

    public void GetVariableValue(out string textStr, int id)
    {
        textStr = stringValues[id];
    }

    public void SetText(string text)
    {
        if (!IsVariableSetter)
        {
            if (!_text) _text = GetComponent<Text>();
            if (!_text) Log.WriteLog("Can not get Text.", Log.LevelsOfLogs.ERROR, "TextUISetter");
            _text.text = text;
        }
    }

    public int GetCountOfStringToSet() => countOfString;
}
