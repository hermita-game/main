using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    public class ItemDatabase : MonoBehaviour
    {
        public List<Item> items = new();
    
        private void Awake()
        {
            BuildDatabase();
            DontDestroyOnLoad(gameObject);
        }

        void BuildDatabase()
        {
            var set1 = Resources.LoadAll("Items/Shikashi", typeof(Sprite)).Cast<Sprite>().ToArray();
            items = new List<Item>()
            {
                new Equipment(0, "Wizard wand", "A wand used by wizards", 'D', set1[71], "hp +25%"),
                new Equipment(1, "Mechanical wand", "A wand used by mechanical wizards", 'C', set1[73], "atk +50%"),
                new Equipment(2, "Forest boss' arm", "A wand that formerly was the arm of the forest miniboss", 'D', set1[74], "atk +75%"),
            };
        }

        public Item GetItem(int id)
        {
            var item = items.First(item => item.Id == id);
            if (item is Equipment equipment)
                return equipment.GetEquipmentInstance(Random.Range(0, 100000));
            return item;
        }
    }
}
