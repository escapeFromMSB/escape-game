using System;
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

    public event Action<CollectAllPuzzle> Completed;

    private bool isCompleted = false;
    public bool IsCompleted => isCompleted;

    private bool isActive = false;
    private float t = 0f;

    
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
            requiredObjects[n] = go; 
        }
    }

    public void Begin()
    {
        if (isCompleted) return;
        
        RebuildLookup();

        isActive = true;
        CenterMessage.Show($"{puzzleTitle}\nCollect all required items.", 2.0f);
    }

    void Update()
    {
        if (!isActive || isCompleted) return;

        t += Time.deltaTime;
        if (t < checkInterval) return;
        t = 0f;

        
        var keys = new List<string>(requiredObjects.Keys);
        foreach (var key in keys)
        {
            if (requiredObjects[key] == null)
            {
                requiredObjects[key] = GameObject.Find(key);
            }
        }

        
        bool allCollected = true;
        foreach (var kv in requiredObjects)
        {
            if (kv.Value != null) { allCollected = false; break; }
        }

        if (allCollected)
        {
            isActive = false;
            isCompleted = true;
            CenterMessage.Show("Puzzle Completed!\nYou collected everything.", 3.0f);
            Completed?.Invoke(this);
        }
    }
}
