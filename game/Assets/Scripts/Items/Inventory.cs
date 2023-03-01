using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        private readonly List<(Item item, int amount)> _items = new();
        private string _filter = "all";
        private string _sort = "name";
        private bool _ascending = true;
        private bool _showing;

        private const KeyCode Key = KeyCode.I;

        private void Update()
        {
            if (!Input.GetKeyDown(Key)) return;
            if (_showing) Hide();
            else Show();
        }

        public List<(Item item, int amount)> Items =>
            _filter switch
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
            Loot((4, 5), (5, 6));
            Loot(6, 10);
            Loot(7, 15);
            canvas.SetActive(false);
            var indexing = canvas.transform.Find("Indexing").transform;
            indexing.Find("All").GetComponent<Button>().onClick.AddListener(() => Filter("all"));
            indexing.Find("Consumable").GetComponent<Button>().onClick.AddListener(() => Filter("consumable"));
            indexing.Find("Equipment").GetComponent<Button>().onClick.AddListener(() => Filter("equipment"));
            indexing.Find("Resource").GetComponent<Button>().onClick.AddListener(() => Filter("resource"));
            var sorting = canvas.transform.Find("Sorting").transform;
            sorting.Find("ByName").GetComponent<Button>().onClick.AddListener(() => Sort("name"));
            sorting.Find("ByTier").GetComponent<Button>().onClick.AddListener(() => Sort("tier"));
            sorting.Find("ByAmount").GetComponent<Button>().onClick.AddListener(() => Sort("amount"));
        }
        
        private void Filter(string filt)
        {
            _filter = filt;
            UpdateDisplay();
        }

        private void Sort(string sortKey)
        {
            if (_showing && sortKey == _sort)
                _ascending = !_ascending;
            var order = _ascending ? 1 : -1;
            _sort = sortKey;
            
            var sorting  = _sort switch
            {
                "name" => (Comparison<(Item item, int amount)>) ((a, b)
                    => order * string.Compare(a.item.Name, b.item.Name, StringComparison.InvariantCultureIgnoreCase)),
                "tier" => (a, b)
                    => order * (a.item.Tier == b.item.Tier
                        ? string.Compare(a.item.Name, b.item.Name, StringComparison.InvariantCultureIgnoreCase)
                        : a.item.Tier - b.item.Tier),
                "amount" => (a, b) // ascending is descending here
                    => order * (a.amount == b.amount
                        ? string.Compare(a.item.Name, b.item.Name, StringComparison.InvariantCultureIgnoreCase)
                        : b.amount - a.amount),
                _ => throw new ArgumentException("Invalid sort key")
            };
            _items.Sort(sorting);
            UpdateDisplay();
        }

        public void Loot(int itemId, int amount)
        {
            var item = db.GetItem(itemId);
        
            if (item is Equipment) // Add amount instances of the equipment (with different stats)
            {
                for (var i = 0; i < amount; i++)
                    _items.Add((db.GetItem(itemId), 1));
            } else
            {
                var index = _items.FindIndex(i => i.item.Id == itemId);
                if (index == -1)
                    _items.Add((item, amount));
                else
                    _items[index] = (item, _items[index].amount + amount);
            }
            if (_showing)
                UpdateDisplay();
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
        
        public bool Remove(int itemId, int amount)
        {
            var index = _items.FindIndex(i => i.item.Id == itemId);
            if (index == -1)
                return false;
            if (_items[index].amount == amount)
                _items.RemoveAt(index);
            else if (_items[index].amount < amount)
                return false;
            else _items[index] = (_items[index].item, _items[index].amount - amount);
            return true;
        }
        
        public bool Remove(int itemId)
            => Remove(itemId, 1);

        public bool Remove(IEnumerable<(int itemId, int amount)> list)
            => list.All(i => Remove(i.itemId, i.amount));

        public void Show()
        {
            canvas.SetActive(true);
            Sort(_sort);
            _showing = true;
        }
        
        public void Hide()
        {
            canvas.SetActive(false);
            _showing = false;
        }
            
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
                if (instance is null)
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
