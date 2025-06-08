using Zenject;

public class InventorySkinsWindow : WindowBase
{
    [Inject] private InventorySkinsService _inventorySkinsService;

    public override void Open()
    {
        base.Open();
        _inventorySkinsService.UpdateSkinsData();
        _inventorySkinsService.UpdateView();
    }
}