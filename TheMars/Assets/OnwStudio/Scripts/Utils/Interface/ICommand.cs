using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Interface
{
    public interface ICommand<in T> where T : class
    {
        void Execute(T order);
    }
}
