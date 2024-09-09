using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ComponentExtensions
    {
        public static void SetPositionX(this Component component, float x)
        {
            component.transform.SetPositionX(x);
        }

        public static void SetPositionY(this Component component, float y)
        {
            component.transform.SetPositionY(y);
        }

        public static void SetPositionZ(this Component component, float z)
        {
            component.transform.SetPositionZ(z);
        }
    }
}
