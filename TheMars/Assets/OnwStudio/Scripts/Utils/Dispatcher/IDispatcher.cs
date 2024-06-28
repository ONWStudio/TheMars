using System;
using System.Collections.Generic;

namespace Onw.Dispatcher
{
    public interface IDispatcher
    {
        void Invoke(Action fn);
    }
}

