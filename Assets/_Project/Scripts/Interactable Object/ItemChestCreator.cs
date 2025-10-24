using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemChestCreator : MonoBehaviour
{
    [SerializeField] private List<ItemStack> items = new();
    private ChestService _chestService;
    [Inject]
    public void Construct(ChestService chestService)
    {
        _chestService = chestService;
    }
    private void Start()
    {
        CreateItemsInChest();
    }
    private void CreateItemsInChest()
    {
        if (items == null || items.Count == 0) return;

        int added = 0;
        bool cutByLimit = false;

        foreach (var itemStack in items)
        {
            if (added >= 8) { cutByLimit = true; break; }
            if (itemStack == null || itemStack.Item == null) continue;
            if (itemStack.Count <= 0) continue;

            int max = itemStack.Item.MaxStackCount;
            int count = itemStack.Count > max ? max : itemStack.Count;
            if (count < itemStack.Count)
                Debug.LogWarning($"максимальное число {itemStack.Item} в стаке — {max}, создан стак с {max} элементами");

            _chestService.AddItem(new ItemStack(itemStack.Item, count));
            added++;
        }

        if (cutByLimit)
            Debug.LogWarning("добавлено слишком много предметов, учтены только первые 8 валидных");
    }
}
