using System.Collections.Generic;
using System.Linq;
using Zenject;

public class ChestService : ServiceBase
{
    private ChestGridController _chestGridController;

    [Inject]
    public void Construct(ChestGridController chestGridController)
    {
        _chestGridController = chestGridController;
    }
    public override void Initialize()
    {
        for (int i = 0; i < 8; i++)
        {
            _cellData.Add(null);
        }
        _chestGridController.Init();
    }
    public void AddItem(ItemStack stack) {
        for (int i = 0; i < _cellData.Count; i++)
        {
            if (_cellData[i] == null)
            {
                _cellData[i] = stack;
                OnChanged(i, stack);
                return;
            }
        }
    }
    public void SortItemsByName()
    {
        List<ItemStack> sortedItems = _cellData
            .Where(stack => stack != null && stack.Item != null)
            .OrderBy(stack => stack.Item.Name)
            .ToList();

        for (int i = 0; i < _cellData.Count; i++)
        {
            ItemStack newValue = i < sortedItems.Count ? sortedItems[i] : null;
            if (!ReferenceEquals(_cellData[i], newValue))
            {
                _cellData[i] = newValue;
                OnChanged(i, newValue);
            }
            else if (newValue != null)
            {
                OnChanged(i, newValue);
            }
        }
    }

}
