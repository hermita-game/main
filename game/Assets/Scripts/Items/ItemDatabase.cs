using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemDatabase : MonoBehaviour
    {
        private List<Item> _items = new();
    
        private void Awake()
        {
            BuildDatabase();
            DontDestroyOnLoad(gameObject);
        }

        private void BuildDatabase()
        {
            var set1 = Resources.LoadAll("Items/Shikashi", typeof(Sprite)).Cast<Sprite>().ToArray();
            _items = new List<Item>
            {
                new Equipment(
                    0,
                    "Wizard wand",
                    "A wand used by wizards",
                    'D',
                    set1[71],
                    StuffType.Wand,
                    "hp +25%"),
                new Equipment(
                    1, 
                    "Mechanical wand",
                    "A wand used by mechanical wizards",
                    'C',
                    set1[73],
                    StuffType.Wand,
                    "atk +50%"),
                new Equipment(
                    2,
                    "Forest boss' arm",
                    "A wand created with the arms of the forest miniboss",
                    'C',
                    set1[74],
                    StuffType.Wand,
                    "atk +75%"),
                new Consumable(
                    3, 
                    "Heal",
                    "A simple heal potion",
                    'D',
                    set1[106],
                    "hp +50"),
                new Item(
                    4,
                    "Water dust",
                    "A dust that can be used to make water pearls",
                    'C',
                    set1[263]),
                new Item(
                    5,
                    "Water pearl",
                    "A pearl that can be used to make water orbs",
                    'B',
                    set1[10]),
                new Item(
                    6,
                    "Mechanical arm",
                    "An arm that belonged to the forest miniboss",
                    'C',
                    set1[93]),
                new Item(
                    7,
                    "Thunder Gem",
                    "A gem that can be used to make thunder orbs",
                    'D',
                    set1[242]),
            };
        }

        public Item GetItem(int id)
        {
            var item = _items.First(item => item.Id == id);
            if (item is Equipment equipment)
                return equipment.GetEquipmentInstance(Random.Range(0, 100000));
            return item;
        }
        
        public Item GetItem(string name)
        {
            var item = _items.First(item => item.Name == name);
            if (item is Equipment equipment)
                return equipment.GetEquipmentInstance(Random.Range(0, 100000));
            return item;
        }
        
        /*
         <summary>
            Get an item with a seed, if not equipment, seed is ignored
         </summary>
        */
        public Item GetItem(int id, int seed)
        {
            var item = _items.First(item => item.Id == id);
            if (item is Equipment equipment)
                return equipment.GetEquipmentInstance(seed);
            return item;
        }
    }
}
