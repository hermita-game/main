using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Items;
using UnityEngine;

namespace Fighting
{
    public class Player : MonoBehaviour
    {
        public FlatStats BaseStats; // Basic stats of the player
        public FlatStats EquipmentStats; // Stats from equipment
        public FlatStats MaxPlayerStats; // Max stats of the player
        public FlatStats PlayerStats; // Stats with modifiers and used for calculations
        public List<(int duration, FlatStats effect)> Effects; // Effects that are applied to the player

        public (Equipment necklace, Equipment robe, Equipment wand) Equipment;

        private void Awake()
        {
            var baseStats = new FlatStats(new Stats("hp +100, mp +100, atk +10, res +10, hp-regen +1, mp-regen +1"));
            BaseStats = new FlatStats(baseStats);
            EquipmentStats = new FlatStats();
            MaxPlayerStats = new FlatStats(baseStats);
            PlayerStats = new FlatStats(baseStats);
        }

        private void UpdateEquipmentStats()
        {
            PlayerStats -= EquipmentStats; // Remove old equipment stats
            EquipmentStats = new FlatStats();
            foreach (var eq in new[] {Equipment.necklace, Equipment.robe, Equipment.wand})
            {
                if (eq is null) continue;
                var flatStats = eq.BaseStats.Flatten(BaseStats);
                foreach (var (stat, val) in flatStats)
                    EquipmentStats[stat] += val;
            }
            PlayerStats += EquipmentStats; // Add new equipment stats
            MaxPlayerStats = BaseStats + EquipmentStats;
        }

        private void InitRegenLoop()
        {
            // Stats are stored in ConcurrentDictionary, so we can access them from multiple threads
            var timer = new Timer(_ =>
            {
                // potion effects
                for (var i = 0; i < Effects.Count; i++)
                {
                    var (duration, effect) = Effects[i];
                    if (Effects[i].duration == 0)
                    {
                        MaxPlayerStats -= effect;
                        PlayerStats -= effect;
                        Effects.RemoveAt(i);
                        i--;
                    }
                    else Effects[i] = (duration - 1, effect);
                }

                // Apply *-regen stats
                foreach (var (key, val) in PlayerStats.Where(x => x.key.EndsWith("-regen")))
                    PlayerStats[key[..^6]] += val;
                PlayerStats.Ceil(MaxPlayerStats);
            }, null, 0, 1000);
        }
        
        private void AddEffect(FlatStats effect, int duration)
        {
            Effects.Add((duration, effect));
            MaxPlayerStats += effect;
            PlayerStats += effect;
        }
        
        public void UseConsumable(Consumable consumable)
        {
            var flatStats = consumable.Stats.Flatten(PlayerStats);
            AddEffect(flatStats, consumable.Duration);
        }
        
        public Equipment Equip(Equipment equipment)
        {
            Equipment oldEquipment;
            switch (equipment.StuffType)
            {
                case StuffType.Necklace:
                    oldEquipment = Equipment.necklace;
                    Equipment.necklace = equipment;
                    break;
                case StuffType.Robe:
                    oldEquipment = Equipment.robe;
                    Equipment.robe = equipment;
                    break;
                case StuffType.Wand:
                    oldEquipment = Equipment.wand;
                    Equipment.wand = equipment;
                    break;
                default:
                    throw new Exception("Unknown equipment type");
            }
            UpdateEquipmentStats();
            return oldEquipment;
        }
        
        private float Round(float val)
            => (float) Math.Round(val * 10) / 10;
        private string ParseDiffNumber(float val)
            => val switch
            {
                > 0 => $"<color=green>+{Round(val)}</color>",
                < 0 => $"<color=red>{Round(val)}</color>",
                _ => ""
            };

        public string GetStatsString()
        {
            var str = "";
            foreach (var (stat, val) in PlayerStats)
            {
                if (stat.EndsWith("-regen")) continue;
                str += $"{stat}: {Round(val)}/{Round(MaxPlayerStats[stat])}";
                var regen = PlayerStats[stat + "-regen"];
                if (regen != 0)
                    str += $" {ParseDiffNumber(regen)}/s";
                str += "\n";
            }
            return str;
        }
    }
}
