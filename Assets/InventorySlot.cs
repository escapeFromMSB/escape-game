using UnityEngine;

[System.Serializable]
public struct InventorySlot
{
    public InventoryItem item;
    public int count;

    public bool IsEmpty => item == null || string.IsNullOrEmpty(item.id) || count <= 0;

    public bool CanStack(InventoryItem other)
    {
        return !IsEmpty
               && item.id == other.id
               && item.stackable
               && count < item.maxStack;
    }

    // Returns leftover that could not be added
    public int Add(InventoryItem other, int amount)
    {
        if (other == null || amount <= 0) return amount;

        if (IsEmpty)
        {
            item = other;
            int cap = other.stackable ? Mathf.Max(1, other.maxStack) : 1;
            int toAdd = Mathf.Clamp(amount, 0, cap);
            count = toAdd;
            return amount - toAdd;
        }

        if (!CanStack(other)) return amount;

        int spaceLeft = Mathf.Max(0, item.maxStack - count);
        int toStack = Mathf.Clamp(amount, 0, spaceLeft);
        count += toStack;
        return amount - toStack;
    }

    // Returns leftover that could not be removed
    public int Remove(int amount)
    {
        if (IsEmpty || amount <= 0) return amount;

        int take = Mathf.Min(count, amount);
        count -= take;

        if (count <= 0)
        {
            item = null;
            count = 0;
        }
        return amount - take;
    }
}