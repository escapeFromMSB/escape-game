using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Window")]
    public bool visible = false;
    public Rect windowRect = new Rect(20, 20, 420, 320);
    public string title = "Inventory";

    [Header("Grid")]
    public int columns = 4;
    public float cellSize = 80f;
    public float cellPad = 6f;

    private GUIStyle titleStyle;
    private GUIStyle itemStyle;
    private Vector2 scroll;

    private PlayerInventory inv;

    void Start()
    {
        inv = FindObjectOfType<PlayerInventory>();
        if (!inv)
        {
            var go = new GameObject("PlayerInventory");
            inv = go.AddComponent<PlayerInventory>();
        }
    }

    public void Show()  { visible = true; }
    public void Hide()  { visible = false; }
    public void Toggle(){ visible = !visible; }

    void OnGUI()
    {
        if (!visible) return;

        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                fontStyle = FontStyle.Bold
            };
        }
        if (itemStyle == null)
        {
            itemStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                wordWrap = true
            };
        }

        windowRect = GUI.Window(GetInstanceID(), windowRect, DrawWindow, title);
    }

    void DrawWindow(int id)
    {
        if (!inv) { GUI.DragWindow(); return; }

        GUILayout.Space(6);
        Rect area = new Rect(10, 28, windowRect.width - 20, windowRect.height - 38);
        GUILayout.BeginArea(area);
        scroll = GUILayout.BeginScrollView(scroll, false, true);

        int rows = Mathf.CeilToInt((float)inv.capacity / Mathf.Max(1, columns));
        float totalW = columns * cellSize + (columns - 1) * cellPad;

        for (int r = 0; r < rows; r++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(Mathf.Max(0, (area.width - totalW) * 0.5f)); 

            for (int c = 0; c < columns; c++)
            {
                int idx = r * columns + c;
                if (idx >= inv.slots.Count) break;

                var slot = inv.slots[idx];

                GUILayout.BeginVertical(GUILayout.Width(cellSize));
                Rect cell = GUILayoutUtility.GetRect(cellSize, cellSize);

                GUI.Box(cell, GUIContent.none);

                if (!slot.IsEmpty)
                {
                    string label = $"{slot.item.displayName}\n×{slot.item.amount}";
                    GUI.Label(cell, label, itemStyle);
                }

                GUILayout.EndVertical();

                if (c < columns - 1) GUILayout.Space(cellPad);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(cellPad);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        GUI.DragWindow(new Rect(0, 0, windowRect.width, 24f));
    }
}
