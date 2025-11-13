using UnityEngine;
using UnityEngine.UI;

public class CenterMessage : MonoBehaviour
{
    private static CenterMessage _instance;
    private Canvas canvas;
    private Text text;
    private float timer;

    public static void Show(string message, float seconds = 2f)
    {
        if (_instance == null)
        {
            var go = new GameObject("CenterMessageCanvas");
            _instance = go.AddComponent<CenterMessage>();
            _instance.Build();
        }

        _instance.text.text = message;
        _instance.timer = Mathf.Max(0.01f, seconds);
        _instance.text.enabled = true;
        _instance.canvas.enabled = true;
    }

    void Build()
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5000;

        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1600, 900);

        gameObject.AddComponent<GraphicRaycaster>();

        var textGO = new GameObject("Message");
        textGO.transform.SetParent(transform, false);
        text = textGO.AddComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;

        // ✅ Unity 6 / newer: use LegacyRuntime.ttf instead of Arial.ttf
        var builtinFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.font = builtinFont;
        text.fontSize = 28;
        text.raycastTarget = false;

        var rect = text.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.2f, 0.4f);
        rect.anchorMax = new Vector2(0.8f, 0.6f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        text.enabled = false;
        canvas.enabled = false;
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.unscaledDeltaTime;
            if (timer <= 0f)
            {
                text.enabled = false;
                canvas.enabled = false;
            }
        }
    }
}
