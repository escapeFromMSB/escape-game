using UnityEngine;

[CreateAssetMenu(menuName = "Game/Inventory Item", fileName = "NewInventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string id;             
    public string displayName;    
    public bool stackable = true;
    [Min(1)] public int maxStack = 1;
    public GameObject dropPrefab; 

    
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