using System;
using System.Collections.Generic;

namespace Common.BehaviourTrees
{
    public static class Extensions
    {
        #region Array
        public static bool IsNullOrEmpty<T>(this T[] self)
        {
            return self == null || self.Length == 0;
        }

        public static bool Contains<T>(this T[] self, T value)
        {
            return Array.IndexOf(self, value) != -1;
        }
        #endregion

        #region Dictionary
        public static TValue GetOrCompute<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TValue> computor)
        {
            if (!self.TryGetValue(key, out var result))
            {
                self[key] = result = computor();
            }
            return result;
        }
        #endregion

        #region Random
        public static float NextFloat(this Random self)
        {
            return (float)self.NextDouble();
        }

        public static float NextFloat(this Random self, float max)
        {
            return self.NextFloat() * max;
        }

        public static float NextFloat(this Random self, float min, float max)
        {
            return min + self.NextFloat() * (max - min);
        }
        #endregion

        #region String
        public static bool IsEmpty(this string self)
        {
            return self.Length == 0;
        }
        #endregion
    }
}
