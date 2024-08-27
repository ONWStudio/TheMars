using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Onw.Helper
{
    [System.Serializable]
    public struct SerializedRect
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public SerializedRect(in Rect rect)
        {
            X = rect.x;
            Y = rect.y;
            Width = rect.width;
            Height = rect.height;
        }

        public Rect ToRect()
        {
            return new(X, Y, Width, Height);
        }
    }
}

