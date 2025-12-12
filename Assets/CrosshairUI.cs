using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Look")]
    public float length = 6f;       
    public float thickness = 2f;    
    public float gap = 4f;          
    public Color color = Color.white;
    [Range(0f, 1f)] public float alpha = 0.9f;

    [Header("Behavior")]
    public bool startVisible = true;

    Canvas canvas;
    Image up, down, left, right;

    void Awake()
    {
        
        var go = new GameObject("CrosshairCanvas");
        go.layer = gameObject.layer;
        canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        
        go.AddComponent<GraphicRaycaster>().enabled = false;
        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        
        up    = MakeArm("Up");
        down  = MakeArm("Down");
        left  = MakeArm("Left");
        right = MakeArm("Right");

        ApplyStyle();
        LayoutArms();

        SetVisible(startVisible);
        DontDestroyOnLoad(go); 
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
        
        up.rectTransform.sizeDelta   = new Vector2(thickness, length);
        down.rectTransform.sizeDelta = new Vector2(thickness, length);
        up.rectTransform.anchoredPosition   = new Vector2(0f,  gap + length * 0.5f);
        down.rectTransform.anchoredPosition = new Vector2(0f, -gap - length * 0.5f);

        
        left.rectTransform.sizeDelta  = new Vector2(length, thickness);
        right.rectTransform.sizeDelta = new Vector2(length, thickness);
        left.rectTransform.anchoredPosition  = new Vector2(-gap - length * 0.5f, 0f);
        right.rectTransform.anchoredPosition = new Vector2( gap + length * 0.5f,  0f);
    }

    
    public void SetVisible(bool visible)
    {
        if (canvas) canvas.enabled = visible;
    }

    
    void OnValidate()
    {
        if (up == null) return;
        ApplyStyle();
        LayoutArms();
    }
}
