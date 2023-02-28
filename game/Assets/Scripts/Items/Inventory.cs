using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<(Item item, int amount)> items = new();
    public ItemDatabase db;
    
    // Start is called before the first frame update
    void Start()
    {
        items.Add((db.GetItem(2), 1));
    }
    
    
    public void Loot(int itemId, int amount)
    {
        var item = db.GetItem(itemId);
        
        if (item is Equipment) // Add amount instances of the equipment (with different stats)
        {
            for (var i = 0; i < amount; i++)
                items.Add((db.GetItem(itemId), 1));
            return;
        }
        
        var index = items.FindIndex(i => i.item.Id == itemId);
        if (index == -1)
            items.Add((item, amount));
        else
            items[index] = (item, items[index].amount + amount);
    }
    
    public void Loot(int itemId)
        => Loot(itemId, 1);

    public void Loot(List<(int itemId, int amount)> loot)
        => loot.ForEach(i => Loot(i.itemId, i.amount));
}
