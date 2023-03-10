using System;
using System.Collections.Generic;
using System.Linq;
using Fighting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public ItemDatabase db;
        public GameObject canvas;
        public KeyCode key = KeyCode.I;

        
        private List<(Item item, int amount)> _items = new();
        private string _filter = "all";
        private string _sort = "name";
        private bool _ascending = true;
        private bool _showing;
        
        private Transform _content;
        private RectTransform _contentRect;
        private Scrollbar _scrollbar;
        private Player _player;
        private TextMeshProUGUI _playerStatsText;
        private GameObject _tooltip;
        private Tooltip _tooltipScript;
        private GameObject _itemPrefab;
        private (Transform neck, Transform chest, Transform wand) _stuff;
        
        private void Update()
        {
            if (!Input.GetKeyDown(key)) return;
            if (_showing) Hide();
            else Show();
        }

        private List<(Item item, int amount)> Items =>
            _filter switch
            {
                "all" => _items,
                "consumable" => _items.FindAll(i => i.item is Consumable),
                "equipment" => _items.FindAll(i => i.item is Equipment),
                "resource" => _items.FindAll(i => i.item is not Consumable and not Equipment),
                _ => throw new ArgumentException("Invalid filter")
            };

        private void Start()
        {
            _content = canvas.transform.Find("Scroll View").Find("Viewport").Find("Content");
            _contentRect = _content.GetComponent<RectTransform>();
            _scrollbar = canvas.transform.Find("Scroll View").Find("Scrollbar Vertical").GetComponent<Scrollbar>();
            _player = Tools.GetPlayer();
            _playerStatsText = canvas.transform.Find("Stats Panel").Find("Player Stats").GetComponent<TextMeshProUGUI>();
            _tooltip = canvas.transform.Find("Tooltip").gameObject;
            _tooltipScript = _tooltip.GetComponent<Tooltip>();
            _tooltip.SetActive(false);
            canvas.SetActive(false);
            _itemPrefab = Resources.Load("Prefabs/Item") as GameObject;
            _stuff = (
                canvas.transform.Find("Stuff").Find("Neck"),
                canvas.transform.Find("Stuff").Find("Chest"),
                canvas.transform.Find("Stuff").Find("Wand")
            );
            _player.OnEquipmentUpdate += UpdatePlayerEquipment;

            Loot(0,1,2,3,4,5,6,7,8,9,10,11,12,100,101,102,103,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224);
            var indexing = canvas.transform.Find("Indexing").transform;
            indexing.Find("All").GetComponent<Button>().onClick.AddListener(() => Filter("all"));
            indexing.Find("Consumable").GetComponent<Button>().onClick.AddListener(() => Filter("consumable"));
            indexing.Find("Equipment").GetComponent<Button>().onClick.AddListener(() => Filter("equipment"));
            indexing.Find("Resource").GetComponent<Button>().onClick.AddListener(() => Filter("resource"));
            var sorting = canvas.transform.Find("Sorting").transform;
            sorting.Find("ByName").GetComponent<Button>().onClick.AddListener(() => CallSort("name"));
            sorting.Find("ByTier").GetComponent<Button>().onClick.AddListener(() => CallSort("tier"));
            sorting.Find("ByAmount").GetComponent<Button>().onClick.AddListener(() => CallSort("amount"));
        }
        
        private void Filter(string filt)
        {
            _filter = filt;
            UpdateDisplay();
        }

        private void CallSort(string sortKey)
        {
            if (sortKey == _sort)
                _ascending = !_ascending;
            _sort = sortKey;
            Sort();
            UpdateDisplay();
        }
        
        private void Sort()
        {
            var order = _ascending ? 1 : -1;

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

        public void Loot(Item item)
        {
            if (item is Equipment)
                _items.Add((item, 1));
            else
            {
                var index = _items.FindIndex(i => i.item.Id == item.Id);
                if (index == -1)
                    _items.Add((item, 1));
                else
                    _items[index] = (item, _items[index].amount + 1);
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
            if (index == -1 || _items[index].amount < amount)
                return false;
            
            if (_items[index].amount == amount)
                _items.RemoveAt(index);
            else _items[index] = (_items[index].item, _items[index].amount - amount);
            if (_showing)
                UpdateDisplay();
            return true;
        }

        public bool Remove(Equipment item)
        {
            var index = _items.FindIndex(i => i.item is Equipment e && Object.ReferenceEquals(e, item));
            if (index == -1)
                return false;
            _items.RemoveAt(index);
            if (_showing)
                UpdateDisplay();
            return true;
        }
        
        public bool Remove(int itemId)
            => Remove(itemId, 1);

        public bool Remove(IEnumerable<(int itemId, int amount)> list)
            => list.All(i => Remove(i.itemId, i.amount));

        private void Show()
        {
            canvas.SetActive(true);
            UpdatePlayerStats();
            UpdateDisplay();
            _showing = true;
            _player.OnStatsUpdate += UpdatePlayerStats;
        }

        private void Hide()
        {
            canvas.SetActive(false);
            _showing = false;
            _player.OnStatsUpdate -= UpdatePlayerStats;
        }
            
        private void UpdateDisplay()
        {
            Sort();
            _tooltip.SetActive(true);
            var nbRows = (Items.Count - 1) / 6 + 1;
            // Set the content's height to the number of rows
            _contentRect.sizeDelta = new Vector2(0, 58*nbRows-3);
            foreach (Transform child in _content)
                Destroy(child.gameObject);

            foreach (var (item, amount) in Items)
            {
                // Instantiate Item Prefab as GameObject and set its parent to the content
                var instance = Instantiate(_itemPrefab, _content);
                // Set the item's name and amount
                var uiItem = instance.GetComponent<UIItem>();
                uiItem.SetItem(item, amount);
                uiItem.tooltip = _tooltipScript;
            }

            // change scroll steps in scrollbar
            _scrollbar.numberOfSteps = Math.Max(1, nbRows - 4);
            _tooltip.SetActive(false);
        }
        
        private void UpdatePlayerStats()
        {
            _playerStatsText.text = _player.GetStatsString();
        }
        
        private void UpdatePlayerEquipment()
        {
            var slots = new[] { _stuff.neck, _stuff.chest, _stuff.wand };
            var equipment = new[] { _player.Equipment.necklace, _player.Equipment.robe, _player.Equipment.wand };
            for (var i = 0; i < 3; i++)
            {
                var eq = equipment[i];
                var slot = slots[i];
                foreach (Transform o in slot)
                    Destroy(o.gameObject);
                if (eq is null) continue;
                var uiItem = Instantiate(_itemPrefab, slot).GetComponent<UIItem>();
                uiItem.SetItem(eq, 1);
                uiItem.tooltip = _tooltipScript;
            }
        }

        public List<(int, int)> ToSave()
        {
            // second int is the amount of items or the seed in case of equipment
            var save = new List<(int, int)>();
            foreach (var (item, amount) in _items)
                save.Add(item is Equipment equipment ?
                    (equipment.Id, equipment.Seed) : (item.Id, amount));
            return save;
        }

        public void LoadSave(List<(int, int)> save)
        {
            _items = new List<(Item, int)>();
            foreach (var (id, seed) in save)
            {
                var item = db.GetItem(id, seed);
                _items.Add(item is Equipment equipment ? (equipment, 1) : (item, seed /* amount */));
            }
        }
    }
}
