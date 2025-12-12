using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
    [System.Serializable]
    public class InventoryItem
    {
        public string id;
        public string displayName;
        public int amount;
        public int maxStack;

        public static InventoryItem Make(string id, string label, int amount, int maxStack = 99)
        {
            return new InventoryItem
            {
                id = id,
                displayName = label,
                amount = amount,
                maxStack = maxStack
            };
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem item;

        public bool IsEmpty => item == null || item.amount <= 0;

        public bool CanStack(string id, int addAmount)
        {
            if (IsEmpty || item.id != id) return false;
            return item.amount < item.maxStack && addAmount > 0;
        }

        
        public int AddToStack(string id, string label, int addAmount, int maxStack = 99)
        {
            if (addAmount <= 0) return 0;

            if (IsEmpty)
            {
                item = InventoryItem.Make(id, label, 0, maxStack);
            }

            if (item.id != id) return 0;

            int space = Mathf.Max(0, item.maxStack - item.amount);
            int putIn = Mathf.Clamp(addAmount, 0, space);
            item.amount += putIn;
            return putIn;
        }

        public void Clear() { item = null; }
    }
    

    [Header("Inventory Config")]
    public int capacity = 16;
    public int defaultMaxStack = 99;

    [Header("State (debug view)")]
    public List<InventorySlot> slots = new List<InventorySlot>();

    void Awake()
    {
        if (slots == null) slots = new List<InventorySlot>();

        
        if (slots.Count != capacity)
        {
            slots.Clear();
            for (int i = 0; i < capacity; i++)
                slots.Add(new InventorySlot());
        }
    }

    
    
    
    
    public bool TryAddItem(string id, string label, int amount)
    {
        if (amount <= 0) return true;
        int remaining = amount;

        
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var s = slots[i];
            if (!s.IsEmpty && s.item.id == id && s.item.amount < s.item.maxStack)
            {
                int put = s.AddToStack(id, label, remaining, s.item.maxStack);
                remaining -= put;
            }
        }

        
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

    public bool HasItem(string id, int minAmount = 1)
    {
        if (minAmount <= 0) return true;

        int total = 0;

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item.id == id)
            {
                total += slot.item.amount;
                if (total >= minAmount)
                    return true;
            }
        }

        return false;
    }


    public bool RemoveItem(string id, int amount = 1)
    {
        int remaining = amount;

        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var s = slots[i];
            if (s != null && !s.IsEmpty && s.item != null && s.item.id == id)
            {
                int take = Mathf.Min(s.item.amount, remaining);
                s.item.amount -= take;
                remaining -= take;

                if (s.item.amount <= 0)
                    s.Clear();
            }
        }

        return remaining == 0;
    }
}
