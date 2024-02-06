using UnityEngine;

namespace Common.BehaviourTrees
{
    internal static class BT_Time
    {
        public static float Nowstamp
            => Time.realtimeSinceStartup * Time.timeScale;

        public static float NowstampUnscaled
            => Time.realtimeSinceStartup;

        public static float GetDeltaTime(ref float cached)
        {
            var nowstamp = Nowstamp;
            var result = nowstamp - cached;
            cached = nowstamp;
            return result;
        }

        public static float GetDeltaTimeUnscaled(ref float cached)
        {
            var nowstamp = NowstampUnscaled;
            var result = nowstamp - cached;
            cached = nowstamp;
            return result;
        }
    }
}