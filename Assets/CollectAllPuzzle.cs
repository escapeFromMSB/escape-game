using System.Collections.Generic;
using UnityEngine;

public class CollectAllPuzzle : MonoBehaviour
{
    [Tooltip("Names of the pickup GameObjects you want the player to collect.")]
    public List<string> requiredObjectNames = new List<string>();

    [Tooltip("Optional nice name to show in messages.")]
    public string puzzleTitle = "Collect Everything";

    [Tooltip("How often (seconds) to check once the puzzle is active.")]
    public float checkInterval = 0.25f;

    private bool isActive = false;
    private float t = 0f;

    // cache of scene objects (looked up by name)
    private readonly Dictionary<string, GameObject> requiredObjects = new();

    public void Initialize(IEnumerable<string> names)
    {
        requiredObjectNames = new List<string>(names);
        RebuildLookup();
    }

    void Awake()
    {
        if (requiredObjectNames.Count > 0 && requiredObjects.Count == 0)
            RebuildLookup();
    }

    private void RebuildLookup()
    {
        requiredObjects.Clear();
        foreach (var n in requiredObjectNames)
        {
            var go = GameObject.Find(n);
            requiredObjects[n] = go; // may be null if not found yet; we’ll handle it
        }
    }

    public void Begin()
    {
        // Safety update (in case anything respawned/renamed)
        RebuildLookup();

        isActive = true;
        CenterMessage.Show($"{puzzleTitle}\nCollect all required items.", 2.0f);
    }

    void Update()
    {
        if (!isActive) return;

        t += Time.deltaTime;
        if (t < checkInterval) return;
        t = 0f;

        // Refresh any nulls once (in case scene order delayed creation)
        foreach (var key in new List<string>(requiredObjectNames))
        {
            if (requiredObjects[key] == null)
            {
                // Try to find it by name again (it might have existed before begin)
                requiredObjects[key] = GameObject.Find(key);
            }
        }

        // All collected == none of them exist anymore in the scene
        bool allCollected = true;
        foreach (var kv in requiredObjects)
        {
            if (kv.Value != null) { allCollected = false; break; }
        }

        if (allCollected)
        {
            isActive = false;
            CenterMessage.Show("Puzzle Completed!\nYou collected everything.", 3.0f);
        }
    }
}
