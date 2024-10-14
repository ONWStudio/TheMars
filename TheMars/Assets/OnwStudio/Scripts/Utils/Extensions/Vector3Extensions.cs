using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class Vector3Extensions
    {
        public static Color ToColor(this Vector3 vec) => new(vec.x, vec.y, vec.z);
    }
}

