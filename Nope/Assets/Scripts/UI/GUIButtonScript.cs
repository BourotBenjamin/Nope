using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIButtonScript : MonoBehaviour
{

    [SerializeField]
    Text _text;
    [SerializeField]
    Image _image;

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
    public Image Image
    {
        get
        {
            return _image;
        }
        set
        {
            _image = value;
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