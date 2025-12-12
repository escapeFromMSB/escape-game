using UnityEngine;
using UnityEngine.UI;

public class WireTerminal : MonoBehaviour
{
    public string wireId;     
    public bool isLeft;
    public bool isConnected;

    [HideInInspector] public WirePuzzleController controller;

    private Button button;
    private Image image;

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        if (button != null)
            button.onClick.AddListener(OnClicked);
    }

    public void Setup(WirePuzzleController ctrl, string id, Color color, bool left)
    {
        controller = ctrl;
        wireId = id;
        isLeft = left;
        isConnected = false;

        if (image == null) image = GetComponent<Image>();
        if (image != null) image.color = color;
    }

    private void OnClicked()
    {
        if (isConnected) return;
        if (controller != null)
            controller.OnTerminalClicked(this);
    }

    public RectTransform Rect => transform as RectTransform;
    public Color TerminalColor => (GetComponent<Image>() != null) ? GetComponent<Image>().color : Color.white;
}