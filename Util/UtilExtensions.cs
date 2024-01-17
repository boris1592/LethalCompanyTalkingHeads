using UnityEngine;

namespace TalkingHeads.Util
{
    internal static class UtilExtensions
    {
        public static float MoveTowards(this float source, float target, float delta)
        {
            if (target > source) return Mathf.Min(target, source + delta);
            else if (target < source) return Mathf.Max(target, source - delta);

            return source;
        }

        public static Transform FindComponentInChildren(this Transform transform, string name)
        {
            foreach (Transform child in transform)
            {
                if (child.name == name) return child;

                var deeper = child.FindComponentInChildren(name);

                if (deeper is not null) return deeper;
            }

            return null;
        }
    }
}
