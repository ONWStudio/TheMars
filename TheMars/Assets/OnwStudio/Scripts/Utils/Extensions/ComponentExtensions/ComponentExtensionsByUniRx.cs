using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ComponentExtensionsByUniRx
    {
        public static void ObserveInformation<TSource, TProperty>(this TSource component, Func<TSource, TProperty> selector, Action<TProperty> observer, int skip = 0) 
            where TSource : Component
        {
            component
                .ObserveEveryValueChanged(selector)
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(component);
        }

        public static void ObserveInformation<TSource, TProperty>(this TSource component, CompositeDisposable disposables, Func<TSource, TProperty> selector, Action<TProperty> observer, int skip = 0) 
            where TSource : Component
        {
            component
                .ObserveEveryValueChanged(selector)
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(disposables);
        }

        public static void ObserveInformationWhere<TSource, TProperty>(this TSource component, Func<TSource, TProperty> selector, Action<TProperty> observer, Func<TProperty, bool> where, int skip = 0)
            where TSource : Component
        {
            component
                .ObserveEveryValueChanged(selector)
                .Where(where)
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(component);
        }

        public static void ObserveInformationWhere<TSource, TProperty>(this TSource component, CompositeDisposable disposables, Func<TSource, TProperty> selector, Action<TProperty> observer, Func<TProperty, bool> where, int skip = 0)
            where TSource : Component
        {
            component
                .ObserveEveryValueChanged(selector)
                .Where(where)
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(disposables);
        }

        public static void ObserveInformationBuffer<TSource, TProperty, TBuffer>(this TSource component, Func<TSource, TProperty> selector, Func<IList<TProperty>, TBuffer> buffer, Action<TBuffer> observer)
            where TSource : Component
        {
            component
                .ObserveEveryValueChanged(selector)
                .Buffer(2, 1)
                .Select(buffer)
                .Subscribe(observer)
                .AddTo(component);
        }

        public static void ObserveInformationBuffer<TSource, TProperty, TBuffer>(this TSource component, CompositeDisposable disposables,  Func<TSource, TProperty> selector, Func<IList<TProperty>, TBuffer> buffer, Action<TBuffer> observer)
            where TSource : Component
        {
            component
                .ObserveEveryValueChanged(selector)
                .Buffer(2, 1)
                .Select(buffer)
                .Subscribe(observer)
                .AddTo(disposables);
        }
    }
}
