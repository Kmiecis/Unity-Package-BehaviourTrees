using UnityEngine;

namespace Common.BehaviourTrees
{
    public static class BT_Time
    {
        public static float Timestamp
            => Time.realtimeSinceStartup * Time.timeScale;

        public static float TimestampUnscaled
            => Time.realtimeSinceStartup;

        public static float GetDeltaTime(ref float cached)
        {
            var nowstamp = Timestamp;
            var result = nowstamp - cached;
            cached = nowstamp;
            return result;
        }

        public static float GetDeltaTimeUnscaled(ref float cached)
        {
            var nowstamp = TimestampUnscaled;
            var result = nowstamp - cached;
            cached = nowstamp;
            return result;
        }
    }
}