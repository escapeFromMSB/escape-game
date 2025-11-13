using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Look")]
    public float length = 6f;       // length of each arm (px)
    public float thickness = 2f;    // line thickness (px)
    public float gap = 4f;          // empty space at center (px)
    public Color color = Color.white;
    [Range(0f, 1f)] public float alpha = 0.9f;

    [Header("Behavior")]
    public bool startVisible = true;

    Canvas canvas;
    Image up, down, left, right;

    void Awake()
    {
        // Canvas (Screen Space - Overlay)
        var go = new GameObject("CrosshairCanvas");
        go.layer = gameObject.layer;
        canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Don’t block clicks
        go.AddComponent<GraphicRaycaster>().enabled = false;
        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Build 4 arms (Images)
        up    = MakeArm("Up");
        down  = MakeArm("Down");
        left  = MakeArm("Left");
        right = MakeArm("Right");

        ApplyStyle();
        LayoutArms();

        SetVisible(startVisible);
        DontDestroyOnLoad(go); // persists across scene loads if you use puzzle scenes
    }

    Image MakeArm(string name)
    {
        var arm = new GameObject(name, typeof(RectTransform), typeof(Image));
        arm.transform.SetParent(canvas.transform, false);

        var rt = arm.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        var img = arm.GetComponent<Image>();
        img.raycastTarget = false;
        return img;
    }

    void ApplyStyle()
    {
        Color c = color;
        c.a = alpha;
        up.color = down.color = left.color = right.color = c;
    }

    void LayoutArms()
    {
        // Vertical (up/down)
        up.rectTransform.sizeDelta   = new Vector2(thickness, length);
        down.rectTransform.sizeDelta = new Vector2(thickness, length);
        up.rectTransform.anchoredPosition   = new Vector2(0f,  gap + length * 0.5f);
        down.rectTransform.anchoredPosition = new Vector2(0f, -gap - length * 0.5f);

        // Horizontal (left/right)
        left.rectTransform.sizeDelta  = new Vector2(length, thickness);
        right.rectTransform.sizeDelta = new Vector2(length, thickness);
        left.rectTransform.anchoredPosition  = new Vector2(-gap - length * 0.5f, 0f);
        right.rectTransform.anchoredPosition = new Vector2( gap + length * 0.5f,  0f);
    }

    // Public API
    public void SetVisible(bool visible)
    {
        if (canvas) canvas.enabled = visible;
    }

    // If you tweak values in the inspector at runtime
    void OnValidate()
    {
        if (up == null) return;
        ApplyStyle();
        LayoutArms();
    }
}
