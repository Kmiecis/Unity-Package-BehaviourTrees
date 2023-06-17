using System;
using System.Collections.Generic;

namespace Common.BehaviourTrees
{
    public static class Extensions
    {
        #region Random
        public static long NextLong(this Random self, long min, long max)
        {
            ulong range = (ulong)(max - min);
            var bytes = new byte[8];
            ulong rand;
            do
            {
                self.NextBytes(bytes);
                rand = (ulong)BitConverter.ToInt64(bytes, 0);
            }
            while (rand > ulong.MaxValue - ((ulong.MaxValue % range) + 1) % range);
            return (long)(rand % range) + min;
        }

        public static long NextLong(this Random self, long max)
        {
            return self.NextLong(long.MinValue, max);
        }

        public static long NextLong(this Random self)
        {
            return self.NextLong(long.MaxValue);
        }
        #endregion

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
            TValue result;
            if (!self.TryGetValue(key, out result))
                self[key] = result = computor();
            return result;
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
