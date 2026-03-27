using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lingtianlu
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maxSlots = 24;

        private readonly Dictionary<string, int> items = new();

        public event Action<string, int> ItemChanged;

        public bool AddItem(string itemId, int amount)
        {
            if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            {
                return false;
            }

            if (!items.ContainsKey(itemId) && items.Count >= maxSlots)
            {
                return false;
            }

            if (!items.ContainsKey(itemId))
            {
                items[itemId] = 0;
            }

            items[itemId] += amount;
            ItemChanged?.Invoke(itemId, items[itemId]);
            return true;
        }

        public bool RemoveItem(string itemId, int amount)
        {
            if (!items.TryGetValue(itemId, out int current) || amount <= 0 || current < amount)
            {
                return false;
            }

            int next = current - amount;
            if (next <= 0)
            {
                items.Remove(itemId);
                ItemChanged?.Invoke(itemId, 0);
                return true;
            }

            items[itemId] = next;
            ItemChanged?.Invoke(itemId, next);
            return true;
        }

        public int GetCount(string itemId)
        {
            return items.TryGetValue(itemId, out int count) ? count : 0;
        }
    }
}
