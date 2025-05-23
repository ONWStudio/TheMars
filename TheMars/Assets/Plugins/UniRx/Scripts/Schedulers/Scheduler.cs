﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UniRx
{
    // Scheduler Extension
    public static partial class Scheduler
    {
        // configurable defaults
        public static class DefaultSchedulers
        {
            private static IScheduler constantTime;
            public static IScheduler ConstantTimeOperations
            {
                get
                {
                    return constantTime ?? (constantTime = Scheduler.Immediate);
                }
                set
                {
                    constantTime = value;
                }
            }

            private static IScheduler tailRecursion;
            public static IScheduler TailRecursion
            {
                get
                {
                    return tailRecursion ?? (tailRecursion = Scheduler.Immediate);
                }
                set
                {
                    tailRecursion = value;
                }
            }

            private static IScheduler iteration;
            public static IScheduler Iteration
            {
                get
                {
                    return iteration ?? (iteration = Scheduler.CurrentThread);
                }
                set
                {
                    iteration = value;
                }
            }

            private static IScheduler timeBasedOperations;
            public static IScheduler TimeBasedOperations
            {
                get
                {
#if UniRxLibrary
                    return timeBasedOperations ?? (timeBasedOperations = Scheduler.ThreadPool);
#else
                    return timeBasedOperations ?? (timeBasedOperations = Scheduler.MainThread); // MainThread as default for TimeBased Operation
#endif
                }
                set
                {
                    timeBasedOperations = value;
                }
            }

            private static IScheduler asyncConversions;
            public static IScheduler AsyncConversions
            {
                get
                {
#if WEB_GL
                    // WebGL does not support threadpool
                    return asyncConversions ?? (asyncConversions = Scheduler.MainThread);
#else
                    return asyncConversions ?? (asyncConversions = Scheduler.ThreadPool);
#endif
                }
                set
                {
                    asyncConversions = value;
                }
            }

            public static void SetDotNetCompatible()
            {
                ConstantTimeOperations = Scheduler.Immediate;
                TailRecursion = Scheduler.Immediate;
                Iteration = Scheduler.CurrentThread;
                TimeBasedOperations = Scheduler.ThreadPool;
                AsyncConversions = Scheduler.ThreadPool;
            }
        }

        // utils

        public static DateTimeOffset Now
        {
            get { return DateTimeOffset.UtcNow; }
        }

        public static TimeSpan Normalize(TimeSpan timeSpan)
        {
            return timeSpan >= TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
        }

        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
        {
            return scheduler.Schedule(dueTime - scheduler.Now, action);
        }

        public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
        {
            // InvokeRec1
            CompositeDisposable group = new CompositeDisposable(1);
            object gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(() =>
            {
                bool isAdded = false;
                bool isDone = false;
                IDisposable d = default(IDisposable);
                d = scheduler.Schedule(() =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }
                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(recursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
        {
            // InvokeRec2

            CompositeDisposable group = new CompositeDisposable(1);
            object gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(dt =>
            {
                bool isAdded = false;
                bool isDone = false;
                IDisposable d = default(IDisposable);
                d = scheduler.Schedule(dt, () =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }
                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(dueTime, recursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
        {
            // InvokeRec3

            CompositeDisposable group = new CompositeDisposable(1);
            object gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(dt =>
            {
                bool isAdded = false;
                bool isDone = false;
                IDisposable d = default(IDisposable);
                d = scheduler.Schedule(dt, () =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }
                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(dueTime, recursiveAction));

            return group;
        }
    }
}