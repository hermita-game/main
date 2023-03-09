using System;
using System.Collections.Generic;
using System.Linq;

namespace Items
{
    public class Recipe
    {
        public int Id;
        public int ItemId;
        public List<(int itemId, int amount)> Ingredients;
        public (int level, BuildingType building) Requirements;
        public CraftType craftType;

        public Recipe(int id, string building, int result, List<(int itemId, int amount)> ingredientsIds, string crafttype)
        {
            Id = id;
            ItemId = result;
            Ingredients = ingredientsIds;
            var buildInfo = building.Split(' ');
            var buildingType = buildInfo[0] switch
            {
                "lab" => BuildingType.Laboratory,
                "ws" => BuildingType.Workshop,
                "wb" => BuildingType.Workbench,
                _ => throw new ArgumentException("Invalid building key")
            };
            var level = int.Parse(buildInfo[1]);
            Requirements = (level, buildingType);
            var craftType = crafttype.Trim() switch
            {
                "wand" => CraftType.Wand,
                "necklace" => CraftType.Necklace,
                "robe" => CraftType.Robe,
                "potion" => CraftType.Potion,
                _ => throw new ArgumentException("Invalid crafttype")
            };
        }
        
        public List<(int item, int amount)> GetMissingIngredients(List<Item> inventory)
            => (from ingredient in Ingredients let count = inventory.Count(item => item.Id == ingredient.itemId)
                where count < ingredient.amount select (ingredient.itemId, ingredient.amount - count)).ToList();
        
        public bool CanCraft(List<Item> inventory)
            => GetMissingIngredients(inventory).Count == 0;
        
        public bool CanCraft(List<Item> inventory, out List<(int itemId, int amount)> missingIngredients)
        {
            missingIngredients = GetMissingIngredients(inventory);
            return missingIngredients.Count == 0;
        }
        
        public bool MeetsRequirements(int level, BuildingType building)
            => level >= Requirements.level && building == Requirements.building;
    }
}