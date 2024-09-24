using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ComponentExtensions
    {
        public static void SetPositionX(this Component component, float x)
        {
            component.transform.position.Set(x, component.transform.position.y, component.transform.position.z);
        }
        
        public static void SetPositionY(this Component component, float y)
        {
            component.transform.position.Set(component.transform.position.x, y, component.transform.position.z);
        }

        public static void SetPositionZ(this Component component, float z)
        {
            component.transform.position.Set(component.transform.position.x, component.transform.position.y, z);
        }

        public static void SetLocalPositionX(this Component component, float x)
        {
            component.transform.localPosition.Set(x, component.transform.localPosition.y, component.transform.localPosition.z);
        }

        public static void SetLocalPositionY(this Component component, float y)
        {
            component.transform.localPosition.Set(component.transform.localPosition.x, y, component.transform.localPosition.z);
        }

        public static void SetLocalPositionZ(this Component component, float z)
        {
            component.transform.localPosition.Set(component.transform.localPosition.x, component.transform.localPosition.y, z);
        }
    }
}
