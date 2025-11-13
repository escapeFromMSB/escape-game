using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // ========= Nested data types (no more global-name collisions) =========
    [System.Serializable]
    public class InventoryItem
    {
        public string id;
        public string displayName;
        public int amount;
        public int maxStack;

        public static InventoryItem Make(string id, string label, int amount, int maxStack = 99)
        {
            return new InventoryItem { id = id, displayName = label, amount = amount, maxStack = maxStack };
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem item; // null or amount==0 means empty
        public bool IsEmpty => item == null || item.amount <= 0;

        public bool CanStack(string id, int addAmount)
        {
            if (IsEmpty || item.id != id) return false;
            return item.amount < item.maxStack && addAmount > 0;
        }

        /// <summary>Returns how many were actually added to this slot (0..addAmount).</summary>
        public int AddToStack(string id, string label, int addAmount, int maxStack = 99)
        {
            if (addAmount <= 0) return 0;

            if (IsEmpty)
            {
                int put = Mathf.Min(addAmount, maxStack);
                item = InventoryItem.Make(id, label, put, maxStack);
                return put;
            }

            if (item.id != id) return 0;

            int space = item.maxStack - item.amount;
            int putIn = Mathf.Clamp(addAmount, 0, space);
            item.amount += putIn;
            return putIn;
        }

        public void Clear() { item = null; }
    }
    // =====================================================================

    [Header("Inventory Config")]
    public int capacity = 16;
    public int defaultMaxStack = 99;

    [Header("State (debug view)")]
    public List<InventorySlot> slots = new List<InventorySlot>();

    void Awake()
    {
        if (slots == null) slots = new List<InventorySlot>();
        if (slots.Count < capacity)
        {
            for (int i = slots.Count; i < capacity; i++)
                slots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// Add items into the inventory. Returns true if the full amount was added.
    /// Tries to stack first, then fills empty slots.
    /// </summary>
    public bool TryAddItem(string id, string label, int amount)
    {
        if (amount <= 0) return true;
        int remaining = amount;

        // 1) fill existing stacks
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var s = slots[i];
            if (!s.IsEmpty && s.item.id == id && s.item.amount < s.item.maxStack)
            {
                int put = s.AddToStack(id, label, remaining, s.item.maxStack);
                remaining -= put;
            }
        }

        // 2) fill empty slots
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var s = slots[i];
            if (s.IsEmpty)
            {
                int put = s.AddToStack(id, label, remaining, defaultMaxStack);
                remaining -= put;
            }
        }

        return remaining == 0;
    }
}
