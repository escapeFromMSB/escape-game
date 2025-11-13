using UnityEngine;

[CreateAssetMenu(menuName = "Game/Inventory Item", fileName = "NewInventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string id;             // unique key
    public string displayName;    // player-facing
    public bool stackable = true;
    [Min(1)] public int maxStack = 1;
    public GameObject dropPrefab; // optional world prefab to drop

    // Handy factory for runtime-only items (no asset needed)
    public static InventoryItem Make(string id, string name, bool stackable, int maxStack, GameObject dropPrefab = null)
    {
        var x = CreateInstance<InventoryItem>();
        x.id = id;
        x.displayName = name;
        x.stackable = stackable;
        x.maxStack = Mathf.Max(1, maxStack);
        x.dropPrefab = dropPrefab;
        return x;
    }
}