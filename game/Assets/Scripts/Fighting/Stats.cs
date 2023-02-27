using System;
using System.Collections.Generic;
using System.Linq;

public enum Type
{
    Percent, // Percent of base stat
    Flat     // Flat value
}

namespace Fighting
{
    public class Stats
    {
        private readonly Dictionary<string, (Type type, float val)> _stats;
    
        public (Type type, float val) this[string stat]
        {
            get => _stats[stat];
            set => _stats[stat] = value;
        }
    
        public Stats()
            => _stats = new Dictionary<string, (Type type, float val)>();
        
        public Stats(Dictionary<string, (Type type, float val)> stats)
            => _stats = stats;
        
        public Stats(params (string, Type, float)[] stats)
        {
            _stats = new Dictionary<string, (Type type, float val)>();
            foreach (var (stat, type, value) in stats)
                _stats[stat] = (type, value);
        }
        
        // <summary>
        //    Creates a new stats object. stats is a string like "hp +10%, atk +5, def -2%,...".
        // </summary>
        public Stats(string stats)
        {
            _stats = new Dictionary<string, (Type type, float val)>();
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
        
        public IEnumerable<(string, (Type type, float val))> GetStats()
            => _stats.Select(stat => (stat.Key, stat.Value));

        public static Stats Ceil(Stats stats, Stats ceiling)
        {
            var result = new Stats();
            foreach (var (key, (type, val)) in stats.GetStats())
                if (type != Type.Flat)
                    throw new Exception("Ceiling only works with flat stats");
                else
                    result[key] = (type, Math.Min(val, ceiling[key].val));
            return result;
        }
    }
}
