using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Onw.Extensions
{
    public static class UniRxObserver
    {
        public static void ObserveInfomation<T>(MonoBehaviour monoBehaviour, Func<Unit, T> selector, Action<T> observer, int skip = 0)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(monoBehaviour);
        }

        public static void ObserveInfomation<T>(CompositeDisposable disposables, MonoBehaviour monoBehaviour, Func<Unit, T> selector, Action<T> observer, int skip = 0)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(disposables);
        }

        public static void ObserveInfomationWhere<T>(MonoBehaviour monoBehaviour, Func<Unit, T> selector, Action<T> observer, Func<T, bool> where, int skip = 0)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Where(where)
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(monoBehaviour);
        }

        public static void ObserveInfomationWhere<T>(CompositeDisposable disposables, MonoBehaviour monoBehaviour, Func<Unit, T> selector, Action<T> observer, Func<T, bool> where, int skip = 0)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Where(where)
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(disposables);
        }

        public static void ObserveInfomationBuffer<T, TR>(MonoBehaviour monoBehaviour, Func<Unit, T> selector, Func<IList<T>, TR> buffer, Action<TR> observer)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Buffer(2, 1)
                .Select(buffer)
                .Subscribe(observer)
                .AddTo(monoBehaviour);
        }

        public static void ObserveInfomationBuffer<T, TR>(CompositeDisposable disposables, MonoBehaviour monoBehaviour, Func<Unit, T> selector, Func<IList<T>, TR> buffer, Action<TR> observer)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Buffer(2, 1)
                .Select(buffer)
                .Subscribe(observer)
                .AddTo(disposables);
        }
    }
}
