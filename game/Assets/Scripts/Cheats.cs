using Items;
using SmartConsole;

public class Cheats : CommandBehaviour
{
    protected override void Start()
    {
        base.Start();
        DontDestroyOnLoad(this);
    }
    
    [Command]
    private static void give_item(int itemId, int amount)
        => PlayerMovement.GetPlayer().GetComponent<Inventory>().Loot(itemId, amount);
}
