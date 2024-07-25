using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    using Attribute = System.Attribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SubstitutionAttribute : Attribute
    {
        public string Label { get; }

        public SubstitutionAttribute(string label)
        {
            Label = label;
        }
    }
}