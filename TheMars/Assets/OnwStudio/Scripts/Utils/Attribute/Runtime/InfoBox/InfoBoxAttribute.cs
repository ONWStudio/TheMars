using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class InfoBoxAttribute : PropertyAttribute
    {
        public string Message { get; }
        public INFO_TYPE InfoType { get; }

        public InfoBoxAttribute(string message, INFO_TYPE infoType = INFO_TYPE.INFO)
        {
            Message = message;
            InfoType = infoType;
        }
    }

    public enum INFO_TYPE
    {
        INFO,
        WARNING,
        ERROR
    }
}