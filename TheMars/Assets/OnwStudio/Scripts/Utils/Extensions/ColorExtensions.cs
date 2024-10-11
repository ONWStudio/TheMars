using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ColorExtensions
    {
        public static Vector3 ToVec3(this Color color) => new(color.r, color.g, color.b);
        public static Vector3 ToVec3(this Color32 color) => new(color.r, color.g, color.b);
    }
}
