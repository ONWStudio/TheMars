using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Onw.Interface
{
    public static class DescriptionBuilder
    {
        public static string BuildToString(this IEnumerable<IDescriptable> descriptables)
        {
            StringBuilder descriptionBuilder = new();
            descriptables.Select(descriptionBuilder.Append);

            return descriptionBuilder.ToString();
        }
    }
}

