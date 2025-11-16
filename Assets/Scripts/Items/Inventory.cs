using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<BaseItem, int> items = new Dictionary<BaseItem, int>();

    public static Inventory Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public int GetAmount(BaseItem item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }

    public void Add(BaseItem item, int amount)
    {
        if (!items.ContainsKey(item))
            items[item] = 0;

        items[item] += amount;
    }

    public bool Remove(BaseItem item, int amount)
    {
        if (GetAmount(item) < amount) return false;

        items[item] -= amount;
        return true;
    }

    public bool HasEnough(BaseItem item, int amount)
    {
        return GetAmount(item) >= amount;
    }
}
