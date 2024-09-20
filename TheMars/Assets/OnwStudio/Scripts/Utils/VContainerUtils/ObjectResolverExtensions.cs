using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Onw.Extensions;

namespace Onw.VContainerUtils
{
    public static class ObjectResolverExtensions
    {
        public static void InjectOnPost(this IObjectResolver objectResolver, object target)
        {
            objectResolver.Inject(target);
            target.Is<IPostInject>(postInject => postInject.PostInject(objectResolver));
        }
        //
        // public static void InjectOnPostForGameObject(this IObjectResolver objectResolver, GameObject gameObject)
        // {
        //     
        // }
    }
}
