using UnityEngine;

public class IntroPage : MonoBehaviour
{
    [Header("Text")]
    public string title = "Welcome to the Facility";
    
    [TextArea(4, 10)]
    public string body =
        "You wake up in an empty Mathematical Sciences Building.\n\n" +
        "- Explore the floors.\n" +
        "- Collect keycards, tools, and components.\n" +
        "- Use consoles and solve puzzles to progress.\n\n" +
        "Controls:\n" +
        "- WASD to move\n" +
        "- Space to jump\n" +
        "- R to pick up items\n" +
        "- I to open inventory\n" +
        "- E to use the Elevator\n" +
        "- TRY TO ESCAPE!";

    [Header("Input")]
    public KeyCode dismissKey = KeyCode.Space;

    private bool showing = true;
    
    private static bool hasShownOnce = false;


    
    private GUIStyle boxStyle;
    private GUIStyle headerStyle;
    private GUIStyle bodyStyle;

    void Start()
    {
        if (hasShownOnce)
        {
            showing = false;
            Time.timeScale = 1f;   
            return;
        }

        hasShownOnce = true;

        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!showing) return;

        
        if (Input.GetKeyDown(dismissKey) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CloseIntro();
        }
    }

    void CloseIntro()
    {
        showing = false;
        Time.timeScale = 1f; 
    }

    void OnDestroy()
    {
        if (showing)
        {
            Time.timeScale = 1f;
        }
    }

    void OnGUI()
    {
        if (!showing) return;

        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.8f));
            boxStyle.padding = new RectOffset(20, 20, 20, 20);

            headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.alignment = TextAnchor.UpperCenter;
            headerStyle.fontSize = 24;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            bodyStyle = new GUIStyle(GUI.skin.label);
            bodyStyle.alignment = TextAnchor.UpperLeft;
            bodyStyle.fontSize = 16;
            bodyStyle.wordWrap = true;
            bodyStyle.normal.textColor = Color.white;
        }

        float w = Mathf.Min(700f, Screen.width - 40f);
        float h = Mathf.Min(400f, Screen.height - 40f);
        float x = (Screen.width - w) * 0.5f;
        float y = (Screen.height - h) * 0.5f;

        Rect boxRect = new Rect(x, y, w, h);
        GUI.Box(boxRect, GUIContent.none, boxStyle);

        Rect headerRect = new Rect(boxRect.x + 10f, boxRect.y + 10f, boxRect.width - 20f, 40f);
        GUI.Label(headerRect, title, headerStyle);

        string fullBody = body + "\n\n[Press " + dismissKey + " to continue]";
        Rect bodyRect = new Rect(boxRect.x + 10f, boxRect.y + 60f, boxRect.width - 20f, boxRect.height - 70f);
        GUI.Label(bodyRect, fullBody, bodyStyle);
    }

    Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++) pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
