using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public List<(Item item, int amount)> items = new();
        public ItemDatabase db;
        public GameObject canvas;

        // Start is called before the first frame update
        private void Start()
        {
            Loot(2, 1, 2, 0, 1, 1, 0, 0, 0, 0, 1, 2);
            canvas.SetActive(false);
            Show();
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

        public void Loot(params (int itemId, int amount)[] list)
        {
            foreach (var (itemId, amount) in list)
                Loot(itemId, amount);
        }
        
        public void Loot(params int[] list)
        {
            foreach (var itemId in list)
                Loot(itemId, 1);
        }

        public void Show()
        {
            canvas.SetActive(true);
            var content = canvas.transform.Find("Scroll View").Find("Viewport").Find("Content");
            foreach (Transform child in content)
                Destroy(child.gameObject);

            foreach (var (item, amount) in items)
            {
                // Instantiate Item Prefab as GameObject and set its parent to the content
                var instance = Instantiate(Resources.Load("Prefabs/Item"), content) as GameObject;
                if (instance == null)
                    throw new Exception("Item prefab not found!");
                // Set the item's name and amount
                instance.GetComponent<UIItem>().SetItem(item, amount);
            }
        }
    }
}
