using System.Collections;
using System.Collections.Generic;
using Fighting;
using UnityEngine;



namespace Items
{
    public class Item
    {
        public int Id;
        public string Name;
        public string Description;
        public char Tier;
        public Sprite Icon;
        
        public Item(int id, string name, string description, char tier, Sprite icon)
        {
            Id = id;
            Name = name;
            Tier = tier;
            Description = description;
            Icon = icon;
        }
    }

    public class Equipment : Item
    {
        public Stats BaseStats;
        
        public Equipment(int id, string name, string description, char tier, Sprite icon, string stats)
            : base(id, name, description, tier, icon)
        {
            BaseStats = new Stats(stats);
        }
    }
    
    public class Consumable : Item
    {
        public Stats Stats;

        public Consumable(int id, string name, string description, char tier, Sprite icon, string stats)
            : base(id, name, description, tier, icon)
        {
            Stats = new Stats(stats);
        }
        
        public void Use(Stats stats, Stats maxStats)
        {
            foreach (var (stat, (type, val)) in Stats.GetStats())
            {
                if (type == Type.Flat)
                    stats[stat] = (Type.Flat, stats[stat].val + val);
                else
                    stats[stat] = (Type.Flat, stats[stat].val + maxStats[stat].val * val / 100);
            }
        }
    }
}