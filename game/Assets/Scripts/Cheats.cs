using Items;
using SmartConsole;
using UnityEngine;

public class Cheats : CommandBehaviour
{
    protected override void Start()
    {
        base.Start();
        DontDestroyOnLoad(this);
    }
    
    [Command]
    private static void give_item(int itemId, int amount)
        => Tools.GetInventory().Loot(itemId, amount);

    [Command]
    private static void use_consumable(int itemId)
    {
        var inventory = Tools.GetInventory();
        var item = inventory.db.GetItem(itemId);
        if (item is Consumable potion)
            Tools.GetPlayer().UseConsumable(potion);
    }
    
    [Command]
    private static void set_stat(string stat, float value)
        => Tools.GetPlayer().SetStat(stat, value);
}
