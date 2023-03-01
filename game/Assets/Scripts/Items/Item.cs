using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fighting;
using UnityEngine;

namespace Items
{
    public enum StuffType
    {
        Wand,
        Armor,
        Necklace
    }

    public class Item
    {
        public int Id;
        public string Name;
        public string Description;
        public int Tier;
        public Sprite Icon;
        
        
        public Item(int id, string name, string description, char tier, Sprite icon)
        {
            Id = id;
            Name = name;
            Tier = tier switch
            {
                'S' => 0,
                >= 'A' and <= 'D' => tier - 'A' + 1,
                _ => throw new ArgumentException("Invalid tier")
            };
            Description = description;
            Icon = icon;
        }

        protected Item(int id, string name, string description, int tier, Sprite icon)
        {
            Id = id;
            Name = name;
            Tier = tier;
            Description = description;
            Icon = icon;
        }
        
        public static string AmountToString(int amount)
            => amount switch
                {
                    < 1000 => amount.ToString(),
                    < 1000000 => $"{amount / 1000}K",
                    < 1000000000 => $"{amount / 1000000}M",
                    _ => $"{amount / 1000000000}B"
                };
    }

    public class Equipment : Item
    {
        public Stats BaseStats;
        public StuffType StuffType;
        
        public Equipment(int id, string name, string description, char tier, Sprite icon, StuffType type,  string stats)
            : base(id, name, description, tier, icon)
        {
            BaseStats = new Stats(stats);
            StuffType = type;
        }
        
        public Equipment(int id, string name, string description, char tier, Sprite icon, Stats stats)
            : base(id, name, description, tier, icon)
        {
            BaseStats = stats;
        }
        
        private Equipment(int id, string name, string description, int tier, Sprite icon, Stats stats)
            : base(id, name, description, tier, icon)
        {
            BaseStats = stats;
        }

        public Equipment GetEquipmentInstance(int seed)
        {
            const float delta = 0.05f; // +-5% of base stat
            var random = new System.Random(seed);
            var newStats = new Stats();
            foreach (var (key, (type, val)) in BaseStats.GetStats())
                newStats[key] = (type, val * (1 + (float) random.NextDouble() * delta * 2 - delta));
            return new Equipment(Id, Name, Description, Tier, Icon, newStats);
        }
        
        public static void Apply(Stats player, List<Stats> equipment)
        {
            // Packing effects
            var (percents, flats) = (new Stats(), new Stats());
            foreach (var stats in equipment)
            {
                foreach (var (stat, (type, val)) in stats.GetStats())
                {
                    if (type == Type.Percent)
                        percents[stat] = (Type.Percent, percents[stat].val + val);
                    else
                        flats[stat] = (Type.Flat, flats[stat].val + val);
                }
            }
            
            // Applying effects
            foreach (var (stat, (type, val)) in percents.GetStats())
                player[stat] = (Type.Flat, player[stat].val + player[stat].val * val / 100);
            foreach (var (stat, (type, val)) in flats.GetStats())
                player[stat] = (Type.Flat, player[stat].val + val);
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