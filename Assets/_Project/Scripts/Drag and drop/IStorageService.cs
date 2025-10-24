public interface IStorageService
{
    ItemStack Get(int index);
    bool TryInsert(int index, ItemStack stack, out ItemStack rest, ItemAmountEnum amount);        
    bool TryExtract(int index, out ItemStack extracted,ItemAmountEnum amount);
    bool CanAccept(int index, ItemStack stack);        
    bool RemoveItem(int index);

    event System.Action<int, ItemStack> Changed;       
}