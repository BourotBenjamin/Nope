using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    string[] classes = { "Warrior", "Healer", "Wizard" };
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    RectTransform serverCanevas;
    private GUIButtonScript[,] guiBtns;

    void Awake()
    {
        guiBtns = new GUIButtonScript[3, classes.Length];
        GameOptionsScript.warriors[0] = null;
        GameOptionsScript.warriors[1] = null;
        GameOptionsScript.warriors[2] = null;
        var btn = ((GameObject)Instantiate(buttonPrefab)).GetComponent<GUIButtonScript>();
        btn.MainRectTransform.SetParent(serverCanevas);
        btn.MainRectTransform.localPosition = Vector3.zero;
        btn.MainRectTransform.anchorMin = new Vector3(0f, 0f);
        btn.MainRectTransform.anchorMax = new Vector3(1f, 0.5f);
        btn.MainRectTransform.offsetMin = new Vector3(0f, 0f);
        btn.MainRectTransform.offsetMax = new Vector3(0f, 0f);
        btn.MainRectTransform.localScale = Vector3.one;
        btn.Text.text = "Server";
        btn.ButtonScript.onClick.AddListener(() =>
        {
            GameOptionsScript.isServer = true;
            Application.LoadLevel("mainScene");
        });
        var btn2 = ((GameObject)Instantiate(buttonPrefab)).GetComponent<GUIButtonScript>();
        btn2.MainRectTransform.SetParent(serverCanevas);
        btn2.MainRectTransform.localPosition = Vector3.zero;
        btn2.MainRectTransform.anchorMin = new Vector3(0f, 0.5f);
        btn2.MainRectTransform.anchorMax = new Vector3(1f, 1f);
        btn2.MainRectTransform.offsetMin = new Vector3(0f, 0f);
        btn2.MainRectTransform.offsetMax = new Vector3(0f, 0f);
        btn2.MainRectTransform.localScale = Vector3.one;
        btn2.Text.text = "Client";
        btn2.ButtonScript.onClick.AddListener(() =>
        {
            GameOptionsScript.isServer = false;
            Destroy(btn.gameObject);
            Destroy(btn2.gameObject);
            CreateClassButtons();
        });
    }

    void CreateClassButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < classes.Length; j++)
            {
                var btn = ((GameObject)Instantiate(buttonPrefab)).GetComponent<GUIButtonScript>();
                btn.MainRectTransform.SetParent(serverCanevas);
                btn.MainRectTransform.localPosition = Vector3.zero;
                btn.MainRectTransform.anchorMin = new Vector3((1f / classes.Length) * (j), 0.33f * (i));
                btn.MainRectTransform.anchorMax = new Vector3((1f / classes.Length)*(j+1), 0.33f*(i+1));
                btn.MainRectTransform.offsetMin = new Vector3(0f, 0f);
                btn.MainRectTransform.offsetMax = new Vector3(0f, 0f);
                btn.MainRectTransform.localScale = Vector3.one;
                btn.Image.sprite = sprites[j];
                btn.Image.type = UnityEngine.UI.Image.Type.Filled;
                btn.Image.preserveAspect = true;
                int a = i, b = j;
                guiBtns[i, j] = btn;
                btn.ButtonScript.onClick.AddListener(() =>
                {
                   for (int k = 0; k < classes.Length; k++)
                   {
                       guiBtns[a, k].Image.color = new Color(1.0f, 1.0f, 1.0f);
                   }
                   btn.Image.color = new Color(0.9f,0.9f,0.9f);
                   GameOptionsScript.warriors[a] = classes[b];
                   if(GameOptionsScript.warriors[0] != null && GameOptionsScript.warriors[1] != null && GameOptionsScript.warriors[2] !=null)
                    {
                        Application.LoadLevel("mainScene");
                    }
                });
            }
        }
    }

}
