using System;

namespace Common.BehaviourTrees
{
    internal static class UTime
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public static float UtcNow
            => DateTime.UtcNow.ToTimestamp();

        public static float ToTimestamp(this DateTime self)
        {
            return (float)self.Subtract(Epoch).TotalSeconds;
        }
    }
}