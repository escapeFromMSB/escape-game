using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WirePuzzleController : MonoBehaviour
{
    [Header("Scene refs")]
    public RectTransform rootPanel;      
    public Transform linesParent;        

    [Header("Terminals")]
    public List<WireTerminal> leftTerminals = new List<WireTerminal>();
    public List<WireTerminal> rightTerminals = new List<WireTerminal>();

    [Header("Config")]
    public string returnSceneName = "MainGame";
    public float lineThickness = 6f;

    private WireTerminal selectedLeft;

    
    private readonly (string id, Color color)[] wires =
    {
        ("red",    new Color(0.85f, 0.2f, 0.2f)),
        ("blue",   new Color(0.2f, 0.35f, 0.9f)),
        ("green",  new Color(0.2f, 0.8f, 0.3f)),
        ("yellow", new Color(0.9f, 0.85f, 0.25f)),
    };

    void Start()
    {
        if (rootPanel == null)
            rootPanel = GetComponent<RectTransform>();

        
        for (int i = 0; i < leftTerminals.Count && i < wires.Length; i++)
        {
            leftTerminals[i].Setup(this, wires[i].id, wires[i].color, true);
        }

        
        var shuffled = new List<(string id, Color color)>(wires);
        Shuffle(shuffled);

        for (int i = 0; i < rightTerminals.Count && i < shuffled.Count; i++)
        {
            rightTerminals[i].Setup(this, shuffled[i].id, shuffled[i].color, false);
        }
    }

    public void OnTerminalClicked(WireTerminal terminal)
    {
        if (terminal.isLeft)
        {
            selectedLeft = terminal;
            return;
        }

        
        if (selectedLeft == null) return;

        
        if (terminal.wireId == selectedLeft.wireId)
        {
            
            terminal.isConnected = true;
            selectedLeft.isConnected = true;

            DrawLine(selectedLeft, terminal, selectedLeft.TerminalColor);

            selectedLeft = null;

            if (AllConnected())
                CompletePuzzle();
        }
        else
        {
            
            selectedLeft = null;
        }
    }

    private bool AllConnected()
    {
        foreach (var t in leftTerminals)
            if (!t.isConnected) return false;
        return true;
    }

    private void CompletePuzzle()
    {
        GameFlags.FirstRoomWiresSolved = true;

        
        SceneManager.LoadScene(returnSceneName);
    }

    

    private void DrawLine(WireTerminal a, WireTerminal b, Color color)
    {
        if (linesParent == null) linesParent = rootPanel;

        GameObject lineGO = new GameObject($"WireLine_{a.wireId}", typeof(RectTransform), typeof(Image));
        lineGO.transform.SetParent(linesParent, false);

        Image img = lineGO.GetComponent<Image>();
        img.color = color;

        RectTransform rt = lineGO.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0.5f);

        Vector2 p1 = GetLocalPoint(a.Rect);
        Vector2 p2 = GetLocalPoint(b.Rect);

        Vector2 mid = (p1 + p2) * 0.5f;
        Vector2 dir = (p2 - p1);
        float length = dir.magnitude;

        rt.anchoredPosition = mid;
        rt.sizeDelta = new Vector2(length, lineThickness);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private Vector2 GetLocalPoint(RectTransform target)
    {
        Vector3 world = target.TransformPoint(target.rect.center);
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, world);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootPanel, screen, null, out Vector2 local);

        return local;
    }

    

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
