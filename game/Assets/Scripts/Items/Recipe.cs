using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Recipe
    {
        public int Id;
        public Item Result;
        public List<Item> Ingredients;
        public (int level, BuildingType building) Requirements;
        
        public Recipe(int id, Item result, List<Item> ingredients, (int level, BuildingType building) requirements)
        {
            Id = id;
            Result = result;
            Ingredients = ingredients;
            Requirements = requirements;
        }
    }
}