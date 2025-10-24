using System;
using UnityEngine;


[Serializable]
public class ItemStack
{
   [SerializeField]  private ItemData _itemData;
   [SerializeField]  private int _count;

    public ItemStack(ItemData data, int count) {

        _itemData = data;
        _count = count;
    }
    public int Count => _count;
    public ItemData Item => _itemData;

    public ItemStack AddCount(int delta) {

        int newCount = Math.Min(_itemData.MaxStackCount,_count+delta);
        return new ItemStack(_itemData, newCount);
    }

    public ItemStack AddCountForBuilding(int delta)
    {
        return new ItemStack(_itemData, _count + delta);
    }
    public ItemStack RemoveCount(int delta) {
        int newCount = Math.Max(0,_count-delta);
        return new ItemStack(_itemData, newCount);

    }
    public bool CanBeAdded(int count) {
        if (_count + 1 <= _itemData.MaxStackCount) return true;
        else return false;
    }
}
