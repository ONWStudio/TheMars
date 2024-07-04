using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public sealed class SpritePreviewAttribute : PropertyAttribute
    {
        public float Size { get; } = 0f;

        public SpritePreviewAttribute(float size = 64f)
        {
            Size = size;
        }
    }
}
