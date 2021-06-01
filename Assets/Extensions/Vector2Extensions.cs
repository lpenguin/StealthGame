using UnityEngine;

namespace Extensions
{
    public static class Vector2Extensions
    {
        public static Vector3 ToVector3(this Vector2 that)
        {
            return new Vector3(that.x, 0, that.y);
        }
        
        public static Vector2 ToVector2(this Vector3 that)
        {
            return new Vector2(that.x, that.z);
        }
    }
}