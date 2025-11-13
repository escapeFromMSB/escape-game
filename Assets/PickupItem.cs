using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    [Header("Item Definition")]
    public string itemId = "key.red";
    public string displayName = "Red Key";
    public bool stackable = true;
    [Min(1)] public int maxStack = 1;

    [Header("Amount in this pickup")]
    [Min(1)] public int amount = 1;

    [Tooltip("Optional: assign a mesh/renderer host if the pickup object is a trigger child.")]
    public Transform visualRoot;

    InventoryItem _runtimeItem;

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Awake()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
        else
        {
            var c = gameObject.AddComponent<BoxCollider>();
            c.isTrigger = true;
        }
    }

    public InventoryItem GetItem()
    {
        if (_runtimeItem == null)
            _runtimeItem = InventoryItem.Make(itemId, displayName, stackable, Mathf.Max(1, maxStack));
        return _runtimeItem;
    }

    public void OnPickedSome(int taken, int leftover)
    {
        amount = leftover;
        if (amount <= 0) Destroy(gameObject);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, $"{displayName} x{amount}");
    }
#endif
}