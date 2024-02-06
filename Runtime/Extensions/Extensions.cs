using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = System.Random;

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

        #region Rect
        public static Rect WithPosition(this Rect self, float x, float y)
        {
            self.x = x;
            self.y = y;

            return self;
        }

        public static Rect WithSize(this Rect self, float width, float height)
        {
            self.width = width;
            self.height = height;

            return self;
        }

        public static Rect WithPadding(this Rect self, float left, float right, float top, float bottom)
        {
            self.xMin += left;
            self.xMax -= right;
            self.yMin += bottom;
            self.yMax -= top;

            return self;
        }

        public static Rect Include(this Rect self, Rect other)
        {
            self.xMin = Mathf.Min(self.xMin, other.xMin);
            self.yMin = Mathf.Min(self.yMin, other.yMin);

            self.xMax = Mathf.Max(self.xMax, other.xMax);
            self.yMax = Mathf.Max(self.yMax, other.yMax);

            return self;
        }

        public static Rect Include(this Rect self, Vector2 point)
        {
            self.xMin = Mathf.Min(self.xMin, point.x);
            self.yMin = Mathf.Min(self.yMin, point.y);

            self.xMax = Mathf.Max(self.xMax, point.x);
            self.yMax = Mathf.Max(self.yMax, point.y);

            return self;
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
