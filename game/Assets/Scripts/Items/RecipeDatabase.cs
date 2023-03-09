using System.Collections.Generic;
using UnityEngine;

// building: "lab 1" or "ws 5" or "wb 69"

namespace Items
{
    public class RecipeDatabase : MonoBehaviour
    {
        private List<Recipe> _recipes;
        // Should be attached to a game object in the housing
        void Awake()
        {
            BuildDatabase();
        }

        private void BuildDatabase()
        {
            _recipes = new List<Recipe>
            {
                new Recipe( //mechanical wand
                    0,
                    "ws 1",
                    1,
                    new List<(int itemId, int amount)> { (224, 2), (219, 1) },
                    "wand"),
                new Recipe( //wand of the forest
                    1,
                    "ws 2",
                    3,
                    new List<(int itemId, int amount)> { (223, 8), (216, 1), (217, 1), (218, 1), (219, 1) },
                    "wand"),
                new Recipe( //huntsman cloak
                    2,
                    "ws 1",
                    7,
                    new List<(int itemId, int amount)> { (221, 6), (222, 2) },
                    "robe"),
                new Recipe( //fire necklace
                    3,
                    "ws 1",
                    9,
                    new List<(int itemId, int amount)> { (220, 1), (216, 1) },
                    "necklace"),
                new Recipe( //water necklace
                    4,
                    "ws 1",
                    10,
                    new List<(int itemId, int amount)> { (220, 1), (217, 1) },
                    "necklace"),
                new Recipe( //earth necklace
                    5,
                    "ws 1",
                    11,
                    new List<(int itemId, int amount)> { (220, 1), (218, 1) },
                    "necklace"),
                new Recipe( //thunder necklace
                    6,
                    "ws 1",
                    12,
                    new List<(int itemId, int amount)> { (220, 1), (219, 1) },
                    "necklace"),
                new Recipe( //potion hp
                    100,
                    "ws 1",
                    100,
                    new List<(int itemId, int amount)> { (202, 1), (201, 1) },
                    "potion"),
                new Recipe( //potion regen hp
                    101,
                    "ws 1",
                    101,
                    new List<(int itemId, int amount)> { (202, 1), (200, 1) },
                    "potion"),
                new Recipe( //potion mp
                    102,
                    "ws 1",
                    102,
                    new List<(int itemId, int amount)> { (203, 1), (201, 1) },
                    "potion"),
                new Recipe( //potion regen mp
                    103,
                    "ws 1",
                    103,
                    new List<(int itemId, int amount)> { (203, 1), (200, 1) },
                    "potion"),
            };
        }
    }
}
