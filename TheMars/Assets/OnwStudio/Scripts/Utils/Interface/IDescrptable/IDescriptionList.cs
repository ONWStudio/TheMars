using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Interface
{
    public interface IDescriptionList
    {
        IEnumerable<string> Descriptions { get; }
    }
}
