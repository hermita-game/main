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
        => PlayerMovement.GetInventory().Loot(itemId, amount);

    [Command]
    private static void use_consumable(int itemId)
    {
        var player = PlayerMovement.GetPlayer();
        var inventory = player.GetComponent<Inventory>();
        var item = inventory.db.GetItem(itemId);
        if (item is Consumable potion)
            player.GetComponent<Fighting.Player>().UseConsumable(potion);
    }
    
    [Command]
    private static void set_stat(string stat, float value)
    {
        var player = PlayerMovement.GetPlayer();
        player.GetComponent<Fighting.Player>().SetStat(stat, value);
    }
}
