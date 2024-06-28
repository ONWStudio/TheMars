using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Onw.Extensions
{
    public static class UniRxObserver
    {
        public static void ObserveInfomation<R>(MonoBehaviour monoBehaviour, Func<Unit, R> selector, Action<R> observer, int skip = 0)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(monoBehaviour);
        }

        public static void ObserveInfomation<R>(CompositeDisposable disposables, MonoBehaviour monoBehaviour, Func<Unit, R> selector, Action<R> observer, int skip = 0)
        {
            monoBehaviour
                .UpdateAsObservable()
                .Select(selector)
                .DistinctUntilChanged()
                .Skip(skip)
                .Subscribe(observer)
                .AddTo(disposables);
        }

        public static void ObserveInfomationWhere<R>(MonoBehaviour monoBehaviour, Func<Unit, R> selector, Action<R> observer, Func<R, bool> where, int skip = 0)
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

        public static void ObserveInfomationWhere<R>(CompositeDisposable disposables, MonoBehaviour monoBehaviour, Func<Unit, R> selector, Action<R> observer, Func<R, bool> where, int skip = 0)
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

        public static void ObserveInfomationBuffer<R, TR>(MonoBehaviour monoBehaviour, Func<Unit, R> selector, Func<IList<R>, TR> buffer, Action<TR> observer)
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

        public static void ObserveInfomationBuffer<R, TR>(CompositeDisposable disposables, MonoBehaviour monoBehaviour, Func<Unit, R> selector, Func<IList<R>, TR> buffer, Action<TR> observer)
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
