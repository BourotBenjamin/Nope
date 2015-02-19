using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerButtonCustomCacheScript : MonoBehaviour
{

    [SerializeField]
    Text _text;

    public Text Text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
        }
    }

    [SerializeField]
    RectTransform _mainRectTransform;

    public RectTransform MainRectTransform
    {
        get
        {
            return _mainRectTransform;
        }
        set
        {
            _mainRectTransform = value;
        }
    }

    [SerializeField]
    RectTransform _textRectTransform;

    public RectTransform TextRectTransform
    {
        get
        {
            return _textRectTransform;
        }
        set
        {
            _textRectTransform = value;
        }
    }

    [SerializeField]
    Button _buttonScript;

    public Button ButtonScript
    {
        get
        {
            return _buttonScript;
        }
        set
        {
            _buttonScript = value;
        }
    }
}
