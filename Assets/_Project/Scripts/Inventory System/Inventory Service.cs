public class InventoryService : ServiceBase
{
    public override void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            _cellData.Add(null);
        }
    }
}
