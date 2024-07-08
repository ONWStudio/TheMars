using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class TransformExtentions
    {
        public static void SetPositionX(this Transform transform, float x)
        {
            transform.position = new(x, transform.position.y, transform.position.z);
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            transform.position = new(transform.position.x, y, transform.position.z);
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            transform.position = new(transform.position.x, transform.position.y, z);
        }
    }
}