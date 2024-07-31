using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Localization
{
    public interface ILocalizable
    {
        LocalizedStringOption StringOption { get; }
    }
}
