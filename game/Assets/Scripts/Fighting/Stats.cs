using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;



namespace Fighting
{
    /* reference
        hp: health points
        mp: mana points
        atk: attack
        res: resistance
        spd: speed
        hp-regen: health points regeneration
        mp-regen: mana points regeneration
    */
    public class Stats : IEnumerable<(string stat, Stats.Type type, float val)>
    {
        public enum Type
        {
            Percent, // Percent of base stat
            Flat     // Flat value
        }
        private readonly ConcurrentDictionary<string, (Type type, float val)> _stats;
    
        public (Type type, float val) this[string stat]
        {
            get => _stats.ContainsKey(stat) ? _stats[stat] : (Type.Flat, 0);
            set
            {
                if (value.val == 0)
                     _stats.TryRemove(stat, out _);
                else _stats[stat] = value;
            }
        }
    
        public Stats()
            => _stats = new ConcurrentDictionary<string, (Type type, float val)>();

        public Stats(params (string, Type, float)[] stats)
        {
            _stats = new ConcurrentDictionary<string, (Type type, float val)>();
            foreach (var (stat, type, value) in stats)
                _stats[stat] = (type, value);
        }
        
        // <summary>
        //    Creates a new stats object. stats is a string like "hp +10%, atk +5, def -2%,...".
        // </summary>
        public Stats(string stats)
        {
            _stats = new ConcurrentDictionary<string, (Type type, float val)>();
            foreach (var stat in stats.Split(", "))
            {
                var parts = stat.Split(' ');
                var (key, val) = (parts[0], parts[1]);
                var type = Type.Flat;
                if (val[^1] == '%')
                {
                    val = val[..^1];
                    type = Type.Percent;
                }
                var nb = float.Parse(val);
                _stats[key] = (type, nb);
            }
        }
        
        public IEnumerator<(string stat,Type type, float val)> GetEnumerator()
            => _stats.Select(stat
                => (stat.Key, stat.Value.type, stat.Value.val)).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public FlatStats Flatten(FlatStats reference)
        {
            var result = new FlatStats(reference);
            foreach (var (key, type, val) in this)
            {
                if (type == Type.Percent)
                     result[key] += reference[key] * val / 100;
                else result[key] += val;
            }
            return result;
        }

        private static string Round(float val)
            => $"{(val > 0 ? "+" : "")}{Math.Round(val * 10) / 10}";
        public override string ToString()
        {
            var result = "";
            foreach (var (key, type, val) in this)
            {
                if (key.EndsWith("-regen"))
                    result += $"{key[..^6]} {Round(val)}{(type == Type.Percent ? "%" : "")}/s";
                else
                    result += $"{key} {Round(val)}{(type == Type.Percent ? "%" : "")}";
                result += "\n";
            }
            return result;
        }
    }

    public class FlatStats : IEnumerable<(string key, float val)>
    {
        private readonly ConcurrentDictionary<string, float> _stats;
        
        public float this[string stat]
        {
            get => _stats.ContainsKey(stat) ? _stats[stat] : 0;
            set => _stats[stat] = value;
        }
        
        public FlatStats()
            => _stats = new ConcurrentDictionary<string, float>();

        public FlatStats(FlatStats stats)
            => _stats = new ConcurrentDictionary<string, float>(stats._stats);
        
        public FlatStats(Stats stats)
        {
            _stats = new ConcurrentDictionary<string, float>();
            foreach (var (key, type, val) in stats)
                if (type != Stats.Type.Flat)
                    throw new Exception("FlatStats only works with flat stats");
                else _stats[key] = val;
        }

        public static FlatStats operator +(FlatStats a, FlatStats b)
        {
            var result = new FlatStats(a);
            foreach (var (key, val) in b)
                result[key] += result[key];
            return result;
        }
        
        public static FlatStats operator -(FlatStats a, FlatStats b)
        {
            var result = new FlatStats(a);
            foreach (var (key, val) in b)
                result[key] -= result[key];
            return result;
        }
        
        public static FlatStats operator *(FlatStats a, float b)
        {
            var result = new FlatStats(a);
            foreach (var (key, val) in result)
                result[key] *= b;
            return result;
        }
        
        public static FlatStats operator /(FlatStats a, float b)
        {
            var result = new FlatStats(a);
            foreach (var (key, val) in result)
                result[key] /= b;
            return result;
        }
        
        public void Ceil(FlatStats ceiling)
        {
            foreach (var (key, val) in this)
                this[key] = Math.Min(val, ceiling[key]);
        }

        public IEnumerator<(string key, float val)> GetEnumerator()
            => _stats.Select(stat => (stat.Key, stat.Value)).GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
