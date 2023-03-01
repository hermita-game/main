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
                new Recipe(
                    0,
                    "lab 1",
                    2,
                    new List<(int itemId, int amount)> { (6, 2), (7, 1) })
            };
        }
    }
}
