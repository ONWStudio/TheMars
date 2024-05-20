using System;
using System.Collections.Generic;

public sealed class Dispatcher : UpdatingClass, IDispatcher
{
    public static Dispatcher Instance { get; } = new Dispatcher();

    private readonly Stack<Action> _pending = new();
    private readonly List<Action> _loopPending = new();

    // Schedule code for execution in the main-thread.
    //
    public void Invoke(Action fn)
    {
        lock (_pending)
        {
            _pending.Push(fn);
        }
    }

    public void InvokeLoop(Action fn)
    {
        lock (_loopPending)
        {
            _loopPending.Add(fn);
        }
    }

    //
    // Execute pending actions.
    //
    private void invokePending()
    {
        lock (_pending)
        {
            while (_pending.Count > 0) _pending.Pop().Invoke();
        }
    }

    private void invokeLoopPending()
    {
        lock (_loopPending)
        {
            _loopPending.ForEach(action => action.Invoke());
        }
    }

    protected internal override void Update()
    {
        invokePending();
        invokeLoopPending();
    }

    private Dispatcher() {}
}