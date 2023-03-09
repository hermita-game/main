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
            var potion_set = Resources.LoadAll("Items/potions", typeof(Sprite)).Cast<Sprite>().ToArray();
            var necklace_set = Resources.LoadAll("Items/necklaces", typeof(Sprite)).Cast<Sprite>().ToArray();
            var animal_skins_set = Resources.LoadAll("Items/animal_skins", typeof(Sprite)).Cast<Sprite>().ToArray();
            var gems_set = Resources.LoadAll("Items/Elemental_gems", typeof(Sprite)).Cast<Sprite>().ToArray();
            var living_wood_set = Resources.LoadAll("Items/living_wood", typeof(Sprite)).Cast<Sprite>().ToArray();
            var mechanical_arm_set = Resources.LoadAll("Items/mechanical_arm", typeof(Sprite)).Cast<Sprite>().ToArray();
            var elemental_ressources_set = Resources.LoadAll("Items/Elemental_ressources", typeof(Sprite)).Cast<Sprite>().ToArray();

            _items = new List<Item>
            {
                new Equipment( // wands
                    0,
                    "Wizard wand",
                    "A basic wand used by wizards",
                    'D',
                    set1[71],
                    StuffType.Wand,
                    "atk +25%"),
                new Equipment(
                    1, 
                    "Mechanical wand",
                    "A wand created with mechanical arms of ancient golems",
                    'C',
                    set1[72],
                    StuffType.Wand,
                    "atk +50%"),
                new Equipment(
                    2,
                    "Forest monster's arm",
                    "A wand that can be looted on the forest boss",
                    'C',
                    set1[73],
                    StuffType.Wand,
                    "atk +75%"),
                new Equipment(
                    3,
                    "Wand of the forest",
                    "A magical wand created with magical ressources from the forest",
                    'B',
                    set1[74],
                    StuffType.Wand,
                    "atk +100%"),
                new Equipment( // robes
                    4,
                    "Wizard robe",
                    "A basic robe used by wizards",
                    'D',
                    set1[83],
                    StuffType.Robe,
                    "res +5%"),
                new Equipment(
                    5,
                    "Shaman robe",
                    "A robe used by goblin shamans",
                    'C',
                    set1[83],
                    StuffType.Robe,
                    "res +15%"),
                new Equipment(
                    6,
                    "Bling bling king cape",
                    "The cape of the goblin king living in the ruins",
                    'C',
                    set1[83],
                    StuffType.Robe,
                    "res +10%, spd +10%, atk +10%"),
                new Equipment(
                    7,
                    "Huntsman cloak",
                    "A cloak created with animal skins ",
                    'C',
                    set1[83],
                    StuffType.Robe,
                    "res +20%"),
                new Equipment(
                    8,
                    "First level boss cloak",
                    "A cloak that can be obtained on the first level final boss",
                    'B',
                    set1[83],
                    StuffType.Robe,
                    "res +15%, atk +25%, spd +10%"),
                new Equipment( // necklaces
                    9,
                    "Fire necklace",
                    "A necklace crafted with a fire gem",
                    'C',
                    necklace_set[1],
                    StuffType.Necklace,
                    "atk +15%"),
                new Equipment(
                    10,
                    "Water necklace",
                    "A necklace crafted with a water gem",
                    'C',
                    necklace_set[2],
                    StuffType.Necklace,
                    "hp-regen +2"),
                new Equipment(
                    11,
                    "Earth necklace",
                    "A necklace crafted with a earth gem",
                    'C',
                    necklace_set[3],
                    StuffType.Necklace,
                    "res +20%"),
                new Equipment(
                    12,
                    "Thunder necklace",
                    "A necklace crafted with a thunder gem",
                    'C',
                    necklace_set[4],
                    StuffType.Necklace,
                    "spd +15%"),
                new Consumable( // consumables
                    100, 
                    "Heal potion",
                    "A simple heal potion",
                    'C',
                    potion_set[0],
                    "hp +50"),
                new Consumable( 
                    101, 
                    "Health regeneration potion",
                    "A simple health regeneration potion",
                    'C',
                    potion_set[1],
                    "hp-regen +5",
					15),
                new Consumable( 
                    102,
                    "Mana potion",
                    "A simple mana potion",
                    'C',
                    potion_set[2],
                    "mp +40"),
                new Consumable( 
                    103, 
                    "Mana regeneration potion",
                    "A simple mana regeneration potion",
                    'C',
                    potion_set[3],
                    "mp-regen +4",
                    15),
                new Item( // elemental dusts
                    200,
                    "Fire dust",
                    "A dust that can be used to make spells and fire pearls",
                    'D',
                    elemental_ressources_set[0]),
                new Item( 
                    201,
                    "Water dust",
                    "A dust that can be used to make spells and water pearls",
                    'D',
                    elemental_ressources_set[1]),
                new Item( 
                    202,
                    "Earth dust",
                    "A dust that can be used to make spells and earth pearls",
                    'D',
                    elemental_ressources_set[2]),
                new Item( 
                    203,
                    "Thunder dust",
                    "A dust that can be used to make spells and thunder pearls",
                    'D',
                    elemental_ressources_set[3]),
                new Item( // elemental pearls
                    204,
                    "Fire pearl",
                    "A pearl that can be used to make spells and fire orbs",
                    'C',
                    elemental_ressources_set[4]),
                new Item( 
                    205,
                    "Water pearl",
                    "A pearl that can be used to make spells and water orbs",
                    'C',
                    elemental_ressources_set[5]),
                new Item( 
                    206,
                    "Earth pearl",
                    "A pearl that can be used to make spells and earth orbs",
                    'C',
                    elemental_ressources_set[6]),
                new Item( 
                    207,
                    "Thunder pearl",
                    "A pearl that can be used to make spells and thunder orbs",
                    'C',
                    elemental_ressources_set[7]),
                new Item( // elemental orbs
                    208,
                    "Fire orb",
                    "An orb that can be used to make spells",
                    'B',
                    elemental_ressources_set[8]),
                new Item( 
                    209,
                    "Water orb",
                    "An orb that can be used to make spells",
                    'B',
                    elemental_ressources_set[9]),
                new Item( 
                    210,
                    "Earth orb",
                    "An orb that can be used to make spells",
                    'B',
                    elemental_ressources_set[10]),
                new Item( 
                    211,
                    "Thunder orb",
                    "An orb that can be used to make spells",
                    'B',
                    elemental_ressources_set[11]),
                new Item( // gems
                    212,
                    "Fire gem",
                    "A gem that needs to be polished to be useful",
                    'D',
                    gems_set[0]),
                new Item( 
                    213,
                    "Water gem",
                    "A gem that needs to be polished to be useful",
                    'D',
                    gems_set[1]),
                new Item( 
                    214,
                    "Earth gem",
                    "A gem that needs to be polished to be useful",
                    'D',
                    gems_set[2]),
                new Item( 
                    215,
                    "Thunder gem",
                    "A gem that needs to be polished to be useful",
                    'D',
                    gems_set[3]),
                new Item( // polished gems
                    216,
                    "Polished fire gem",
                    "A polished gem that can be used in crafts",
                    'C',
                    gems_set[0]),
                new Item( 
                    217,
                    "Polished water gem",
                    "A polished gem that can be used in crafts",
                    'C',
                    gems_set[1]),
                new Item( 
                    218,
                    "Polished earth gem",
                    "A polished gem that can be used in crafts",
                    'C',
                    gems_set[2]),
                new Item( 
                    219,
                    "Polished thunder gem",
                    "A polished gem that can be used in crafts",
                    'C',
                    gems_set[3]),
                new Item( // crafting materials
                    220,
                    "Simple necklace",
                    "A simple necklace that can be used to craft more powerful necklaces",
                    'D',
                    necklace_set[0]),
                new Item( 
                    221,
                    "Boar skin",
                    "A piece of boar skin that can be used in crafts",
                    'D',
                    animal_skins_set[0]),
                new Item( 
                    222,
                    "Bear skin",
                    "A piece of bear skin that can be used in crafts",
                    'C',
                    animal_skins_set[1]),
                new Item( 
                    223,
                    "Living wood",
                    "A piece of magical wood useful for crafts",
                    'B',
                    living_wood_set[0]),
                new Item( 
                    224,
                    "Mechanical arm",
                    "A piece of mechanical golem that can be useful for some crafts",
                    'C',
                    mechanical_arm_set[0]),
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
