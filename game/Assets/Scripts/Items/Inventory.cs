using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        private readonly List<(Item item, int amount)> _items = new();
        private string filter = "all";

        public List<(Item item, int amount)> Items =>
            filter switch
            {
                "all" => _items,
                "consumable" => _items.FindAll(i => i.item is Consumable),
                "equipment" => _items.FindAll(i => i.item is Equipment),
                "resource" => _items.FindAll(i => i.item is not Consumable and not Equipment),
                _ => throw new ArgumentException("Invalid filter")
            };

        public ItemDatabase db;
        public GameObject canvas;

        // Start is called before the first frame update
        private void Start()
        {
            Loot(5, 2, 1, 2, 0, 4, 1, 1, 0, 0, 3, 3, 3, 3, 1, 1, 2, 3, 1, 2, 1, 2, 1, 1, 0, 0, 0, 0, 1, 2, 2, 1, 2,
                1, 0, 0, 0, 0, 1,2,1,2,1,1,0,0,0,1,0,0,1,0,1,2,2,0,5,4,5,4,5,4,4,4,5);
            canvas.SetActive(false);
            canvas.transform.Find("All").GetComponent<Button>().onClick.AddListener(() => Filter("all"));
            canvas.transform.Find("Consumable").GetComponent<Button>().onClick.AddListener(() => Filter("consumable"));
            canvas.transform.Find("Equipment").GetComponent<Button>().onClick.AddListener(() => Filter("equipment"));
            canvas.transform.Find("Resource").GetComponent<Button>().onClick.AddListener(() => Filter("resource"));
            Show();
        }
        
        private void Filter(string filt)
        {
            filter = filt;
            UpdateDisplay();
        }

        public void Loot(int itemId, int amount)
        {
            var item = db.GetItem(itemId);
        
            if (item is Equipment) // Add amount instances of the equipment (with different stats)
            {
                for (var i = 0; i < amount; i++)
                    _items.Add((db.GetItem(itemId), 1));
                return;
            }
        
            var index = _items.FindIndex(i => i.item.Id == itemId);
            if (index == -1)
                _items.Add((item, amount));
            else
                _items[index] = (item, _items[index].amount + amount);
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
            UpdateDisplay();
        }
        
        public void Hide()
            => canvas.SetActive(false);
            
        private void UpdateDisplay()
        {
            var nbRows = (Items.Count - 1) / 6 + 1;
            var content = canvas.transform.Find("Scroll View").Find("Viewport").Find("Content");
            // Set the content's height to the number of rows
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 58*nbRows-3);
            foreach (Transform child in content)
                Destroy(child.gameObject);

            foreach (var (item, amount) in Items)
            {
                // Instantiate Item Prefab as GameObject and set its parent to the content
                var instance = Instantiate(Resources.Load("Prefabs/Item"), content) as GameObject;
                if (instance == null)
                    throw new Exception("Item prefab not found!");
                // Set the item's name and amount
                instance.GetComponent<UIItem>().SetItem(item, amount);
            }

            // change scroll steps in scrollbar
            var scrollBar = canvas.transform.Find("Scroll View").Find("Scrollbar Vertical").GetComponent<Scrollbar>();
            scrollBar.numberOfSteps = Math.Max(1, nbRows - 4);
        }
    }
}
