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

    [Command] private static void hello_world() => Debug.Log("Hello World!");
}
