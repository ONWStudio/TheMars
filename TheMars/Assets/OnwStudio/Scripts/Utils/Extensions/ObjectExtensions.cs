using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ObjectExtensions
    {
        public static bool Is<TCast>(this object @class, Action<TCast> castCall)
        {
            if (@class is not TCast castClass) return false;
            
            castCall?.Invoke(castClass);
            return true;
        }
    }
}
