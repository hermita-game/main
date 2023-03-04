using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        private List<(Item item, int amount)> _items = new();
        private string _filter = "all";
        private string _sort = "name";
        private bool _ascending = true;
        private bool _showing;
        private Timer _refreshTimer;
        private int _lastItemCount;
        
        private Transform _content;
        private RectTransform _contentRect;
        private Scrollbar _scrollbar;
        private Fighting.Player _player;
        private TextMeshProUGUI _playerStatsText;
        private GameObject _tooltip;

        private const KeyCode Key = KeyCode.I;

        private void Update()
        {
            if (!Input.GetKeyDown(Key)) return;
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

        public ItemDatabase db;
        public GameObject canvas;

        private void Start()
        {
            _content = canvas.transform.Find("Scroll View").Find("Viewport").Find("Content");
            _contentRect = _content.GetComponent<RectTransform>();
            _scrollbar = canvas.transform.Find("Scroll View").Find("Scrollbar Vertical").GetComponent<Scrollbar>();
            _player = GetComponent<Fighting.Player>();
            _playerStatsText = canvas.transform.Find("Stats Panel").Find("Player Stats").GetComponent<TextMeshProUGUI>();
            _tooltip = canvas.transform.Find("Tooltip").gameObject;
            _tooltip.SetActive(false);
            canvas.SetActive(false);

            Loot(0,1,2,3,4,5,6,7,8,9,10,11,12,100,101,102,103,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224);
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

        private void Show()
        {
            canvas.SetActive(true);
            UpdatePlayerStats();
            Sort(_sort);
            _showing = true;
            _refreshTimer = new Timer(_ =>
            {
                Debug.Log(_player.GetStatsString());
                _playerStatsText.text = _player.GetStatsString();
                // force update
                _playerStatsText.enabled = false;
                _playerStatsText.enabled = true;
            }, null, 0, 1000);
        }

        private void Hide()
        {
            canvas.SetActive(false);
            _showing = false;
            _refreshTimer.Dispose();
        }
            
        private void UpdateDisplay()
        {
            _tooltip.SetActive(true);
            var nbRows = (Items.Count - 1) / 6 + 1;
            // Set the content's height to the number of rows
            _contentRect.sizeDelta = new Vector2(0, 58*nbRows-3);
            foreach (Transform child in _content)
                Destroy(child.gameObject);

            foreach (var (item, amount) in Items)
            {
                // Instantiate Item Prefab as GameObject and set its parent to the content
                var instance = Instantiate(Resources.Load("Prefabs/Item"), _content) as GameObject;
                if (instance is null)
                    throw new Exception("Item prefab not found!");
                // Set the item's name and amount
                instance.GetComponent<UIItem>().SetItem(item, amount);
            }

            // change scroll steps in scrollbar
            _scrollbar.numberOfSteps = Math.Max(1, nbRows - 4);
            _tooltip.SetActive(false);
        }
        
        private void UpdatePlayerStats()
        {
            _playerStatsText.gameObject.SetActive(false);
            _playerStatsText.text = _player.GetStatsString();
            // update displayed stats
            _playerStatsText.gameObject.SetActive(true);
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
