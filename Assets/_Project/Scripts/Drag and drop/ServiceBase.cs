using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class ServiceBase : IStorageService, IInitializable
{
    protected List<ItemStack> _cellData = new();

    public event Action<int, ItemStack> Changed;

    public virtual bool CanAccept(int index, ItemStack stack)
    {
        ItemStack currentStack = _cellData[index];
        
        if (currentStack == null)
            return true;

        if (currentStack.Item == stack.Item)
        {
            int maxCount = currentStack.Item.MaxStackCount;
            int currentCount = currentStack.Count;
            return currentCount < maxCount;
        }

        return false;
    }

    protected void OnChanged(int index, ItemStack value) {
        Changed?.Invoke(index,value);
    }
    public virtual ItemStack Get(int index)
    {
        return _cellData[index];
    }

    public virtual bool TryInsert(int index, ItemStack stack, out ItemStack rest, ItemAmountEnum amount)
    {
        rest = null;
        ItemStack currentStack = _cellData[index];
        if (amount == ItemAmountEnum.All)
        {
            if (currentStack == null)
            {
                _cellData[index] = stack;
                Changed?.Invoke(index, stack);
                return true;

            }
            if (currentStack.Item != null)
            {

                int maxCount = currentStack.Item.MaxStackCount;
                int currentCount = currentStack.Count;
                int restCount = maxCount - currentCount;

                int toAdd = stack.Count > restCount ? restCount : stack.Count;

                _cellData[index] = new(currentStack.Item, currentCount + toAdd);

                rest = stack.Count > restCount ? new(stack.Item, stack.Count - restCount) : null;
                Changed?.Invoke(index, _cellData[index]);
                return true;
            }
        }
        if (amount == ItemAmountEnum.One)
        {
            if (_cellData[index] == null)
            {

                _cellData[index] = new(stack.Item, 1);
                Changed?.Invoke(index, _cellData[index]);

                if (stack.Count == 1) return true;
                rest = new(stack.Item, stack.Count - 1);
                return true;
            }
            else
            {
                int count = _cellData[index].Count;
                _cellData[index] = new(stack.Item, count + 1);
                Changed?.Invoke(index, _cellData[index]);

                if (stack.Count == 1) return true;

                rest = new(stack.Item, stack.Count - 1);
                return true;
            }
        }
        return false;
    }

    public virtual bool TryExtract(int index, out ItemStack extracted, ItemAmountEnum amount)
    {
        ItemStack currentStack = _cellData[index];
        extracted = null;
        if (currentStack == null) return false;

        if (currentStack.Item != null)
        {
            if (amount == ItemAmountEnum.All || currentStack.Count == 1)
            {
                extracted = currentStack;
                _cellData[index] = null;
                Changed?.Invoke(index, null);
                return true;
            }
            else if (amount == ItemAmountEnum.Half)
            {
                int halfCount = currentStack.Count / 2;
                extracted = new(currentStack.Item, halfCount);
                _cellData[index] = new(currentStack.Item, currentStack.Count - halfCount);

                Changed?.Invoke(index, _cellData[index]);
                return true;
            }
            else if (amount == ItemAmountEnum.One)
            {
                _cellData[index] = new(currentStack.Item, currentStack.Count - 1);
                extracted = new(currentStack.Item, 1);
                Changed?.Invoke(index, _cellData[index]);
                return true;
            }
        }
        return false;
    }

    public virtual bool RemoveItem(int index)
    {
        ItemStack currentStack = _cellData[index];
        if (currentStack == null) return false;

        if (currentStack.Count > 1)
        {
            _cellData[index] = new(currentStack.Item, currentStack.Count - 1);
            Changed?.Invoke(index, _cellData[index]);
        }
        else
        {
            _cellData[index] = null;
            Changed?.Invoke(index, null);
        }
        
        return true;
    }

    public abstract void Initialize();
}
